namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class LeadTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Lead_Sync_Compiles()
    {
        await _source.Lead(2, (current, lead) => $"{current}:{lead}").ConsumeAsync();
    }

    [Fact]
    public async Task Lead_SyncWithDefault_Compiles()
    {
        await _source.Lead(2, -1, (current, lead) => $"{current}:{lead}").ConsumeAsync();
    }

    [Fact]
    public async Task Lead_Async_Compiles()
    {
        await _source.Lead(2, (current, lead, _) => ValueTask.FromResult($"{current}:{lead}")).ConsumeAsync();
    }

    [Fact]
    public async Task Lead_AsyncWithDefault_Compiles()
    {
        await _source.Lead(2, -1, (current, lead, _) => ValueTask.FromResult($"{current}:{lead}")).ConsumeAsync();
    }
}

