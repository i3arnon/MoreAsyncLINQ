namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class CountingTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<string> _source2 = AsyncEnumerable.Range(0, 10).Select(i => i.ToString());

    [Fact]
    public async Task AtLeastAsync_Compiles()
    {
        await _source.AtLeastAsync(5);
        await _source.AtLeastAsync(5, CancellationToken.None);
    }

    [Fact]
    public async Task AtMostAsync_Compiles()
    {
        await _source.AtMostAsync(5);
        await _source.AtMostAsync(5, CancellationToken.None);
    }

    [Fact]
    public async Task CompareCountAsync_Compiles()
    {
        await _source.CompareCountAsync(_source2);
        await _source.CompareCountAsync(_source2, CancellationToken.None);
    }

    [Fact]
    public async Task CountBetweenAsync_Compiles()
    {
        await _source.CountBetweenAsync(1, 10);
        await _source.CountBetweenAsync(1, 10, CancellationToken.None);
    }

    [Fact]
    public async Task ExactlyAsync_Compiles()
    {
        await _source.ExactlyAsync(5);
        await _source.ExactlyAsync(5, CancellationToken.None);
    }
}