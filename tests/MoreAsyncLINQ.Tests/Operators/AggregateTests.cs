using MoreLinq;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ.Tests;

public class AggregateTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_TwoAccumulators_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            "source",
            () =>
                MoreAsyncEnumerable.AggregateAsync<int, int, int, int>(
                    null!,
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => acc + x,
                    (a1, a2) => a1 + a2));

        Assert.Throws<ArgumentNullException>(
            "accumulator1",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        null!,
                        0,
                        (acc, x) => acc + x,
                        (a1, a2) => a1 + a2));

        Assert.Throws<ArgumentNullException>(
            "accumulator2",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x) => acc + x,
                        0,
                        null!,
                        (a1, a2) => a1 + a2));

        Assert.Throws<ArgumentNullException>(
            "resultSelector",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x) => acc + x,
                        0,
                        (acc, x) => acc + x,
                        (Func<int, int, int>)null!));

        Assert.Throws<ArgumentNullException>(
            "source",
            () =>
                MoreAsyncEnumerable.AggregateAsync<int, int, int, int>(
                    null!,
                    0,
                    (acc, x, _) => ValueTask.FromResult(acc + x),
                    0,
                    (acc, x, _) => ValueTask.FromResult(acc + x),
                    (a1, a2, _) => ValueTask.FromResult(a1 + a2)));

        Assert.Throws<ArgumentNullException>(
            "accumulator1",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        null!,
                        0,
                        (acc, x, _) => ValueTask.FromResult(acc + x),
                        (a1, a2, _) => ValueTask.FromResult(a1 + a2)));

        Assert.Throws<ArgumentNullException>(
            "accumulator2",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x, _) => ValueTask.FromResult(acc + x),
                        0,
                        null!,
                        (a1, a2, _) => ValueTask.FromResult(a1 + a2)));

        Assert.Throws<ArgumentNullException>(
            "resultSelector",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x, _) => ValueTask.FromResult(acc + x),
                        0,
                        (acc, x, _) => ValueTask.FromResult(acc + x),
                        (Func<int, int, CancellationToken, ValueTask<int>>)null!));
    }

    [Fact]
    public void InvalidInputs_ThreeAccumulators_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            "source",
            () =>
                MoreAsyncEnumerable.AggregateAsync<int, int, int, int, int>(
                    null!,
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => acc + x,
                    (a1, a2, a3) => a1 + a2 + a3));

        Assert.Throws<ArgumentNullException>(
            "accumulator1",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        null!,
                        0,
                        (acc, x) => acc + x,
                        0,
                        (acc, x) => acc + x,
                        (a1, a2, a3) => a1 + a2 + a3));

        Assert.Throws<ArgumentNullException>(
            "accumulator2",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x) => acc + x,
                        0,
                        null!,
                        0,
                        (acc, x) => acc + x,
                        (a1, a2, a3) => a1 + a2 + a3));

        Assert.Throws<ArgumentNullException>(
            "accumulator3",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x) => acc + x,
                        0,
                        (acc, x) => acc + x,
                        0,
                        null!,
                        (a1, a2, a3) => a1 + a2 + a3));

        Assert.Throws<ArgumentNullException>(
            "resultSelector",
            () =>
                Empty<int>().
                    AggregateAsync(
                        0,
                        (acc, x) => acc + x,
                        0,
                        (acc, x) => acc + x,
                        0,
                        (acc, x) => acc + x,
                        (Func<int, int, int, int>)null!));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EmptySequence_TwoAccumulators(bool isAsync)
    {
        var source = Empty<int>();

        var result =
            isAsync
                ? await source.AggregateAsync(
                    10,
                    async (acc, x, _) => acc + x,
                    20,
                    async (acc, x, _) => acc * x,
                    async (a1, a2, _) => (a1, a2))
                : await source.AggregateAsync(
                    10,
                    (acc, x) => acc + x,
                    20,
                    (acc, x) => acc * x,
                    (a1, a2) => (a1, a2));

        Assert.Equal((10, 20), result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EmptySequence_ThreeAccumulators(bool isAsync)
    {
        var source = Empty<int>();

        var result =
            isAsync
                ? await source.AggregateAsync(
                    10,
                    async (acc, x, _) => acc + x,
                    20,
                    async (acc, x, _) => acc * x,
                    30,
                    async (acc, x, _) => acc - x,
                    async (a1, a2, a3, _) => (a1, a2, a3))
                : await source.AggregateAsync(
                    10,
                    (acc, x) => acc + x,
                    20,
                    (acc, x) => acc * x,
                    30,
                    (acc, x) => acc - x,
                    (a1, a2, a3) => (a1, a2, a3));

        Assert.Equal((10, 20, 30), result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                0,
                (acc, x) => acc + x,
                (sum1, sum2) => new[] { sum1, sum2 });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    0,
                    async (acc, x, _) => acc + x,
                    async (sum1, sum2, _) => new[] { sum1, sum2 })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => acc + x,
                    (sum1, sum2) => new[] { sum1, sum2 });

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThreeAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                1,
                (acc, x) => acc * x,
                0,
                (acc, _) => acc + 1,
                (sum, product, count) => new { Sum = sum, Product = product, Count = count });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    1,
                    async (acc, x, _) => acc * x,
                    0,
                    async (acc, _, _) => acc + 1,
                    async (sum, product, count, _) => new { Sum = sum, Product = product, Count = count })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    1,
                    (acc, x) => acc * x,
                    0,
                    (acc, _) => acc + 1,
                    (sum, product, count) => new { Sum = sum, Product = product, Count = count });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.Product, actual.Product);
        Assert.Equal(expected.Count, actual.Count);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task FourAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                0,
                (acc, x) => x % 2 == 0 ? acc + x : acc,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                (sum, evenSum, min, max) => new { Sum = sum, EvenSum = evenSum, Min = min, Max = max });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    0,
                    async (acc, x, _) => x % 2 == 0 ? acc + x : acc,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Max(n, x) : x,
                    async (sum, evenSum, min, max, _) => new { Sum = sum, EvenSum = evenSum, Min = min, Max = max })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => x % 2 == 0 ? acc + x : acc,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                    (sum, evenSum, min, max) => new { Sum = sum, EvenSum = evenSum, Min = min, Max = max });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.EvenSum, actual.EvenSum);
        Assert.Equal(expected.Min, actual.Min);
        Assert.Equal(expected.Max, actual.Max);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task FiveAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                0,
                (acc, x) => x % 2 == 0 ? acc + x : acc,
                0,
                (acc, _) => acc + 1,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                (sum, evenSum, count, min, max) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    0,
                    async (acc, x, _) => x % 2 == 0 ? acc + x : acc,
                    0,
                    async (acc, _, _) => acc + 1,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Max(n, x) : x,
                    async (sum, evenSum, count, min, max, _) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => x % 2 == 0 ? acc + x : acc,
                    0,
                    (acc, _) => acc + 1,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                    (sum, evenSum, count, min, max) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.EvenSum, actual.EvenSum);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected.Min, actual.Min);
        Assert.Equal(expected.Max, actual.Max);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task SixAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                0,
                (acc, x) => x % 2 == 0 ? acc + x : acc,
                0,
                (acc, _) => acc + 1,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                new HashSet<int>(),
                (acc, x) =>
                {
                    acc.Add(x % 3);
                    return acc;
                },
                (sum, evenSum, count, min, max, modulos) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max, Modulos = modulos });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    0,
                    async (acc, x, _) => x % 2 == 0 ? acc + x : acc,
                    0,
                    async (acc, _, _) => acc + 1,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Max(n, x) : x,
                    new HashSet<int>(),
                    async (acc, x, _) =>
                    {
                        acc.Add(x % 3);
                        return acc;
                    },
                    async (sum, evenSum, count, min, max, modulos, _) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max, Modulos = modulos })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => x % 2 == 0 ? acc + x : acc,
                    0,
                    (acc, _) => acc + 1,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                    new HashSet<int>(),
                    (acc, x) =>
                    {
                        acc.Add(x % 3);
                        return acc;
                    },
                    (sum, evenSum, count, min, max, modulos) => new { Sum = sum, EvenSum = evenSum, Count = count, Min = min, Max = max, Modulos = modulos });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.EvenSum, actual.EvenSum);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected.Min, actual.Min);
        Assert.Equal(expected.Max, actual.Max);
        Assert.Equal(expected.Modulos.OrderBy(x => x), actual.Modulos.OrderBy(x => x));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task SevenAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).Select(n => new { Num = n, Str = n.ToString() }).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, e) => acc + e.Num,
                0,
                (acc, e) => e.Num % 2 == 0 ? acc + e.Num : acc,
                0,
                (acc, _) => acc + 1,
                (int?)null,
                (acc, e) => acc is { } n ? Math.Min(n, e.Num) : e.Num,
                (int?)null,
                (acc, e) => acc is { } n ? Math.Max(n, e.Num) : e.Num,
                new HashSet<int>(),
                (acc, e) =>
                {
                    acc.Add(e.Str.Length);
                    return acc;
                },
                new List<(int Num, string Str)>(),
                (acc, e) =>
                {
                    acc.Add((e.Num, e.Str));
                    return acc;
                },
                (sum, evenSum, count, min, max, lengths, items) => new
                {
                    Sum = sum,
                    EvenSum = evenSum,
                    Count = count,
                    Average = (double)sum / count,
                    Min = min ?? throw new InvalidOperationException(),
                    Max = max ?? throw new InvalidOperationException(),
                    UniqueLengths = lengths,
                    Items = items,
                });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, e, _) => acc + e.Num,
                    0,
                    async (acc, e, _) => e.Num % 2 == 0 ? acc + e.Num : acc,
                    0,
                    async (acc, _, _) => acc + 1,
                    (int?)null,
                    async (acc, e, _) => acc is { } n ? Math.Min(n, e.Num) : e.Num,
                    (int?)null,
                    async (acc, e, _) => acc is { } n ? Math.Max(n, e.Num) : e.Num,
                    new HashSet<int>(),
                    async (acc, e, _) =>
                    {
                        acc.Add(e.Str.Length);
                        return acc;
                    },
                    new List<(int Num, string Str)>(),
                    async (acc, e, _) =>
                    {
                        acc.Add((e.Num, e.Str));
                        return acc;
                    },
                    async (sum, evenSum, count, min, max, lengths, items, _) => new
                    {
                        Sum = sum,
                        EvenSum = evenSum,
                        Count = count,
                        Average = (double)sum / count,
                        Min = min ?? throw new InvalidOperationException(),
                        Max = max ?? throw new InvalidOperationException(),
                        UniqueLengths = lengths,
                        Items = items,
                    })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, e) => acc + e.Num,
                    0,
                    (acc, e) => e.Num % 2 == 0 ? acc + e.Num : acc,
                    0,
                    (acc, _) => acc + 1,
                    (int?)null,
                    (acc, e) => acc is { } n ? Math.Min(n, e.Num) : e.Num,
                    (int?)null,
                    (acc, e) => acc is { } n ? Math.Max(n, e.Num) : e.Num,
                    new HashSet<int>(),
                    (acc, e) =>
                    {
                        acc.Add(e.Str.Length);
                        return acc;
                    },
                    new List<(int Num, string Str)>(),
                    (acc, e) =>
                    {
                        acc.Add((e.Num, e.Str));
                        return acc;
                    },
                    (sum, evenSum, count, min, max, lengths, items) => new
                    {
                        Sum = sum,
                        EvenSum = evenSum,
                        Count = count,
                        Average = (double)sum / count,
                        Min = min ?? throw new InvalidOperationException(),
                        Max = max ?? throw new InvalidOperationException(),
                        UniqueLengths = lengths,
                        Items = items,
                    });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.EvenSum, actual.EvenSum);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected.Average, actual.Average);
        Assert.Equal(expected.Min, actual.Min);
        Assert.Equal(expected.Max, actual.Max);
        Assert.Equal(expected.UniqueLengths.OrderBy(x => x), actual.UniqueLengths.OrderBy(x => x));
        Assert.Equal(
            expected.Items.OrderBy(x => x.Num).ToArray(),
            actual.Items.OrderBy(x => x.Num).ToArray());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EightAccumulators(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToArray();
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, x) => acc + x,
                0,
                (acc, x) => x % 2 == 0 ? acc + x : acc,
                0,
                (acc, x) => x % 2 != 0 ? acc + x : acc,
                0,
                (acc, _) => acc + 1,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                (int?)null,
                (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                new HashSet<int>(),
                (acc, x) =>
                {
                    acc.Add(x % 3);
                    return acc;
                },
                new List<int>(),
                (acc, x) =>
                {
                    acc.Add(x);
                    return acc;
                },
                (sum, evenSum, oddSum, count, min, max, modulos, items) => new
                {
                    Sum = sum,
                    EvenSum = evenSum,
                    OddSum = oddSum,
                    Count = count,
                    Min = min,
                    Max = max,
                    Modulos = modulos,
                    Items = items,
                });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, x, _) => acc + x,
                    0,
                    async (acc, x, _) => x % 2 == 0 ? acc + x : acc,
                    0,
                    async (acc, x, _) => x % 2 != 0 ? acc + x : acc,
                    0,
                    async (acc, _, _) => acc + 1,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    async (acc, x, _) => acc is { } n ? Math.Max(n, x) : x,
                    new HashSet<int>(),
                    async (acc, x, _) =>
                    {
                        acc.Add(x % 3);
                        return acc;
                    },
                    new List<int>(),
                    async (acc, x, _) =>
                    {
                        acc.Add(x);
                        return acc;
                    },
                    async (sum, evenSum, oddSum, count, min, max, modulos, items, _) => new
                    {
                        Sum = sum,
                        EvenSum = evenSum,
                        OddSum = oddSum,
                        Count = count,
                        Min = min,
                        Max = max,
                        Modulos = modulos,
                        Items = items,
                    })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, x) => acc + x,
                    0,
                    (acc, x) => x % 2 == 0 ? acc + x : acc,
                    0,
                    (acc, x) => x % 2 != 0 ? acc + x : acc,
                    0,
                    (acc, _) => acc + 1,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Min(n, x) : x,
                    (int?)null,
                    (acc, x) => acc is { } n ? Math.Max(n, x) : x,
                    new HashSet<int>(),
                    (acc, x) =>
                    {
                        acc.Add(x % 3);
                        return acc;
                    },
                    new List<int>(),
                    (acc, x) =>
                    {
                        acc.Add(x);
                        return acc;
                    },
                    (sum, evenSum, oddSum, count, min, max, modulos, items) => new
                    {
                        Sum = sum,
                        EvenSum = evenSum,
                        OddSum = oddSum,
                        Count = count,
                        Min = min,
                        Max = max,
                        Modulos = modulos,
                        Items = items,
                    });

        Assert.Equal(expected.Sum, actual.Sum);
        Assert.Equal(expected.EvenSum, actual.EvenSum);
        Assert.Equal(expected.OddSum, actual.OddSum);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected.Min, actual.Min);
        Assert.Equal(expected.Max, actual.Max);
        Assert.Equal(expected.Modulos.OrderBy(x => x), actual.Modulos.OrderBy(x => x));
        Assert.Equal(expected.Items, actual.Items);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task AccumulatorsWithDifferentTypes(bool isAsync)
    {
        var source = new[] { "one", "two", "three", "four", "five" };
        var asyncSource = source.ToAsyncEnumerable();

        var expected =
            source.Aggregate(
                0,
                (acc, s) => acc + s.Length,
                "",
                (acc, s) => acc + s[0],
                (totalLength, firstChars) => new { TotalLength = totalLength, FirstChars = firstChars });

        var actual =
            isAsync
                ? await asyncSource.AggregateAsync(
                    0,
                    async (acc, s, _) => acc + s.Length,
                    "",
                    async (acc, s, _) => acc + s[0],
                    async (totalLength, firstChars, _) => new { TotalLength = totalLength, FirstChars = firstChars })
                : await asyncSource.AggregateAsync(
                    0,
                    (acc, s) => acc + s.Length,
                    "",
                    (acc, s) => acc + s[0],
                    (totalLength, firstChars) => new { TotalLength = totalLength, FirstChars = firstChars });

        Assert.Equal(expected.TotalLength, actual.TotalLength);
        Assert.Equal(expected.FirstChars, actual.FirstChars);
    }
}

