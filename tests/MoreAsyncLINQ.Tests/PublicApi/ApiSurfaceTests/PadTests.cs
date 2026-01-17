namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PadTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);

    [Fact]
    public async Task Pad_Compiles()
    {
        await _source.Pad(10).ConsumeAsync();
    }

    [Fact]
    public async Task Pad_WithPaddingValue_Compiles()
    {
        await _source.Pad(10, -1).ConsumeAsync();
    }

    [Fact]
    public async Task Pad_SyncSelector_Compiles()
    {
        await _source.Pad(10, i => i * 2).ConsumeAsync();
    }

    [Fact]
    public async Task Pad_AsyncSelector_Compiles()
    {
        await _source.Pad(10, (i, _) => ValueTask.FromResult(i * 2)).ConsumeAsync();
    }
}

