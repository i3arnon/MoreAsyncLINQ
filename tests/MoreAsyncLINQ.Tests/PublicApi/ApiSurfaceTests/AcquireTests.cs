namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class AcquireTests
{
    private static readonly IAsyncEnumerable<IDisposable> _disposableSource = AsyncEnumerable.Empty<IDisposable>();

    [Fact]
    public async Task AcquireAsync_Compiles()
    {
        await _disposableSource.AcquireAsync();
        await _disposableSource.AcquireAsync(CancellationToken.None);
    }
}

