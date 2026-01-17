namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class UnfoldTests
{
    [Fact]
    public async Task Unfold_Async_Compiles()
    {
        await MoreAsyncEnumerable.Unfold(
            1,
            (state, _) => ValueTask.FromResult(state),
            (t, _) => ValueTask.FromResult(t <= 10),
            (t, _) => ValueTask.FromResult(t + 1),
            (t, _) => ValueTask.FromResult(t)).ConsumeAsync();
    }
}
