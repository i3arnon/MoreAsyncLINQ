namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class BatchTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Batch_Compiles()
    {
        await _source.Batch(10).ConsumeAsync();
    }

    [Fact]
    public async Task Batch_WithResultSelector_Sync_Compiles()
    {
        await _source.Batch(10, batch => batch.Sum()).ConsumeAsync();
    }

    [Fact]
    public async Task Batch_WithResultSelector_Async_Compiles()
    {
        await _source.Batch(10, (batch, _) => ValueTask.FromResult(batch.Sum())).ConsumeAsync();
    }
}