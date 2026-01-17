namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class LagTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Lag_Sync_Compiles()
    {
        await _source.Lag(2, (current, lag) => $"{current}:{lag}").ConsumeAsync();
    }

    [Fact]
    public async Task Lag_SyncWithDefault_Compiles()
    {
        await _source.Lag(2, -1, (current, lag) => $"{current}:{lag}").ConsumeAsync();
    }

    [Fact]
    public async Task Lag_Async_Compiles()
    {
        await _source.Lag(2, (current, lag, _) => ValueTask.FromResult($"{current}:{lag}")).ConsumeAsync();
    }

    [Fact]
    public async Task Lag_AsyncWithDefault_Compiles()
    {
        await _source.Lag(2, -1, (current, lag, _) => ValueTask.FromResult($"{current}:{lag}")).ConsumeAsync();
    }
}

