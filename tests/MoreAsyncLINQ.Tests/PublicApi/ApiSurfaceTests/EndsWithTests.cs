namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class EndsWithTests
{
    private static readonly IAsyncEnumerable<int> _first = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<int> _second = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task EndsWithAsync_Compiles()
    {
        await _first.EndsWithAsync(_second);
        await _first.EndsWithAsync(_second, CancellationToken.None);
    }

    [Fact]
    public async Task EndsWithAsync_WithComparer_Compiles()
    {
        await _first.EndsWithAsync(_second, EqualityComparer<int>.Default);
        await _first.EndsWithAsync(_second, EqualityComparer<int>.Default, CancellationToken.None);
    }
}

