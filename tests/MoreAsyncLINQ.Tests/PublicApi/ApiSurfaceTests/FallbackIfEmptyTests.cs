namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class FallbackIfEmptyTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Empty<int>();
    private static readonly IAsyncEnumerable<int> _fallbackSource = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task FallbackIfEmpty_OneFallback_Compiles()
    {
        await _source.FallbackIfEmpty(1).ConsumeAsync();
    }

    [Fact]
    public async Task FallbackIfEmpty_TwoFallbacks_Compiles()
    {
        await _source.FallbackIfEmpty(1, 2).ConsumeAsync();
    }

    [Fact]
    public async Task FallbackIfEmpty_ThreeFallbacks_Compiles()
    {
        await _source.FallbackIfEmpty(1, 2, 3).ConsumeAsync();
    }

    [Fact]
    public async Task FallbackIfEmpty_FourFallbacks_Compiles()
    {
        await _source.FallbackIfEmpty(1, 2, 3, 4).ConsumeAsync();
    }

    [Fact]
    public async Task FallbackIfEmpty_ArrayFallback_Compiles()
    {
        await _source.FallbackIfEmpty(1, 2, 3, 4, 5).ConsumeAsync();
    }

    [Fact]
    public async Task FallbackIfEmpty_SequenceFallback_Compiles()
    {
        await _source.FallbackIfEmpty(_fallbackSource).ConsumeAsync();
    }
}

