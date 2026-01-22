namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PipeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Pipe_Sync_Compiles()
    {
        await _source.Pipe(Console.WriteLine).ConsumeAsync();
    }

    [Fact]
    public async Task Pipe_Async_Compiles()
    {
        await _source.Pipe((x, _) => ValueTask.CompletedTask).ConsumeAsync();
    }
}


