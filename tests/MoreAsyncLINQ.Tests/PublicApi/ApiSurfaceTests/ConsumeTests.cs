namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ConsumeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task ConsumeAsync_Compiles()
    {
        await _source.ConsumeAsync();
        await _source.ConsumeAsync(CancellationToken.None);
    }
}