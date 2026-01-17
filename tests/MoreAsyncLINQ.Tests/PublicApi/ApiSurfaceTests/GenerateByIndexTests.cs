namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class GenerateByIndexTests
{
    [Fact]
    public async Task GenerateByIndex_Async_Compiles()
    {
        await MoreAsyncEnumerable.GenerateByIndex((i, _) => ValueTask.FromResult(i * 2)).Take(5).ConsumeAsync();
    }
}
