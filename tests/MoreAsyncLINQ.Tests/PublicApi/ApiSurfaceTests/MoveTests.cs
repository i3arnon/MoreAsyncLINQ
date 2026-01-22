namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class MoveTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Move_Compiles()
    {
        await _source.Move(2, 3, 5).ConsumeAsync();
    }
}


