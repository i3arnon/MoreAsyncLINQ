namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PadStartTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);

    [Fact]
    public async Task PadStart_Compiles()
    {
        await _source.PadStart(10).ConsumeAsync();
    }

    [Fact]
    public async Task PadStart_WithPaddingValue_Compiles()
    {
        await _source.PadStart(10, -1).ConsumeAsync();
    }

    [Fact]
    public async Task PadStart_SyncSelector_Compiles()
    {
        await _source.PadStart(10, i => i * 2).ConsumeAsync();
    }

    [Fact]
    public async Task PadStart_AsyncSelector_Compiles()
    {
        await _source.PadStart(10, (i, _) => ValueTask.FromResult(i * 2)).ConsumeAsync();
    }
}

