namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class TraceTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);

    [Fact]
    public async Task Trace_Compiles()
    {
        await _source.Trace().ConsumeAsync();
    }

    [Fact]
    public async Task Trace_WithFormat_Compiles()
    {
        await _source.Trace("Item: {0}").ConsumeAsync();
    }

    [Fact]
    public async Task Trace_SyncFormatter_Compiles()
    {
        await _source.Trace(x => $"Item: {x}").ConsumeAsync();
    }

    [Fact]
    public async Task Trace_AsyncFormatter_Compiles()
    {
        await _source.Trace((x, _) => ValueTask.FromResult($"Item: {x}")).ConsumeAsync();
    }
}


