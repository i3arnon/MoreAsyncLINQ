namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class RightJoinTests
{
    private static readonly IAsyncEnumerable<int> _first = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<int> _second = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<string> _second2 = AsyncEnumerable.Range(0, 5).Select(i => i.ToString());

    // Homogeneous sequences
    [Fact]
    public async Task RightJoin_Homogeneous_Sync_Compiles()
    {
        await _first.RightJoin(
            _second,
            keySelector: x => x,
            secondSelector: x => x.ToString(),
            bothSelector: (x, y) => $"{x}:{y}").ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Homogeneous_SyncWithComparer_Compiles()
    {
        await _first.RightJoin(
            _second,
            keySelector: x => x,
            secondSelector: x => x.ToString(),
            bothSelector: (x, y) => $"{x}:{y}",
            comparer: EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Homogeneous_Async_Compiles()
    {
        await _first.RightJoin(
            _second,
            keySelector: (x, _) => ValueTask.FromResult(x),
            secondSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}")).ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Homogeneous_AsyncWithComparer_Compiles()
    {
        await _first.RightJoin(
            _second,
            keySelector: (x, _) => ValueTask.FromResult(x),
            secondSelector: (x, _) => ValueTask.FromResult(x.ToString()),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}"),
            comparer: EqualityComparer<int>.Default).ConsumeAsync();
    }

    // Heterogeneous sequences
    [Fact]
    public async Task RightJoin_Heterogeneous_Sync_Compiles()
    {
        await _first.RightJoin(
            _second2,
            firstKeySelector: x => x,
            secondKeySelector: x => x.Length,
            secondSelector: x => x,
            bothSelector: (x, y) => $"{x}:{y}").ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Heterogeneous_SyncWithComparer_Compiles()
    {
        await _first.RightJoin(
            _second2,
            firstKeySelector: x => x,
            secondKeySelector: x => x.Length,
            secondSelector: x => x,
            bothSelector: (x, y) => $"{x}:{y}",
            comparer: EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Heterogeneous_Async_Compiles()
    {
        await _first.RightJoin(
            _second2,
            firstKeySelector: (x, _) => ValueTask.FromResult(x),
            secondKeySelector: (x, _) => ValueTask.FromResult(x.Length),
            secondSelector: (x, _) => ValueTask.FromResult(x),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}")).ConsumeAsync();
    }

    [Fact]
    public async Task RightJoin_Heterogeneous_AsyncWithComparer_Compiles()
    {
        await _first.RightJoin(
            _second2,
            firstKeySelector: (x, _) => ValueTask.FromResult(x),
            secondKeySelector: (x, _) => ValueTask.FromResult(x.Length),
            secondSelector: (x, _) => ValueTask.FromResult(x),
            bothSelector: (x, y, _) => ValueTask.FromResult($"{x}:{y}"),
            comparer: EqualityComparer<int>.Default).ConsumeAsync();
    }
}

