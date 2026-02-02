using MoreLinq;

namespace MoreAsyncLINQ.Tests;

public class CartesianTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int>(null!, AsyncEnumerable.Empty<int>(), (a, b) => a + b));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int>(null!, (a, b) => a + b));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), (Func<int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int>(null!, AsyncEnumerable.Empty<int>(), (a, b, _) => ValueTask.FromResult(a + b)));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int>(null!, (a, b, _) => ValueTask.FromResult(a + b)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), (Func<int, int, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c) => a + b + c));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), (a, b, c) => a + b + c));
        Assert.Throws<ArgumentNullException>("third", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int>(AsyncEnumerable.Empty<int>(), null!, (a, b, c) => a + b + c));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (Func<int, int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, _) => ValueTask.FromResult(a + b + c)));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), (a, b, c, _) => ValueTask.FromResult(a + b + c)));
        Assert.Throws<ArgumentNullException>("third", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int>(AsyncEnumerable.Empty<int>(), null!, (a, b, c, _) => ValueTask.FromResult(a + b + c)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (Func<int, int, int, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d) => a + b + c + d));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d) => a + b + c + d));
        Assert.Throws<ArgumentNullException>("third", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(AsyncEnumerable.Empty<int>(), null!, AsyncEnumerable.Empty<int>(), (a, b, c, d) => a + b + c + d));
        Assert.Throws<ArgumentNullException>("fourth", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), null!, (a, b, c, d) => a + b + c + d));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (Func<int, int, int, int, int>)null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.Cartesian<int, int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
        Assert.Throws<ArgumentNullException>("second", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(null!, AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
        Assert.Throws<ArgumentNullException>("third", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(AsyncEnumerable.Empty<int>(), null!, AsyncEnumerable.Empty<int>(), (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
        Assert.Throws<ArgumentNullException>("fourth", () => AsyncEnumerable.Empty<int>().Cartesian<int, int, int, int, int>(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), null!, (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (Func<int, int, int, int, CancellationToken, ValueTask<int>>)null!));
    }

    [Fact]
    public void EmptySequence()
    {
        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), (a, b) => a + b));

        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), (a, b, _) => ValueTask.FromResult(a + b)));

        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c) => a + b + c));

        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, _) => ValueTask.FromResult(a + b + c)));

        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d) => a + b + c + d));

        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Cartesian(AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), AsyncEnumerable.Empty<int>(), (a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));
    }

    [Fact]
    public void IsLazy()
    {
        var bs = new BreakingSequence<int>();

        _ = bs.Cartesian(new BreakingSequence<int>(), BreakingFunc.Of<int, int, int>());
        _ = bs.Cartesian(new BreakingSequence<int>(), BreakingFunc.OfAsync<int, int, int>());

        _ = bs.Cartesian(new BreakingSequence<int>(), new BreakingSequence<int>(), BreakingFunc.Of<int, int, int, int>());
        _ = bs.Cartesian(new BreakingSequence<int>(), new BreakingSequence<int>(), BreakingFunc.OfAsync<int, int, int, int>());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_BothEmpty(bool isAsync)
    {
        var sequenceA = Array.Empty<int>();
        var sequenceB = Array.Empty<int>();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();

        await AssertEqual(
            sequenceA.Cartesian(sequenceB, (a, b) => a + b),
            isAsync
                ? asyncA.Cartesian(asyncB, async (a, b, _) => a + b)
                : asyncA.Cartesian(asyncB, (a, b) => a + b));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_EmptyAndNonEmpty(bool isAsync)
    {
        var sequenceA = Array.Empty<int>();
        var sequenceB = Enumerable.Repeat(1, 10).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();

        await AssertEqual(
            sequenceA.Cartesian(sequenceB, (a, b) => a + b),
            isAsync
                ? asyncA.Cartesian(asyncB, async (a, b, _) => a + b)
                : asyncA.Cartesian(asyncB, (a, b) => a + b));
        
        asyncA = sequenceA.ToAsyncEnumerable();
        asyncB = sequenceB.ToAsyncEnumerable();

        await AssertEqual(
            sequenceB.Cartesian(sequenceA, (a, b) => a + b),
            isAsync
                ? asyncB.Cartesian(asyncA, async (a, b, _) => a + b)
                : asyncB.Cartesian(asyncA, (a, b) => a + b));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_ProductCount(bool isAsync)
    {
        const int countA = 10;
        const int countB = 7;
        var sequenceA = Enumerable.Range(1, countA).ToList();
        var sequenceB = Enumerable.Range(1, countB).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();

        var result =
            isAsync
                ? await asyncA.Cartesian(asyncB, async (a, b, _) => a + b).CountAsync()
                : await asyncA.Cartesian(asyncB, (a, b) => a + b).CountAsync();

        Assert.Equal(countA * countB, result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_ProductCombinations(bool isAsync)
    {
        var sequenceA = Enumerable.Range(0, 5).ToList();
        var sequenceB = Enumerable.Range(0, 5).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();

        await AssertEqual(
            sequenceA.Cartesian(sequenceB, (a, b) => (A: a, B: b)),
            isAsync
                ? asyncA.Cartesian(asyncB, async (a, b, _) => (A: a, B: b))
                : asyncA.Cartesian(asyncB, (a, b) => (A: a, B: b)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_AllCellsVisited(bool isAsync)
    {
        var sequenceA = Enumerable.Range(0, 5).ToList();
        var sequenceB = Enumerable.Range(0, 5).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();

        var expectedSet = Enumerable.Range(0, 5).Select(_ => new bool[5]).ToArray();

        var result =
            isAsync
                ? await asyncA.Cartesian(asyncB, async (a, b, _) => (A: a, B: b)).ToArrayAsync()
                : await asyncA.Cartesian(asyncB, (a, b) => (A: a, B: b)).ToArrayAsync();

        Assert.Equal(sequenceA.Count * sequenceB.Count, result.Length);

        foreach (var coord in result)
        {
            expectedSet[coord.A][coord.B] = true;
        }

        Assert.True(expectedSet.SelectMany(x => x).All(z => z));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThreeSequences_ProductCount(bool isAsync)
    {
        const int countA = 5;
        const int countB = 4;
        const int countC = 3;
        var sequenceA = Enumerable.Range(1, countA).ToList();
        var sequenceB = Enumerable.Range(1, countB).ToList();
        var sequenceC = Enumerable.Range(1, countC).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();
        var asyncC = sequenceC.ToAsyncEnumerable();

        var result =
            isAsync
                ? await asyncA.Cartesian(asyncB, asyncC, async (a, b, c, _) => a + b + c).CountAsync()
                : await asyncA.Cartesian(asyncB, asyncC, (a, b, c) => a + b + c).CountAsync();

        Assert.Equal(countA * countB * countC, result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThreeSequences_ProductCombinations(bool isAsync)
    {
        var sequenceA = Enumerable.Range(0, 3).ToList();
        var sequenceB = Enumerable.Range(0, 3).ToList();
        var sequenceC = Enumerable.Range(0, 3).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();
        var asyncC = sequenceC.ToAsyncEnumerable();

        await AssertEqual(
            sequenceA.Cartesian(sequenceB, sequenceC, (a, b, c) => (A: a, B: b, C: c)),
            isAsync
                ? asyncA.Cartesian(asyncB, asyncC, async (a, b, c, _) => (A: a, B: b, C: c))
                : asyncA.Cartesian(asyncB, asyncC, (a, b, c) => (A: a, B: b, C: c)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task FourSequences_ProductCount(bool isAsync)
    {
        const int countA = 4;
        const int countB = 3;
        const int countC = 3;
        const int countD = 2;
        var sequenceA = Enumerable.Range(1, countA).ToList();
        var sequenceB = Enumerable.Range(1, countB).ToList();
        var sequenceC = Enumerable.Range(1, countC).ToList();
        var sequenceD = Enumerable.Range(1, countD).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();
        var asyncC = sequenceC.ToAsyncEnumerable();
        var asyncD = sequenceD.ToAsyncEnumerable();

        var result =
            isAsync
                ? await asyncA.Cartesian(asyncB, asyncC, asyncD, async (a, b, c, d, _) => a + b + c + d).CountAsync()
                : await asyncA.Cartesian(asyncB, asyncC, asyncD, (a, b, c, d) => a + b + c + d).CountAsync();

        Assert.Equal(countA * countB * countC * countD, result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task FourSequences_ProductCombinations(bool isAsync)
    {
        var sequenceA = Enumerable.Range(0, 2).ToList();
        var sequenceB = Enumerable.Range(0, 2).ToList();
        var sequenceC = Enumerable.Range(0, 2).ToList();
        var sequenceD = Enumerable.Range(0, 2).ToList();
        var asyncA = sequenceA.ToAsyncEnumerable();
        var asyncB = sequenceB.ToAsyncEnumerable();
        var asyncC = sequenceC.ToAsyncEnumerable();
        var asyncD = sequenceD.ToAsyncEnumerable();

        await AssertEqual(
            sequenceA.Cartesian(sequenceB, sequenceC, sequenceD, (a, b, c, d) => (A: a, B: b, C: c, D: d)),
            isAsync
                ? asyncA.Cartesian(asyncB, asyncC, asyncD, async (a, b, c, d, _) => (A: a, B: b, C: c, D: d))
                : asyncA.Cartesian(asyncB, asyncC, asyncD, (a, b, c, d) => (A: a, B: b, C: c, D: d)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_WithStringResult(bool isAsync)
    {
        var letters = new[] { 'a', 'b', 'c' };
        var digits = new[] { 1, 2, 3 };
        var asyncLetters = letters.ToAsyncEnumerable();
        var asyncDigits = digits.ToAsyncEnumerable();

        await AssertEqual(
            letters.Cartesian(digits, (l, d) => $"{l}{d}"),
            isAsync
                ? asyncLetters.Cartesian(asyncDigits, async (l, d, _) => $"{l}{d}")
                : asyncLetters.Cartesian(asyncDigits, (l, d) => $"{l}{d}"));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task TwoSequences_PreservesOrder(bool isAsync)
    {
        var first = new[] { 1, 2 };
        var second = new[] { 'a', 'b', 'c' };
        var asyncFirst = first.ToAsyncEnumerable();
        var asyncSecond = second.ToAsyncEnumerable();

        // Expected order: (1,a), (1,b), (1,c), (2,a), (2,b), (2,c) - nested foreach order
        await AssertEqual(
            first.Cartesian(second, (f, s) => (f, s)),
            isAsync
                ? asyncFirst.Cartesian(asyncSecond, async (f, s, _) => (f, s))
                : asyncFirst.Cartesian(asyncSecond, (f, s) => (f, s)));
    }
}

