namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class InterleaveTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);
    private static readonly IAsyncEnumerable<int> _other1 = AsyncEnumerable.Range(10, 5);
    private static readonly IAsyncEnumerable<int> _other2 = AsyncEnumerable.Range(20, 5);

    [Fact]
    public async Task Interleave_Single_Compiles()
    {
        await _source.Interleave(_other1).ConsumeAsync();
    }

    [Fact]
    public async Task Interleave_Params_Compiles()
    {
        await _source.Interleave(_other1, _other2).ConsumeAsync();
    }
}

