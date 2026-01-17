namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ToLookupTests
{
    private static readonly IAsyncEnumerable<KeyValuePair<int, string>> _kvpSource =
        AsyncEnumerable.Range(0, 10).Select(i => new KeyValuePair<int, string>(i % 3, i.ToString()));

    private static readonly IAsyncEnumerable<(int Key, string Value)> _tupleSource =
        AsyncEnumerable.Range(0, 10).Select(i => (i % 3, i.ToString()));

    [Fact]
    public async Task ToLookupAsync_KeyValuePair_Compiles()
    {
        await _kvpSource.ToLookupAsync();
        await _kvpSource.ToLookupAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ToLookupAsync_KeyValuePair_WithComparer_Compiles()
    {
        await _kvpSource.ToLookupAsync(EqualityComparer<int>.Default);
        await _kvpSource.ToLookupAsync(EqualityComparer<int>.Default, CancellationToken.None);
    }

    [Fact]
    public async Task ToLookupAsync_Tuple_Compiles()
    {
        await _tupleSource.ToLookupAsync();
        await _tupleSource.ToLookupAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ToLookupAsync_Tuple_WithComparer_Compiles()
    {
        await _tupleSource.ToLookupAsync(EqualityComparer<int>.Default);
        await _tupleSource.ToLookupAsync(EqualityComparer<int>.Default, CancellationToken.None);
    }
}
