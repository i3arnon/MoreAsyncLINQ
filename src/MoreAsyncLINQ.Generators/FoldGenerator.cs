using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CodeAnalysis;

namespace MoreAsyncLINQ.Generators;

[Generator]
public class FoldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context) =>
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            var source = GenerateOverloads();
            postInitializationContext.AddSource("MoreAsyncEnumerable.Fold.g.cs", source);
        });

    private string GenerateOverloads()
    {
        using var stringWriter = new StringWriter();
        using var writer = Writers.CreateSourceWriter(stringWriter);

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            // Sync overloads: arity 1-16
            for (var arity = 1; arity <= 16; arity++)
            {
                GenerateSyncOverload(writer, arity);
                writer.WriteLine();
            }

            // Async overloads: arity 1-15 (limited by Func delegate having max 16 input params,
            // and async needs CancellationToken as additional input)
            for (var arity = 1; arity <= 15; arity++)
            {
                GenerateAsyncOverload(writer, arity);

                if (arity < 15)
                {
                    writer.WriteLine();
                }
            }
        }

        return stringWriter.ToString();
    }

    private void GenerateSyncOverload(IndentedTextWriter writer, int arity)
    {
        WriteXmlDocumentation(writer, arity);
        WriteMethodSignature(writer, arity, isAsync: false);

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            WriteNullChecks(writer);
            writer.WriteLine();

            writer.WriteLine("return Core(source, folder, cancellationToken);");
            writer.WriteLine();
            WriteCoreMethod(writer, arity, isAsync: false);
        }
    }

    private void GenerateAsyncOverload(IndentedTextWriter writer, int arity)
    {
        WriteXmlDocumentation(writer, arity);
        WriteMethodSignature(writer, arity, isAsync: true);

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            WriteNullChecks(writer);
            writer.WriteLine();

            writer.WriteLine("return Core(source, folder, cancellationToken);");
            writer.WriteLine();
            WriteCoreMethod(writer, arity, isAsync: true);
        }
    }

    private void WriteXmlDocumentation(IndentedTextWriter writer, int arity)
    {
        writer = Writers.CreateXmlDocWriter(writer);

        writer.WriteLine("<summary>");
        writer.WriteLine("Returns the result of applying a function to a sequence of");
        writer.WriteLine($"{arity} element{(arity == 1 ? "" : "s")}.");
        writer.WriteLine("</summary>");
        writer.WriteLine("<remarks>");
        writer.WriteLine("This operator uses immediate execution and effectively buffers");
        writer.WriteLine("as many items of the source sequence as necessary.");
        writer.WriteLine("</remarks>");
        writer.WriteLine("<typeparam name=\"TSource\">Type of element in the source sequence</typeparam>");
        writer.WriteLine("<typeparam name=\"TResult\">Type of the result</typeparam>");
        writer.WriteLine("<param name=\"source\">The sequence of items to fold.</param>");
        writer.WriteLine("<param name=\"folder\">Function to apply to the elements in the sequence.</param>");
        writer.WriteLine("<param name=\"cancellationToken\">The optional cancellation token to be used for cancelling the sequence at any time.</param>");
        writer.WriteLine("<returns>The folded value returned by <paramref name=\"folder\"/>.</returns>");
        writer.WriteLine("<exception cref=\"ArgumentNullException\"><paramref name=\"source\"/> is null</exception>");
        writer.WriteLine("<exception cref=\"ArgumentNullException\"><paramref name=\"folder\"/> is null</exception>");
        writer.WriteLine($"<exception cref=\"InvalidOperationException\"><paramref name=\"source\"/> does not contain exactly {arity} element{(arity == 1 ? "" : "s")}</exception>");
    }

    private void WriteMethodSignature(IndentedTextWriter writer, int arity, bool isAsync)
    {
        var folderType = GetFolderFuncType(arity, isAsync);

        writer.WriteLine("public static ValueTask<TResult> FoldAsync<TSource, TResult>(");

        using var _ = writer.BeginIndentScope();

        writer.WriteLine("this IAsyncEnumerable<TSource> source,");
        writer.WriteLine($"{folderType} folder,");
        writer.WriteLine("CancellationToken cancellationToken = default)");
    }

    private void WriteNullChecks(IndentedTextWriter writer)
    {
        Writers.WriteNullCheck(writer, "source");
        Writers.WriteNullCheck(writer, "folder");
    }

    private void WriteCoreMethod(IndentedTextWriter writer, int arity, bool isAsync)
    {
        var folderType = GetFolderFuncType(arity, isAsync);

        writer.WriteLine("static async ValueTask<TResult> Core(");

        using (writer.BeginIndentScope())
        {
            writer.WriteLine("IAsyncEnumerable<TSource> source,");
            writer.WriteLine($"{folderType} folder,");
            writer.WriteLine("CancellationToken cancellationToken)");
        }

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            writer.WriteLine($"var elements = await GetFoldElementsAsync(source, count: {arity}, cancellationToken);");
            writer.WriteLine(isAsync ? "return await folder(" : "return folder(");

            using (writer.BeginIndentScope())
            {
                for (var index = 0; index < arity; index++)
                {
                    var isLast = index == arity - 1;

                    if (isAsync)
                    {
                        writer.WriteLine($"elements[{index}],");
                    }
                    else
                    {
                        writer.WriteLine(isLast ? $"elements[{index}]);" : $"elements[{index}],");
                    }
                }

                if (isAsync)
                {
                    writer.WriteLine("cancellationToken);");
                }
            }
        }
    }

    private static string GetFolderFuncType(int arity, bool isAsync)
    {
        var sourceParams = string.Join(", ", System.Linq.Enumerable.Repeat("TSource", arity));

        return isAsync
            ? $"Func<{sourceParams}, CancellationToken, ValueTask<TResult>>"
            : $"Func<{sourceParams}, TResult>";
    }
}

