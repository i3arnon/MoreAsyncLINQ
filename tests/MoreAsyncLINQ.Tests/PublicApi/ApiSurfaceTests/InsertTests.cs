namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class InsertTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<int> _toInsert = AsyncEnumerable.Range(100, 3);

    [Fact]
    public async Task Insert_Compiles()
    {
        await _source.Insert(_toInsert, 5).ConsumeAsync();
    }
}

