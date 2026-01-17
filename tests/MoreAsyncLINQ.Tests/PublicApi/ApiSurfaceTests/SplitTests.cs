namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class SplitTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Split_ByElement_Compiles()
    {
        await _source.Split(5).ConsumeAsync();
    }

    [Fact]
    public async Task Split_ByElementWithComparer_Compiles()
    {
        await _source.Split(5, EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task Split_ByElementWithCount_Compiles()
    {
        await _source.Split(5, 3).ConsumeAsync();
    }

    [Fact]
    public async Task Split_ByElementWithComparerAndCount_Compiles()
    {
        await _source.Split(5, EqualityComparer<int>.Default, 3).ConsumeAsync();
    }

    [Fact]
    public async Task Split_SyncPredicate_Compiles()
    {
        await _source.Split(x => x % 3 == 0).ConsumeAsync();
    }

    [Fact]
    public async Task Split_SyncPredicateWithCount_Compiles()
    {
        await _source.Split(x => x % 3 == 0, 3).ConsumeAsync();
    }

    [Fact]
    public async Task Split_AsyncPredicate_Compiles()
    {
        await _source.Split((x, _) => ValueTask.FromResult(x % 3 == 0)).ConsumeAsync();
    }

    [Fact]
    public async Task Split_AsyncPredicateWithCount_Compiles()
    {
        await _source.Split((x, _) => ValueTask.FromResult(x % 3 == 0), 3).ConsumeAsync();
    }
}

