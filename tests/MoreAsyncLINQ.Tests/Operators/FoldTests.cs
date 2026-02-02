using MoreLinq;

namespace MoreAsyncLINQ.Tests;

// ReSharper disable StringLiteralTypo

public class FoldTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, _ => 0));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (_, _) => ValueTask.FromResult(0)));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b) => a + b));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, _) => ValueTask.FromResult(a + b)));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, c, d) => a + b + c + d));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int, int, int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, c, d, e, f, g, h, _) => ValueTask.FromResult(a + b + c + d + e + f + g + h)));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int, int, int, int, int, CancellationToken, ValueTask<int>>)null!));

        // Arity 16 - sync only (async not available due to Func delegate limit)
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync<int, int>(null!, (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)null!));

        // Arity 15 - async (highest async arity)
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, CancellationToken, ValueTask<int>> asyncFolder15 = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, _) => ValueTask.FromResult(a + b + c + d + e + f + g + h + i + j + k + l + m + n + o);
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.FoldAsync(null!, asyncFolder15));
        Assert.Throws<ArgumentNullException>("folder", () => AsyncEnumerable.Empty<int>().FoldAsync((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, CancellationToken, ValueTask<int>>)null!));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task WithTooFewItems_Throws(bool isAsync)
    {
        var source = AsyncEnumerable.Range(1, 3);

        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                if (isAsync)
                {
                    await source.FoldAsync(async (int a, int b, int c, int d, CancellationToken _) => a + b + c + d);
                }
                else
                {
                    await source.FoldAsync((a, b, c, d) => a + b + c + d);
                }
            });

        Assert.Equal("Sequence contains too few elements when exactly 4 were expected.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task WithEmptySequence_Throws(bool isAsync)
    {
        var source = AsyncEnumerable.Empty<int>();

        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                if (isAsync)
                {
                    await source.FoldAsync(async (int x, CancellationToken _) => x);
                }
                else
                {
                    await source.FoldAsync(x => x);
                }
            });

        Assert.Equal("Sequence contains too few elements when exactly 1 was expected.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task WithTooManyItems_Throws(bool isAsync)
    {
        var source = AsyncEnumerable.Range(1, 3);

        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                if (isAsync)
                {
                    await source.FoldAsync(async (int a, int b, CancellationToken _) => a + b);
                }
                else
                {
                    await source.FoldAsync((a, b) => a + b);
                }
            });

        Assert.Equal("Sequence contains too many elements when exactly 2 were expected.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold1(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(1).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold(a => string.Join(string.Empty, a));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, CancellationToken _) => string.Join(string.Empty, a))
                : await asyncSource.FoldAsync(a => string.Join(string.Empty, a));

        Assert.Equal(expected, actual);
        Assert.Equal("a", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold2(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(2).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b) => string.Join(string.Empty, a, b));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, CancellationToken _) => string.Join(string.Empty, a, b))
                : await asyncSource.FoldAsync((a, b) => string.Join(string.Empty, a, b));

        Assert.Equal(expected, actual);
        Assert.Equal("ab", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold3(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(3).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c) => string.Join(string.Empty, a, b, c));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, CancellationToken _) => string.Join(string.Empty, a, b, c))
                : await asyncSource.FoldAsync((a, b, c) => string.Join(string.Empty, a, b, c));

        Assert.Equal(expected, actual);
        Assert.Equal("abc", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold4(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(4).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d) => string.Join(string.Empty, a, b, c, d));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, CancellationToken _) => string.Join(string.Empty, a, b, c, d))
                : await asyncSource.FoldAsync((a, b, c, d) => string.Join(string.Empty, a, b, c, d));

        Assert.Equal(expected, actual);
        Assert.Equal("abcd", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold5(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(5).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e) => string.Join(string.Empty, a, b, c, d, e));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e))
                : await asyncSource.FoldAsync((a, b, c, d, e) => string.Join(string.Empty, a, b, c, d, e));

        Assert.Equal(expected, actual);
        Assert.Equal("abcde", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold6(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(6).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f) => string.Join(string.Empty, a, b, c, d, e, f));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f))
                : await asyncSource.FoldAsync((a, b, c, d, e, f) => string.Join(string.Empty, a, b, c, d, e, f));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdef", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold7(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(7).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g) => string.Join(string.Empty, a, b, c, d, e, f, g));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g) => string.Join(string.Empty, a, b, c, d, e, f, g));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefg", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold8(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(8).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h) => string.Join(string.Empty, a, b, c, d, e, f, g, h));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h) => string.Join(string.Empty, a, b, c, d, e, f, g, h));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefgh", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold9(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(9).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghi", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold10(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghij", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold11(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(11).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, char k, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijk", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold12(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(12).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k, l) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, char k, char l, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k, l) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijkl", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold13(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(13).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, char k, char l, char m, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k, l, m) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijklm", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold14(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(14).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, char k, char l, char m, char n, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijklmn", actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Fold15(bool isAsync)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(15).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o));
        var actual =
            isAsync
                ? await asyncSource.FoldAsync(async (char a, char b, char c, char d, char e, char f, char g, char h, char i, char j, char k, char l, char m, char n, char o, CancellationToken _) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o))
                : await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijklmno", actual);
    }

    [Fact]
    public async Task Fold16_Sync()
    {
        // Async overload not available for arity 16 due to Func delegate limit (16 input params + CancellationToken would exceed)
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var source = alphabet.Take(16).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p));
        var actual = await asyncSource.FoldAsync((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p));

        Assert.Equal(expected, actual);
        Assert.Equal("abcdefghijklmnop", actual);
    }
}
