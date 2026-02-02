using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CodeAnalysis;

namespace MoreAsyncLINQ.Generators;

[Generator]
public class CartesianGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context) =>
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            var source = GenerateOverloads();
            postInitializationContext.AddSource("MoreAsyncEnumerable.Cartesian.g.cs", source);
        });

    private string GenerateOverloads()
    {
        using var stringWriter = new StringWriter();
        using var writer = Writers.CreateSourceWriter(stringWriter, "System.Runtime.CompilerServices");

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            for (var arity = 2; arity <= 8; arity++)
            {
                GenerateSyncOverload(writer, arity);
                writer.WriteLine();
                GenerateAsyncOverload(writer, arity);

                if (arity < 8)
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
            WriteNullChecks(writer, arity);
            writer.WriteLine();

            WriteEmptyCheck(writer, arity);
            writer.Indent++;
            writer.WriteLine("? AsyncEnumerable.Empty<TResult>()");
            writer.WriteLine(": Core(");
            writer.Indent++;
            WriteCoreCallArgs(writer, arity);
            writer.WriteLine(");");
            writer.Indent--;
            writer.Indent--;

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
            WriteNullChecks(writer, arity);
            writer.WriteLine();

            WriteEmptyCheck(writer, arity);
            writer.Indent++;
            writer.WriteLine("? AsyncEnumerable.Empty<TResult>()");
            writer.WriteLine(": Core(");
            writer.Indent++;
            WriteCoreCallArgs(writer, arity);
            writer.WriteLine(");");
            writer.Indent--;
            writer.Indent--;

            writer.WriteLine();
            WriteCoreMethod(writer, arity, isAsync: true);
        }
    }

    private void WriteXmlDocumentation(IndentedTextWriter writer, int arity)
    {
        writer = Writers.CreateXmlDocWriter(writer);

        writer.WriteLine("<summary>");
        writer.WriteLine($"Returns the Cartesian product of {Writers.Ordinals[arity - 1]} sequences by enumerating all");
        writer.WriteLine("possible combinations of one item from each sequence, and applying");
        writer.WriteLine("a user-defined projection to the items in a given combination.");
        writer.WriteLine("</summary>");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"<typeparam name=\"T{index}\">");
            writer.WriteLine($"The type of the elements of <paramref name=\"{Writers.Ordinals[index - 1]}\"/>.</typeparam>");
        }

        writer.WriteLine("<typeparam name=\"TResult\">");
        writer.WriteLine("The type of the elements of the result sequence.</typeparam>");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"<param name=\"{Writers.Ordinals[index - 1]}\">The {Writers.Ordinals[index - 1]} sequence of elements.</param>");
        }

        writer.WriteLine("<param name=\"resultSelector\">A projection function that combines");
        writer.WriteLine("elements from all the sequences.</param>");
        writer.WriteLine("<returns>A sequence of elements returned by");
        writer.WriteLine("<paramref name=\"resultSelector\"/>.</returns>");
        writer.WriteLine("<remarks>");
        writer.WriteLine("<para>");
        writer.WriteLine("The method returns items in the same order as a nested foreach");
        writer.WriteLine("loop, but all sequences except for <paramref name=\"first\"/> are");
        writer.WriteLine("cached when iterated over. The cache is then re-used for any");
        writer.WriteLine("subsequent iterations.</para>");
        writer.WriteLine("<para>");
        writer.WriteLine("This method uses deferred execution and stream its results.</para>");
        writer.WriteLine("</remarks>");
    }

    private void WriteMethodSignature(IndentedTextWriter writer, int arity, bool isAsync)
    {
        writer.WriteLine("public static IAsyncEnumerable<TResult> Cartesian<");

        using var _ = writer.BeginIndentScope();

        // Type parameters
        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"T{index},");
        }
        writer.WriteLine("TResult>(");

        // Method parameters - sequences
        for (var index = 1; index <= arity; index++)
        {
            var extensionThis = index == 1 ? "this " : "";
            writer.WriteLine($"{extensionThis}IAsyncEnumerable<T{index}> {Writers.Ordinals[index - 1]},");
        }

        // Result selector
        var resultSelectorType =
            Writers.GetFuncType(
                Writers.GetTypeParams(arity),
                "TResult",
                isAsync);

        writer.WriteLine($"{resultSelectorType} resultSelector)");
    }

    private void WriteNullChecks(IndentedTextWriter writer, int arity)
    {
        for (var index = 1; index <= arity; index++)
        {
            Writers.WriteNullCheck(writer, Writers.Ordinals[index - 1]);
        }

        Writers.WriteNullCheck(writer, "resultSelector");
    }

    private void WriteEmptyCheck(IndentedTextWriter writer, int arity)
    {
        writer.Write("return ");
        for (var index = 1; index <= arity; index++)
        {
            writer.Write($"{Writers.Ordinals[index - 1]}.IsKnownEmpty()");
            if (index < arity)
            {
                writer.WriteLine(" &&");
                writer.Write("       ");
            }
        }
        writer.WriteLine();
    }

    private void WriteCoreCallArgs(IndentedTextWriter writer, int arity)
    {
        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"{Writers.Ordinals[index - 1]},");
        }

        writer.WriteLine("resultSelector,");
        writer.Write("default");
    }

    private void WriteCoreMethod(IndentedTextWriter writer, int arity, bool isAsync)
    {
        writer.WriteLine("static async IAsyncEnumerable<TResult> Core(");

        using (writer.BeginIndentScope())
        {
            for (var index = 1; index <= arity; index++)
            {
                writer.WriteLine($"IAsyncEnumerable<T{index}> {Writers.Ordinals[index - 1]},");
            }

            var resultSelectorType =
                Writers.GetFuncType(
                    Writers.GetTypeParams(arity),
                    "TResult",
                    isAsync);

            writer.WriteLine($"{resultSelectorType} resultSelector,");
            writer.WriteLine("[EnumeratorCancellation] CancellationToken cancellationToken)");
        }

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            // Memoize all sequences except first
            for (var index = 2; index <= arity; index++)
            {
                writer.WriteLine($"await using var {Writers.Ordinals[index - 1]}Memo = {Writers.Ordinals[index - 1]}.Memoize();");
            }

            writer.WriteLine();

            // Nested foreach loops
            writer.WriteLine("await foreach (var firstElement in first.WithCancellation(cancellationToken))");
            for (var index = 2; index <= arity; index++)
            {
                writer.WriteLine($"await foreach (var {Writers.Ordinals[index - 1]}Element in {Writers.Ordinals[index - 1]}Memo.WithCancellation(cancellationToken))");
            }

            using (writer.BeginBracketsScope())
            using (writer.BeginIndentScope())
            {
                writer.WriteLine(
                    isAsync
                        ? "yield return await resultSelector("
                        : "yield return resultSelector(");

                using (writer.BeginIndentScope())
                {
                    for (var index = 1; index <= arity; index++)
                    {
                        var elementName = $"{Writers.Ordinals[index - 1]}Element";
                        if (isAsync)
                        {
                            writer.WriteLine($"{elementName},");
                        }
                        else
                        {
                            writer.WriteLine(index < arity ? $"{elementName}," : $"{elementName});");
                        }
                    }

                    if (isAsync)
                    {
                        writer.WriteLine("cancellationToken);");
                    }
                }
            }
        }
    }
}
