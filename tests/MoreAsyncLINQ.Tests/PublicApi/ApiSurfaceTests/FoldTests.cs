namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class FoldTests
{
    private static IAsyncEnumerable<int> Source(int count) => AsyncEnumerable.Range(0, count);

    [Fact]
    public async Task FoldAsync_One_Sync_Compiles()
    {
        await Source(1).FoldAsync(x1 => x1.ToString());
    }

    [Fact]
    public async Task FoldAsync_One_Async_Compiles()
    {
        await Source(1).FoldAsync((int x1, CancellationToken _) => ValueTask.FromResult(x1));
    }

    [Fact]
    public async Task FoldAsync_Two_Sync_Compiles()
    {
        await Source(2).FoldAsync((x1, x2) => (x1 + x2).ToString());
    }

    [Fact]
    public async Task FoldAsync_Two_Async_Compiles()
    {
        await Source(2).FoldAsync((int x1, int x2, CancellationToken _) => ValueTask.FromResult(x1 + x2));
    }

    [Fact]
    public async Task FoldAsync_Three_Sync_Compiles()
    {
        await Source(3).FoldAsync((x1, x2, x3) => (x1 + x2 + x3).ToString());
    }

    [Fact]
    public async Task FoldAsync_Three_Async_Compiles()
    {
        await Source(3).FoldAsync((int x1, int x2, int x3, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3));
    }

    [Fact]
    public async Task FoldAsync_Four_Sync_Compiles()
    {
        await Source(4).FoldAsync((x1, x2, x3, x4) => (x1 + x2 + x3 + x4).ToString());
    }

    [Fact]
    public async Task FoldAsync_Four_Async_Compiles()
    {
        await Source(4).FoldAsync((int x1, int x2, int x3, int x4, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4));
    }

    [Fact]
    public async Task FoldAsync_Five_Sync_Compiles()
    {
        await Source(5).FoldAsync((x1, x2, x3, x4, x5) => (x1 + x2 + x3 + x4 + x5).ToString());
    }

    [Fact]
    public async Task FoldAsync_Five_Async_Compiles()
    {
        await Source(5).FoldAsync((int x1, int x2, int x3, int x4, int x5, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5));
    }

    [Fact]
    public async Task FoldAsync_Six_Sync_Compiles()
    {
        await Source(6).FoldAsync((x1, x2, x3, x4, x5, x6) => (x1 + x2 + x3 + x4 + x5 + x6).ToString());
    }

    [Fact]
    public async Task FoldAsync_Six_Async_Compiles()
    {
        await Source(6).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6));
    }

    [Fact]
    public async Task FoldAsync_Seven_Sync_Compiles()
    {
        await Source(7).FoldAsync((x1, x2, x3, x4, x5, x6, x7) => (x1 + x2 + x3 + x4 + x5 + x6 + x7).ToString());
    }

    [Fact]
    public async Task FoldAsync_Seven_Async_Compiles()
    {
        Func<int, int, int, int, int, int, int, CancellationToken, ValueTask<int>> folder = (x1, x2, x3, x4, x5, x6, x7, _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7);
        await Source(7).FoldAsync(folder);
    }

    [Fact]
    public async Task FoldAsync_Eight_Sync_Compiles()
    {
        await Source(8).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8).ToString());
    }

    [Fact]
    public async Task FoldAsync_Eight_Async_Compiles()
    {
        await Source(8).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8));
    }

    [Fact]
    public async Task FoldAsync_Nine_Sync_Compiles()
    {
        await Source(9).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9).ToString());
    }

    [Fact]
    public async Task FoldAsync_Nine_Async_Compiles()
    {
        await Source(9).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9));
    }

    [Fact]
    public async Task FoldAsync_Ten_Sync_Compiles()
    {
        await Source(10).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10).ToString());
    }

    [Fact]
    public async Task FoldAsync_Ten_Async_Compiles()
    {
        await Source(10).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10));
    }

    [Fact]
    public async Task FoldAsync_Eleven_Sync_Compiles()
    {
        await Source(11).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11).ToString());
    }

    [Fact]
    public async Task FoldAsync_Eleven_Async_Compiles()
    {
        await Source(11).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11));
    }

    [Fact]
    public async Task FoldAsync_Twelve_Sync_Compiles()
    {
        await Source(12).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12).ToString());
    }

    [Fact]
    public async Task FoldAsync_Twelve_Async_Compiles()
    {
        await Source(12).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, int x12, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12));
    }

    [Fact]
    public async Task FoldAsync_Thirteen_Sync_Compiles()
    {
        await Source(13).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13).ToString());
    }

    [Fact]
    public async Task FoldAsync_Thirteen_Async_Compiles()
    {
        await Source(13).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, int x12, int x13, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13));
    }

    [Fact]
    public async Task FoldAsync_Fourteen_Sync_Compiles()
    {
        await Source(14).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13 + x14).ToString());
    }

    [Fact]
    public async Task FoldAsync_Fourteen_Async_Compiles()
    {
        await Source(14).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, int x12, int x13, int x14, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13 + x14));
    }

    [Fact]
    public async Task FoldAsync_Fifteen_Sync_Compiles()
    {
        await Source(15).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13 + x14 + x15).ToString());
    }

    [Fact]
    public async Task FoldAsync_Fifteen_Async_Compiles()
    {
        await Source(15).FoldAsync((int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, int x12, int x13, int x14, int x15, CancellationToken _) => ValueTask.FromResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13 + x14 + x15));
    }

    [Fact]
    public async Task FoldAsync_Sixteen_Sync_Compiles()
    {
        await Source(16).FoldAsync((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16) => (x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12 + x13 + x14 + x15 + x16).ToString());
    }
}

