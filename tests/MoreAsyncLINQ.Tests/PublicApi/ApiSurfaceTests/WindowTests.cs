namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class WindowTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Window_Compiles()
    {
        await _source.Window(3).ConsumeAsync();
    }

    [Fact]
    public async Task WindowLeft_Compiles()
    {
        await _source.WindowLeft(3).ConsumeAsync();
    }

    [Fact]
    public async Task WindowRight_Compiles()
    {
        await _source.WindowRight(3).ConsumeAsync();
    }
}


