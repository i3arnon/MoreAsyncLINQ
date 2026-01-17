namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class AggregateTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task AggregateAsync_TwoAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            resultSelector: (acc1, acc2) => acc1 + acc2);
    }

    [Fact]
    public async Task AggregateAsync_TwoAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, _) => ValueTask.FromResult(acc1 + acc2));
    }

    [Fact]
    public async Task AggregateAsync_ThreeAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3) => acc1 + acc2 + acc3);
    }

    [Fact]
    public async Task AggregateAsync_ThreeAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, _) => ValueTask.FromResult(acc1 + acc2 + acc3));
    }

    [Fact]
    public async Task AggregateAsync_FourAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            seed4: 0, accumulator4: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3, acc4) => acc1 + acc2 + acc3 + acc4);
    }

    [Fact]
    public async Task AggregateAsync_FourAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed4: 0, accumulator4: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, acc4, _) => ValueTask.FromResult(acc1 + acc2 + acc3 + acc4));
    }

    [Fact]
    public async Task AggregateAsync_FiveAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            seed4: 0, accumulator4: (acc, x) => acc + x,
            seed5: 0, accumulator5: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3, acc4, acc5) => acc1 + acc2 + acc3 + acc4 + acc5);
    }

    [Fact]
    public async Task AggregateAsync_FiveAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed4: 0, accumulator4: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed5: 0, accumulator5: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, acc4, acc5, _) => ValueTask.FromResult(acc1 + acc2 + acc3 + acc4 + acc5));
    }

    [Fact]
    public async Task AggregateAsync_SixAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            seed4: 0, accumulator4: (acc, x) => acc + x,
            seed5: 0, accumulator5: (acc, x) => acc + x,
            seed6: 0, accumulator6: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6) => acc1 + acc2 + acc3 + acc4 + acc5 + acc6);
    }

    [Fact]
    public async Task AggregateAsync_SixAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed4: 0, accumulator4: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed5: 0, accumulator5: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed6: 0, accumulator6: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6, _) => ValueTask.FromResult(acc1 + acc2 + acc3 + acc4 + acc5 + acc6));
    }

    [Fact]
    public async Task AggregateAsync_SevenAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            seed4: 0, accumulator4: (acc, x) => acc + x,
            seed5: 0, accumulator5: (acc, x) => acc + x,
            seed6: 0, accumulator6: (acc, x) => acc + x,
            seed7: 0, accumulator7: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6, acc7) => acc1 + acc2 + acc3 + acc4 + acc5 + acc6 + acc7);
    }

    [Fact]
    public async Task AggregateAsync_SevenAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed4: 0, accumulator4: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed5: 0, accumulator5: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed6: 0, accumulator6: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed7: 0, accumulator7: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6, acc7, _) => ValueTask.FromResult(acc1 + acc2 + acc3 + acc4 + acc5 + acc6 + acc7));
    }

    [Fact]
    public async Task AggregateAsync_EightAccumulators_Sync_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x) => acc + x,
            seed2: 0, accumulator2: (acc, x) => acc + x,
            seed3: 0, accumulator3: (acc, x) => acc + x,
            seed4: 0, accumulator4: (acc, x) => acc + x,
            seed5: 0, accumulator5: (acc, x) => acc + x,
            seed6: 0, accumulator6: (acc, x) => acc + x,
            seed7: 0, accumulator7: (acc, x) => acc + x,
            seed8: 0, accumulator8: (acc, x) => acc + x,
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6, acc7, acc8) => acc1 + acc2 + acc3 + acc4 + acc5 + acc6 + acc7 + acc8);
    }

    [Fact]
    public async Task AggregateAsync_EightAccumulators_Async_Compiles()
    {
        await _source.AggregateAsync(
            seed1: 0, accumulator1: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed2: 0, accumulator2: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed3: 0, accumulator3: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed4: 0, accumulator4: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed5: 0, accumulator5: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed6: 0, accumulator6: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed7: 0, accumulator7: (acc, x, _) => ValueTask.FromResult(acc + x),
            seed8: 0, accumulator8: (acc, x, _) => ValueTask.FromResult(acc + x),
            resultSelector: (acc1, acc2, acc3, acc4, acc5, acc6, acc7, acc8, _) => ValueTask.FromResult(acc1 + acc2 + acc3 + acc4 + acc5 + acc6 + acc7 + acc8));
    }
}

