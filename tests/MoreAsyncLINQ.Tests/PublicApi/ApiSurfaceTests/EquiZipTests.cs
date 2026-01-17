using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class EquiZipTests
{
    private static readonly IAsyncEnumerable<int> _s1 = Range(0, 10);
    private static readonly IAsyncEnumerable<string> _s2 = Range(0, 10).Select(i => i.ToString());
    private static readonly IAsyncEnumerable<double> _s3 = Range(0, 10).Select(i => (double)i);
    private static readonly IAsyncEnumerable<bool> _s4 = Range(0, 10).Select(i => i % 2 == 0);

    [Fact]
    public async Task EquiZip_Two_Sync_Compiles()
    {
        await _s1.EquiZip(_s2, (x1, x2) => $"{x1}{x2}").ConsumeAsync();
    }

    [Fact]
    public async Task EquiZip_Two_Async_Compiles()
    {
        await _s1.EquiZip(_s2, (x1, x2, _) => ValueTask.FromResult($"{x1}{x2}")).ConsumeAsync();
    }

    [Fact]
    public async Task EquiZip_Three_Sync_Compiles()
    {
        await _s1.EquiZip(_s2, _s3, (x1, x2, x3) => $"{x1}{x2}{x3}").ConsumeAsync();
    }

    [Fact]
    public async Task EquiZip_Three_Async_Compiles()
    {
        await _s1.EquiZip(_s2, _s3, (x1, x2, x3, _) => ValueTask.FromResult($"{x1}{x2}{x3}")).ConsumeAsync();
    }

    [Fact]
    public async Task EquiZip_Four_Sync_Compiles()
    {
        await _s1.EquiZip(_s2, _s3, _s4, (x1, x2, x3, x4) => $"{x1}{x2}{x3}{x4}").ConsumeAsync();
    }

    [Fact]
    public async Task EquiZip_Four_Async_Compiles()
    {
        await _s1.EquiZip(_s2, _s3, _s4, (x1, x2, x3, x4, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}")).ConsumeAsync();
    }
}

