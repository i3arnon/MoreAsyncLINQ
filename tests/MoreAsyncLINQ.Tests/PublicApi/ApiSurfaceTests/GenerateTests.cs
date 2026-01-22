namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class GenerateTests
{
    [Fact]
    public async Task Generate_Compiles()
    {
        await MoreAsyncEnumerable.Generate(1, (x, _) => ValueTask.FromResult(x + 1)).Take(5).ConsumeAsync();
    }
}


