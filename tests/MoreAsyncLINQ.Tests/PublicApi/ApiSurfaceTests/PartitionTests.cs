namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PartitionTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task PartitionAsync_Sync_Compiles()
    {
        await _source.PartitionAsync(x => x % 2 == 0);
    }

    [Fact]
    public async Task PartitionAsync_SyncWithResultSelector_Compiles()
    {
        await _source.PartitionAsync(
            x => x % 2 == 0,
            (trueItems, falseItems) => trueItems.Count() + falseItems.Count());
    }

    [Fact]
    public async Task PartitionAsync_Async_Compiles()
    {
        await _source.PartitionAsync((x, _) => ValueTask.FromResult(x % 2 == 0));
    }

    [Fact]
    public async Task PartitionAsync_AsyncWithResultSelector_Compiles()
    {
        await _source.PartitionAsync(
            (x, _) => ValueTask.FromResult(x % 2 == 0),
            (trueItems, falseItems, _) => ValueTask.FromResult(trueItems.Count() + falseItems.Count()));
    }
}

