namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class FillForwardTests
{
    private static readonly IAsyncEnumerable<int?> _source = AsyncEnumerable.Range(0, 10).Select(_ => (int?)null);

    [Fact]
    public async Task FillForward_Compiles()
    {
        await _source.FillForward().ConsumeAsync();
    }

    [Fact]
    public async Task FillForward_SyncPredicate_Compiles()
    {
        await _source.FillForward(x => x is null).ConsumeAsync();
    }

    [Fact]
    public async Task FillForward_SyncPredicateAndFillSelector_Compiles()
    {
        await _source.FillForward(x => x is null, (_, prev) => prev).ConsumeAsync();
    }

    [Fact]
    public async Task FillForward_AsyncPredicate_Compiles()
    {
        await _source.FillForward((x, _) => ValueTask.FromResult(x is null)).ConsumeAsync();
    }

    [Fact]
    public async Task FillForward_AsyncPredicateAndFillSelector_Compiles()
    {
        await _source.FillForward(
                (x, _) => ValueTask.FromResult(x is null),
                (_, prev, _) => ValueTask.FromResult(prev)).
            ConsumeAsync();
    }
}