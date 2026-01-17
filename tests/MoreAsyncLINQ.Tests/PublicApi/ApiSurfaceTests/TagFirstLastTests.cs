namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class TagFirstLastTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task TagFirstLast_SyncSelector_Compiles()
    {
        await _source.TagFirstLast((item, isFirst, isLast) => $"{item}:{isFirst}:{isLast}").ConsumeAsync();
    }

    [Fact]
    public async Task TagFirstLast_AsyncSelector_Compiles()
    {
        await _source.TagFirstLast((item, isFirst, isLast, _) => ValueTask.FromResult($"{item}:{isFirst}:{isLast}")).ConsumeAsync();
    }
}
