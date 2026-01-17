namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class RepeatTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 5);

    [Fact]
    public async Task Repeat_Compiles()
    {
        await _source.Repeat(3).ConsumeAsync();
    }

    [Fact]
    public async Task Repeat_Forever_Compiles()
    {
        await _source.Repeat().Take(15).ConsumeAsync();
    }
}

