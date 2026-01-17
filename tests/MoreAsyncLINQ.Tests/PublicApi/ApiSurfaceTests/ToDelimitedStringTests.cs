namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ToDelimitedStringTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<string> _stringSource = AsyncEnumerable.Range(0, 5).Select(i => i.ToString());

    [Fact]
    public async Task ToDelimitedStringAsync_WithDelimiter_Compiles()
    {
        await _source.ToDelimitedStringAsync(", ");
        await _source.ToDelimitedStringAsync(", ", CancellationToken.None);
    }

    [Fact]
    public async Task ToDelimitedStringAsync_Generic_Compiles()
    {
        await _stringSource.ToDelimitedStringAsync(", ");
        await _stringSource.ToDelimitedStringAsync(", ", CancellationToken.None);
    }
}
