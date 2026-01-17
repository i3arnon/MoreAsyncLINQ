namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class TransposeTests
{
    [Fact]
    public async Task Transpose_Compiles()
    {
        var source = AsyncEnumerable.Range(0, 3).Select(row => AsyncEnumerable.Range(row * 3, 3));
        await source.Transpose().ConsumeAsync();
    }
}
