namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class RankTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Rank_Compiles()
    {
        await _source.Rank().ConsumeAsync();
    }

    [Fact]
    public async Task Rank_WithComparer_Compiles()
    {
        await _source.Rank(Comparer<int>.Default).ConsumeAsync();
    }
}
