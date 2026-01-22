namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class FromTests
{
    [Fact]
    public async Task From_OneFunction_Compiles()
    {
        await MoreAsyncEnumerable.From(_ => ValueTask.FromResult(1)).ConsumeAsync();
    }

    [Fact]
    public async Task From_TwoFunctions_Compiles()
    {
        await MoreAsyncEnumerable.From(
            _ => ValueTask.FromResult(1),
            _ => ValueTask.FromResult(2)).ConsumeAsync();
    }

    [Fact]
    public async Task From_ThreeFunctions_Compiles()
    {
        await MoreAsyncEnumerable.From(
            _ => ValueTask.FromResult(1),
            _ => ValueTask.FromResult(2),
            _ => ValueTask.FromResult(3)).ConsumeAsync();
    }

    [Fact]
    public async Task From_ArrayFunctions_Compiles()
    {
        Func<CancellationToken, ValueTask<int>>[] functions =
        [
            _ => ValueTask.FromResult(1),
            _ => ValueTask.FromResult(2),
            _ => ValueTask.FromResult(3),
            _ => ValueTask.FromResult(4)
        ];
        await MoreAsyncEnumerable.From(functions).ConsumeAsync();
    }
}


