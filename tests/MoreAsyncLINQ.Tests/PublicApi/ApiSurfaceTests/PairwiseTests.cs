namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PairwiseTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Pairwise_Sync_Compiles()
    {
        await _source.Pairwise((a, b) => a + b).ConsumeAsync();
    }

    [Fact]
    public async Task Pairwise_Async_Compiles()
    {
        await _source.Pairwise((a, b, _) => ValueTask.FromResult(a + b)).ConsumeAsync();
    }
}


