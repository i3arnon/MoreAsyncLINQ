using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CodeAnalysis;

namespace MoreAsyncLINQ.Generators;

[Generator]
public class AggregateGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context) =>
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            var source = GenerateOverloads();
            postInitializationContext.AddSource("MoreAsyncEnumerable.Aggregate.g.cs", source);
        });

    private string GenerateOverloads()
    {
        using var stringWriter = new StringWriter();
        using var writer = Writers.CreateSourceWriter(stringWriter);

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

            writer.WriteLine("return source.IsKnownEmpty()");
            writer.Indent++;
            writer.WriteLine("? ValueTasks.FromResult(");
            writer.Indent++;
            writer.WriteLine("resultSelector(");
            writer.Indent++;
            WriteSeedArgs(writer, arity);
            writer.WriteLine("))");
            writer.Indent--;
            writer.Indent--;
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

            writer.WriteLine("return Core(");
            writer.Indent++;
            WriteCoreCallArgs(writer, arity);
            writer.WriteLine(");");
            writer.Indent--;

            writer.WriteLine();
            WriteCoreMethod(writer, arity, isAsync: true);
        }
    }

    private void WriteXmlDocumentation(IndentedTextWriter writer, int arity)
    {
        writer = Writers.CreateXmlDocWriter(writer);

        writer.WriteLine("<summary>");
        writer.WriteLine($"Applies {Writers.Ordinals[arity - 1]} accumulators sequentially in a single pass over a");
        writer.WriteLine("sequence.");
        writer.WriteLine("</summary>");
        writer.WriteLine("<typeparam name=\"TSource\">The type of elements in <paramref name=\"source\"/>.</typeparam>");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"<typeparam name=\"TAccumulate{index}\">The type of {Writers.Ordinals[index - 1]} accumulator value.</typeparam>");
        }

        writer.WriteLine("<typeparam name=\"TResult\">The type of the accumulated result.</typeparam>");
        writer.WriteLine("<param name=\"source\">The source sequence</param>");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"<param name=\"seed{index}\">The seed value for the {Writers.Ordinals[index - 1]} accumulator.</param>");
            writer.WriteLine($"<param name=\"accumulator{index}\">The {Writers.Ordinals[index - 1]} accumulator.</param>");
        }

        writer.WriteLine("<param name=\"resultSelector\">");
        writer.WriteLine("A function that projects a single result given the result of each");
        writer.WriteLine("accumulator.</param>");
        writer.WriteLine("<param name=\"cancellationToken\">The optional cancellation token to be used for cancelling the sequence at any time.</param>");
        writer.WriteLine("<returns>The value returned by <paramref name=\"resultSelector\"/>.</returns>");
        writer.WriteLine("<remarks>");
        writer.WriteLine("This operator executes immediately.");
        writer.WriteLine("</remarks>");
    }

    private void WriteMethodSignature(IndentedTextWriter writer, int arity, bool isAsync)
    {
        writer.WriteLine("public static ValueTask<TResult> AggregateAsync<");

        using var _ = writer.BeginIndentScope();

        // Type parameters
        writer.WriteLine("TSource,");
        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"TAccumulate{index},");
        }
        writer.WriteLine("TResult>(");

        // Method parameters
        writer.WriteLine("this IAsyncEnumerable<TSource> source,");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"TAccumulate{index} seed{index},");

            var funcType =
                Writers.GetFuncType(
                    [$"TAccumulate{index}", "TSource"],
                    $"TAccumulate{index}",
                    isAsync);

            writer.WriteLine($"{funcType} accumulator{index},");
        }

        var resultSelectorType =
            Writers.GetFuncType(
                Writers.GetTypeParams(arity, "TAccumulate"),
                "TResult",
                isAsync);

        writer.WriteLine($"{resultSelectorType} resultSelector,");
        writer.WriteLine("CancellationToken cancellationToken = default)");
    }

    private void WriteNullChecks(IndentedTextWriter writer, int arity)
    {
        Writers.WriteNullCheck(writer, "source");

        for (var index = 1; index <= arity; index++)
        {
            Writers.WriteNullCheck(writer, $"accumulator{index}");
        }

        Writers.WriteNullCheck(writer, "resultSelector");
    }

    private void WriteCoreCallArgs(IndentedTextWriter writer, int arity)
    {
        writer.WriteLine("source,");

        for (var index = 1; index <= arity; index++)
        {
            writer.WriteLine($"seed{index},");
            writer.WriteLine($"accumulator{index},");
        }

        writer.WriteLine("resultSelector,");
        writer.Write("cancellationToken");
    }

    private void WriteSeedArgs(IndentedTextWriter writer, int arity)
    {
        for (var index = 1; index <= arity; index++)
        {
            writer.Write($"seed{index}");

            if (index < arity)
            {
                writer.WriteLine(",");
            }
        }
    }

    private void WriteCoreMethod(IndentedTextWriter writer, int arity, bool isAsync)
    {
        writer.WriteLine("static async ValueTask<TResult> Core(");

        using (writer.BeginIndentScope())
        {
            writer.WriteLine("IAsyncEnumerable<TSource> source,");

            for (var index = 1; index <= arity; index++)
            {
                writer.WriteLine($"TAccumulate{index} seed{index},");

                var funcType =
                    Writers.GetFuncType(
                        [$"TAccumulate{index}", "TSource"],
                        $"TAccumulate{index}",
                        isAsync);

                writer.WriteLine($"{funcType} accumulator{index},");
            }

            var resultSelectorType =
                Writers.GetFuncType(
                    Writers.GetTypeParams(arity, "TAccumulate"),
                    "TResult",
                    isAsync);

            writer.WriteLine($"{resultSelectorType} resultSelector,");
            writer.WriteLine("CancellationToken cancellationToken)");
        }

        using (writer.BeginBracketsScope())
        using (writer.BeginIndentScope())
        {
            for (var index = 1; index <= arity; index++)
            {
                writer.WriteLine($"var accumulate{index} = seed{index};");
            }

            writer.WriteLine();
            writer.WriteLine("await foreach (var element in source.WithCancellation(cancellationToken))");
            using (writer.BeginBracketsScope())
            using (writer.BeginIndentScope())
            {
                for (var index = 1; index <= arity; index++)
                {
                    writer.WriteLine(
                        isAsync
                            ? $"accumulate{index} = await accumulator{index}(accumulate{index}, element, cancellationToken);"
                            : $"accumulate{index} = accumulator{index}(accumulate{index}, element);");
                }
            }

            writer.WriteLine();

            if (isAsync)
            {
                writer.WriteLine("return await resultSelector(");
                writer.Indent++;

                for (var index = 1; index <= arity; index++)
                {
                    writer.WriteLine($"accumulate{index},");
                }

                writer.WriteLine("cancellationToken);");
            }
            else
            {
                writer.WriteLine("return resultSelector(");
                writer.Indent++;

                for (var index = 1; index <= arity; index++)
                {
                    writer.WriteLine(
                        index < arity
                            ? $"accumulate{index},"
                            : $"accumulate{index});");
                }
            }

            writer.Indent--;
        }
    }
}
