namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class EvaluateTests
{
    [Fact]
    public async Task Evaluate_Sync_Compiles()
    {
        var functions = AsyncEnumerable.Empty<Func<int>>();
        await functions.Evaluate().ConsumeAsync();
    }

    [Fact]
    public async Task Evaluate_AsyncNoToken_Compiles()
    {
        var functions = AsyncEnumerable.Empty<Func<ValueTask<int>>>();
        await functions.Evaluate().ConsumeAsync();
    }

    [Fact]
    public async Task Evaluate_AsyncWithToken_Compiles()
    {
        var functions = AsyncEnumerable.Empty<Func<CancellationToken, ValueTask<int>>>();
        await functions.Evaluate().ConsumeAsync();
    }
}

