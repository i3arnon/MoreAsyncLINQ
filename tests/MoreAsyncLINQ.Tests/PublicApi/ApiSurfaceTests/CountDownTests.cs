namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class CountDownTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task CountDown_Compiles()
    {
        await _source.CountDown(10).ConsumeAsync();
    }

    [Fact]
    public async Task CountDown_WithResultSelector_Sync_Compiles()
    {
        await _source.CountDown(10, (element, countdown) => $"{element}:{countdown}").ConsumeAsync();
    }

    [Fact]
    public async Task CountDown_WithResultSelector_Async_Compiles()
    {
        await _source.CountDown(10, (element, countdown, _) => ValueTask.FromResult($"{element}:{countdown}")).ConsumeAsync();
    }
}

