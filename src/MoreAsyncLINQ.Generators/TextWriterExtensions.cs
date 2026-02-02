using System;
using System.CodeDom.Compiler;
using System.IO;

namespace MoreAsyncLINQ.Generators;

public static class TextWriterExtensions
{
    extension(TextWriter writer)
    {
        public IDisposable BeginBracketsScope() => writer.BeginScope("{", "}");

        public IDisposable BeginScope(string opening, string closing)
        {
            writer.WriteLine(opening);

            return new WriterScope(writer, closing);
        }
    }

    extension(IndentedTextWriter writer)
    {
        public IDisposable BeginIndentScope()
        {
            writer.Indent++;

            return new IndentScope(writer);
        }
    }

    private class WriterScope(TextWriter writer, string closing) : IDisposable
    {
        public void Dispose() => writer.WriteLine(closing);
    }

    private class IndentScope(IndentedTextWriter writer) : IDisposable
    {
        public void Dispose() => writer.Indent--;
    }
}