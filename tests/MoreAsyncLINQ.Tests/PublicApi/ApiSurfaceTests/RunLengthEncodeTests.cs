namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class RunLengthEncodeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task RunLengthEncode_Compiles()
    {
        await _source.RunLengthEncode().ConsumeAsync();
    }

    [Fact]
    public async Task RunLengthEncode_WithComparer_Compiles()
    {
        await _source.RunLengthEncode(EqualityComparer<int>.Default).ConsumeAsync();
    }
}


