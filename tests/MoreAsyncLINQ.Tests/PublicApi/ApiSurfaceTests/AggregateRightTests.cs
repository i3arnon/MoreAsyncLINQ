namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class AggregateRightTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task AggregateRightAsync_NoSeed_Sync_Compiles()
    {
        await _source.AggregateRightAsync((x, acc) => x + acc);
        await _source.AggregateRightAsync((x, acc) => x + acc, CancellationToken.None);
    }

    [Fact]
    public async Task AggregateRightAsync_NoSeed_Async_Compiles()
    {
        await _source.AggregateRightAsync((x, acc, _) => ValueTask.FromResult(x + acc));
        await _source.AggregateRightAsync((x, acc, _) => ValueTask.FromResult(x + acc), CancellationToken.None);
    }

    [Fact]
    public async Task AggregateRightAsync_WithSeed_Sync_Compiles()
    {
        await _source.AggregateRightAsync(seed: 0, func: (x, acc) => x + acc);
        await _source.AggregateRightAsync(seed: 0, func: (x, acc) => x + acc, cancellationToken: CancellationToken.None);
    }

    [Fact]
    public async Task AggregateRightAsync_WithSeed_Async_Compiles()
    {
        await _source.AggregateRightAsync(seed: 0, func: (x, acc, _) => ValueTask.FromResult(x + acc));
        await _source.AggregateRightAsync(seed: 0, func: (x, acc, _) => ValueTask.FromResult(x + acc), cancellationToken: CancellationToken.None);
    }

    [Fact]
    public async Task AggregateRightAsync_WithSeedAndResultSelector_Sync_Compiles()
    {
        await _source.AggregateRightAsync(seed: 0, func: (x, acc) => x + acc, resultSelector: acc => acc.ToString());
        await _source.AggregateRightAsync(seed: 0, func: (x, acc) => x + acc, resultSelector: acc => acc.ToString(), cancellationToken: CancellationToken.None);
    }

    [Fact]
    public async Task AggregateRightAsync_WithSeedAndResultSelector_Async_Compiles()
    {
        await _source.AggregateRightAsync(
            seed: 0, 
            func: (x, acc, _) => ValueTask.FromResult(x + acc), 
            resultSelector: (acc, _) => ValueTask.FromResult(acc.ToString()));
        await _source.AggregateRightAsync(
            seed: 0, 
            func: (x, acc, _) => ValueTask.FromResult(x + acc), 
            resultSelector: (acc, _) => ValueTask.FromResult(acc.ToString()), 
            cancellationToken: CancellationToken.None);
    }
}

