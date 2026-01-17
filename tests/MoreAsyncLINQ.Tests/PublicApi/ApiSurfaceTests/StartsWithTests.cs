namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class StartsWithTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<int> _prefix = AsyncEnumerable.Range(0, 3);

    [Fact]
    public async Task StartsWithAsync_Compiles()
    {
        await _source.StartsWithAsync(_prefix);
        await _source.StartsWithAsync(_prefix, CancellationToken.None);
    }

    [Fact]
    public async Task StartsWithAsync_WithComparer_Compiles()
    {
        await _source.StartsWithAsync(_prefix, EqualityComparer<int>.Default);
        await _source.StartsWithAsync(_prefix, EqualityComparer<int>.Default, CancellationToken.None);
    }
}

