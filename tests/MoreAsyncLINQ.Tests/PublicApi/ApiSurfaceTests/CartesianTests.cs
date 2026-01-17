using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class CartesianTests
{
    private static readonly IAsyncEnumerable<int> _s1 = Range(0, 3);
    private static readonly IAsyncEnumerable<string> _s2 = Range(0, 3).Select(i => i.ToString());
    private static readonly IAsyncEnumerable<double> _s3 = Range(0, 3).Select(i => (double)i);
    private static readonly IAsyncEnumerable<bool> _s4 = Range(0, 3).Select(i => i % 2 == 0);
    private static readonly IAsyncEnumerable<long> _s5 = Range(0, 3).Select(i => (long)i);
    private static readonly IAsyncEnumerable<char> _s6 = Range(0, 3).Select(i => (char)i);
    private static readonly IAsyncEnumerable<byte> _s7 = Range(0, 3).Select(i => (byte)i);
    private static readonly IAsyncEnumerable<short> _s8 = Range(0, 3).Select(i => (short)i);

    [Fact]
    public async Task Cartesian_Two_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, (x1, x2) => $"{x1}{x2}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Two_Async_Compiles()
    {
        await _s1.Cartesian(_s2, (x1, x2, _) => ValueTask.FromResult($"{x1}{x2}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Three_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, (x1, x2, x3) => $"{x1}{x2}{x3}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Three_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, (x1, x2, x3, _) => ValueTask.FromResult($"{x1}{x2}{x3}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Four_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, (x1, x2, x3, x4) => $"{x1}{x2}{x3}{x4}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Four_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, (x1, x2, x3, x4, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Five_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, (x1, x2, x3, x4, x5) => $"{x1}{x2}{x3}{x4}{x5}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Five_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, (x1, x2, x3, x4, x5, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}{x5}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Six_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, (x1, x2, x3, x4, x5, x6) => $"{x1}{x2}{x3}{x4}{x5}{x6}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Six_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, (x1, x2, x3, x4, x5, x6, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}{x5}{x6}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Seven_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, _s7, (x1, x2, x3, x4, x5, x6, x7) => $"{x1}{x2}{x3}{x4}{x5}{x6}{x7}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Seven_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, _s7, (x1, x2, x3, x4, x5, x6, x7, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}{x5}{x6}{x7}")).ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Eight_Sync_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, _s7, _s8, (x1, x2, x3, x4, x5, x6, x7, x8) => $"{x1}{x2}{x3}{x4}{x5}{x6}{x7}{x8}").ConsumeAsync();
    }

    [Fact]
    public async Task Cartesian_Eight_Async_Compiles()
    {
        await _s1.Cartesian(_s2, _s3, _s4, _s5, _s6, _s7, _s8, (x1, x2, x3, x4, x5, x6, x7, x8, _) => ValueTask.FromResult($"{x1}{x2}{x3}{x4}{x5}{x6}{x7}{x8}")).ConsumeAsync();
    }
}

