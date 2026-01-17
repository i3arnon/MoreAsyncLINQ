namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class FillBackwardTests
{
    private static readonly IAsyncEnumerable<int?> _source = AsyncEnumerable.Range(0, 10).Select(_ => (int?)null);

    [Fact]
    public async Task FillBackward_Compiles()
    {
        await _source.FillBackward().ConsumeAsync();
    }

    [Fact]
    public async Task FillBackward_SyncPredicate_Compiles()
    {
        await _source.FillBackward(x => x is null).ConsumeAsync();
    }

    [Fact]
    public async Task FillBackward_SyncPredicateAndFillSelector_Compiles()
    {
        await _source.FillBackward(x => x is null, (_, next) => next).ConsumeAsync();
    }

    [Fact]
    public async Task FillBackward_AsyncPredicate_Compiles()
    {
        await _source.FillBackward((x, _) => ValueTask.FromResult(x is null)).ConsumeAsync();
    }

    [Fact]
    public async Task FillBackward_AsyncPredicateAndFillSelector_Compiles()
    {
        await _source.FillBackward(
                (x, _) => ValueTask.FromResult(x is null),
                (_, next, _) => ValueTask.FromResult(next)).
            ConsumeAsync();
    }
}

