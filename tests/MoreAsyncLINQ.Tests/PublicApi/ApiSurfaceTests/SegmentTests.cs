namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class SegmentTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Segment_SyncPredicate_Compiles()
    {
        await _source.Segment(x => x % 3 == 0).ConsumeAsync();
    }

    [Fact]
    public async Task Segment_SyncPredicateWithIndex_Compiles()
    {
        await _source.Segment((_, i) => i % 3 == 0).ConsumeAsync();
    }

    [Fact]
    public async Task Segment_SyncPredicateWithIndexAndPrevious_Compiles()
    {
        await _source.Segment((current, previous, _) => current > previous).ConsumeAsync();
    }

    [Fact]
    public async Task Segment_AsyncPredicate_Compiles()
    {
        await _source.Segment((x, _) => ValueTask.FromResult(x % 3 == 0)).ConsumeAsync();
    }

    [Fact]
    public async Task Segment_AsyncPredicateWithIndex_Compiles()
    {
        await _source.Segment((_, i, _) => ValueTask.FromResult(i % 3 == 0)).ConsumeAsync();
    }

    [Fact]
    public async Task Segment_AsyncPredicateWithIndexAndPrevious_Compiles()
    {
        await _source.Segment((current, previous, _, _) => ValueTask.FromResult(current > previous)).ConsumeAsync();
    }
}

