namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class OrderedMergeTests
{
    private static readonly IAsyncEnumerable<int> _first = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<int> _second = AsyncEnumerable.Range(3, 5);
    private static readonly IAsyncEnumerable<string> _second2 = AsyncEnumerable.Range(0, 5).Select(i => i.ToString());

    // Homogeneous sequences
    [Fact]
    public async Task OrderedMerge_Homogeneous_Compiles()
    {
        await _first.OrderedMerge(_second).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_SyncKeySelector_Compiles()
    {
        await _first.OrderedMerge(_second, keySelector: x => x).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_SyncFullSelectors_Compiles()
    {
        await _first.OrderedMerge(
            _second,
            keySelector: x => x,
            firstSelector: x => x.ToString(),
            secondSelector: x => x.ToString(),
            bothSelector: (x, y) => $"{x}:{y}").ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_SyncWithComparer_Compiles()
    {
        await _first.OrderedMerge(
            _second,
            keySelector: x => x,
            firstSelector: x => x.ToString(),
            secondSelector: x => x.ToString(),
            bothSelector: (x, y) => $"{x}:{y}",
            comparer: Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_AsyncKeySelector_Compiles()
    {
        await _first.OrderedMerge(_second, keySelector: (x, _) => ValueTask.FromResult(x)).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_AsyncFullSelectors_Compiles()
    {
        await _first.OrderedMerge(
            _second,
            keySelector: (x, _) => ValueTask.FromResult(x),
            firstSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            secondSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}")).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Homogeneous_AsyncWithComparer_Compiles()
    {
        await _first.OrderedMerge(
            _second,
            keySelector: (x, _) => ValueTask.FromResult(x),
            firstSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            secondSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}"),
            comparer: Comparer<int>.Default).ConsumeAsync();
    }

    // Heterogeneous sequences
    [Fact]
    public async Task OrderedMerge_Heterogeneous_Sync_Compiles()
    {
        await _first.OrderedMerge(
            _second2,
            firstKeySelector: x => x,
            secondKeySelector: x => x.Length,
            firstSelector: x => x.ToString(),
            secondSelector: x => x,
            bothSelector: (x, y) => $"{x}:{y}").ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Heterogeneous_SyncWithComparer_Compiles()
    {
        await _first.OrderedMerge(
            _second2,
            firstKeySelector: x => x,
            secondKeySelector: x => x.Length,
            firstSelector: x => x.ToString(),
            secondSelector: x => x,
            bothSelector: (x, y) => $"{x}:{y}",
            comparer: Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Heterogeneous_Async_Compiles()
    {
        await _first.OrderedMerge(
            _second2,
            firstKeySelector: (x, _) => ValueTask.FromResult(x),
            secondKeySelector: (x, _) => ValueTask.FromResult(x.Length),
            firstSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            secondSelector: (x, _) => ValueTask.FromResult(x),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}")).ConsumeAsync();
    }

    [Fact]
    public async Task OrderedMerge_Heterogeneous_AsyncWithComparer_Compiles()
    {
        await _first.OrderedMerge(
            _second2,
            firstKeySelector: (x, _) => ValueTask.FromResult(x),
            secondKeySelector: (x, _) => ValueTask.FromResult(x.Length),
            firstSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            secondSelector: (x, _) => ValueTask.FromResult(x),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}"),
            comparer: Comparer<int>.Default).ConsumeAsync();
    }
}


