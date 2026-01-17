namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ExtremaTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    // Maxima
    [Fact]
    public async Task Maxima_Sync_Compiles()
    {
        await _source.Maxima(x => x).ConsumeAsync();
    }

    [Fact]
    public async Task Maxima_SyncWithComparer_Compiles()
    {
        await _source.Maxima(x => x, Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task Maxima_Async_Compiles()
    {
        await _source.Maxima((x, _) => ValueTask.FromResult(x)).ConsumeAsync();
    }

    [Fact]
    public async Task Maxima_AsyncWithComparer_Compiles()
    {
        await _source.Maxima((x, _) => ValueTask.FromResult(x), Comparer<int>.Default).ConsumeAsync();
    }

    // Minima
    [Fact]
    public async Task Minima_Sync_Compiles()
    {
        await _source.Minima(x => x).ConsumeAsync();
    }

    [Fact]
    public async Task Minima_SyncWithComparer_Compiles()
    {
        await _source.Minima(x => x, Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task Minima_Async_Compiles()
    {
        await _source.Minima((x, _) => ValueTask.FromResult(x)).ConsumeAsync();
    }

    [Fact]
    public async Task Minima_AsyncWithComparer_Compiles()
    {
        await _source.Minima((x, _) => ValueTask.FromResult(x), Comparer<int>.Default).ConsumeAsync();
    }

    // IExtremaAsyncEnumerable methods
    [Fact]
    public async Task IExtremaAsyncEnumerable_Take_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.Take(5).ConsumeAsync();
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_TakeLast_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.TakeLast(5).ConsumeAsync();
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_FirstAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.FirstAsync();
        await extrema.FirstAsync(CancellationToken.None);
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_FirstOrDefaultAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.FirstOrDefaultAsync();
        await extrema.FirstOrDefaultAsync(CancellationToken.None);
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_LastAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.LastAsync();
        await extrema.LastAsync(CancellationToken.None);
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_LastOrDefaultAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.LastOrDefaultAsync();
        await extrema.LastOrDefaultAsync(CancellationToken.None);
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_SingleAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.SingleAsync();
        await extrema.SingleAsync(CancellationToken.None);
    }

    [Fact]
    public async Task IExtremaAsyncEnumerable_SingleOrDefaultAsync_Compiles()
    {
        var extrema = _source.Maxima(x => x);
        await extrema.SingleOrDefaultAsync();
        await extrema.SingleOrDefaultAsync(CancellationToken.None);
    }
}

