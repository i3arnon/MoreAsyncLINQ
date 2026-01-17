namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ToArrayByIndexTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 10).Select(i => i.ToString());

    [Fact]
    public async Task ToArrayByIndexAsync_Sync_Compiles()
    {
        await _source.ToArrayByIndexAsync(x => x.Length);
        await _source.ToArrayByIndexAsync(x => x.Length, CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_SyncWithResultSelector_Compiles()
    {
        await _source.ToArrayByIndexAsync(x => x.Length, x => x.ToUpper());
        await _source.ToArrayByIndexAsync(x => x.Length, x => x.ToUpper(), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_SyncWithResultSelectorAndIndex_Compiles()
    {
        await _source.ToArrayByIndexAsync(x => x.Length, (x, index) => $"{index}:{x}");
        await _source.ToArrayByIndexAsync(x => x.Length, (x, index) => $"{index}:{x}", CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_SyncWithLength_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, x => x.Length);
        await _source.ToArrayByIndexAsync(20, x => x.Length, CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_SyncWithLengthAndResultSelector_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, x => x.Length, x => x.ToUpper());
        await _source.ToArrayByIndexAsync(20, x => x.Length, x => x.ToUpper(), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_SyncWithLengthAndResultSelectorAndIndex_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, x => x.Length, (x, index) => $"{index}:{x}");
        await _source.ToArrayByIndexAsync(20, x => x.Length, (x, index) => $"{index}:{x}", CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_Async_Compiles()
    {
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length));
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_AsyncWithResultSelector_Compiles()
    {
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length), (x, _) => ValueTask.FromResult(x.ToUpper()));
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length), (x, _) => ValueTask.FromResult(x.ToUpper()), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_AsyncWithResultSelectorAndIndex_Compiles()
    {
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length), (x, index, _) => ValueTask.FromResult($"{index}:{x}"));
        await _source.ToArrayByIndexAsync((x, _) => ValueTask.FromResult(x.Length), (x, index, _) => ValueTask.FromResult($"{index}:{x}"), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_AsyncWithLength_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length));
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_AsyncWithLengthAndResultSelector_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length), (x, _) => ValueTask.FromResult(x.ToUpper()));
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length), (x, _) => ValueTask.FromResult(x.ToUpper()), CancellationToken.None);
    }

    [Fact]
    public async Task ToArrayByIndexAsync_AsyncWithLengthAndResultSelectorAndIndex_Compiles()
    {
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length), (x, index, _) => ValueTask.FromResult($"{index}:{x}"));
        await _source.ToArrayByIndexAsync(20, (x, _) => ValueTask.FromResult(x.Length), (x, index, _) => ValueTask.FromResult($"{index}:{x}"), CancellationToken.None);
    }
}
