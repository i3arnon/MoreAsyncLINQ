namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class SortedMergeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<int> _other1 = AsyncEnumerable.Range(3, 5);
    private static readonly IAsyncEnumerable<int> _other2 = AsyncEnumerable.Range(6, 5);

    [Fact]
    public async Task SortedMerge_Compiles()
    {
        await _source.SortedMerge(OrderByDirection.Ascending, _other1).ConsumeAsync();
    }

    [Fact]
    public async Task SortedMerge_WithComparer_Compiles()
    {
        await _source.SortedMerge(OrderByDirection.Ascending, Comparer<int>.Default, _other1, _other2).ConsumeAsync();
    }
}

