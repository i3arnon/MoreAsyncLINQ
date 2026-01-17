namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ZipTests
{
    private static readonly IAsyncEnumerable<int> _first = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<string> _second = AsyncEnumerable.Range(0, 8).Select(i => i.ToString());
    private static readonly IAsyncEnumerable<double> _third = AsyncEnumerable.Range(0, 6).Select(i => (double)i);
    private static readonly IAsyncEnumerable<bool> _fourth = AsyncEnumerable.Range(0, 4).Select(i => i % 2 == 0);

    [Fact]
    public async Task ZipLongest_Two_SyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, (a, b) => $"{a}:{b}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipLongest_Two_AsyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, (a, b, _) => ValueTask.FromResult($"{a}:{b}")).ConsumeAsync();
    }

    [Fact]
    public async Task ZipLongest_Three_SyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, _third, (a, b, c) => $"{a}:{b}:{c}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipLongest_Three_AsyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, _third, (a, b, c, _) => ValueTask.FromResult($"{a}:{b}:{c}")).ConsumeAsync();
    }

    [Fact]
    public async Task ZipLongest_Four_SyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, _third, _fourth, (a, b, c, d) => $"{a}:{b}:{c}:{d}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipLongest_Four_AsyncSelector_Compiles()
    {
        await _first.ZipLongest(_second, _third, _fourth, (a, b, c, d, _) => ValueTask.FromResult($"{a}:{b}:{c}:{d}")).ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Two_SyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, (a, b) => $"{a}:{b}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Two_AsyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, (a, b, _) => ValueTask.FromResult($"{a}:{b}")).ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Three_SyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, _third, (a, b, c) => $"{a}:{b}:{c}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Three_AsyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, _third, (a, b, c, _) => ValueTask.FromResult($"{a}:{b}:{c}")).ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Four_SyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, _third, _fourth, (a, b, c, d) => $"{a}:{b}:{c}:{d}").ConsumeAsync();
    }

    [Fact]
    public async Task ZipShortest_Four_AsyncSelector_Compiles()
    {
        await _first.ZipShortest(_second, _third, _fourth, (a, b, c, d, _) => ValueTask.FromResult($"{a}:{b}:{c}:{d}")).ConsumeAsync();
    }
}
