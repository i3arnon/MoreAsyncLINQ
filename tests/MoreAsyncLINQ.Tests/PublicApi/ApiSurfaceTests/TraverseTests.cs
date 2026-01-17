namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class TraverseTests
{
    [Fact]
    public async Task TraverseBreadthFirst_Compiles()
    {
        await MoreAsyncEnumerable.TraverseBreadthFirst(
                0,
                x => x < 5
                    ? AsyncEnumerable.Range(x + 1, 1)
                    : AsyncEnumerable.Empty<int>()).
            ConsumeAsync();
    }

    [Fact]
    public async Task TraverseDepthFirst_Compiles()
    {
        await MoreAsyncEnumerable.TraverseDepthFirst(
                0,
                x => x < 5
                    ? AsyncEnumerable.Range(x + 1, 1)
                    : AsyncEnumerable.Empty<int>()).
            ConsumeAsync();
    }
}
