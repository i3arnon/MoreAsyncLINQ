namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PartialSortTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 100);

    [Fact]
    public async Task PartialSort_Compiles()
    {
        await _source.PartialSort(10).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSort_WithComparer_Compiles()
    {
        await _source.PartialSort(10, Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSort_WithDirection_Compiles()
    {
        await _source.PartialSort(10, OrderByDirection.Descending).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSort_WithComparerAndDirection_Compiles()
    {
        await _source.PartialSort(10, Comparer<int>.Default, OrderByDirection.Descending).ConsumeAsync();
    }
}

