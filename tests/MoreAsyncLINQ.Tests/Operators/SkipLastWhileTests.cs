using MoreLinq;

namespace MoreAsyncLINQ.Tests;

public class SkipLastWhileTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.SkipLastWhile<int>(null!, _ => true));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().SkipLastWhile((Func<int, bool>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.SkipLastWhile<int>(null!, (_, _) => ValueTask.FromResult(true)));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().SkipLastWhile((Func<int, CancellationToken, ValueTask<bool>>)null!));
    }

    [Fact]
    public void EmptySequence()
    {
        var source = AsyncEnumerable.Empty<int>();

        var result = source.SkipLastWhile(_ => true);

        AssertKnownEmpty(result);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public void IsLazy(bool isAsync)
    {
        var bs = new BreakingSequence<object>();

        _ = isAsync
            ? bs.SkipLastWhile(BreakingFunc.OfAsync<object, bool>())
            : bs.SkipLastWhile(BreakingFunc.Of<object, bool>());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PredicateNeverFalse(bool isAsync)
    {
        var source = new[] { 0, 1, 2, 3, 4 };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.SkipLastWhile(x => x < 5),
            isAsync
                ? asyncSource.SkipLastWhile(async (number, _) => number < 5)
                : asyncSource.SkipLastWhile(number => number < 5));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PredicateNeverTrue(bool isAsync)
    {
        var source = new[] { 0, 1, 2, 3, 4 };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.SkipLastWhile(x => x == 100),
            isAsync
                ? asyncSource.SkipLastWhile(async (number, _) => number == 100)
                : asyncSource.SkipLastWhile(number => number == 100));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PredicateBecomesTruePartWay(bool isAsync)
    {
        var source = new[] { 0, 1, 2, 3, 4 };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.SkipLastWhile(x => x > 2),
            isAsync
                ? asyncSource.SkipLastWhile(async (number, _) => number > 2)
                : asyncSource.SkipLastWhile(number => number > 2));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task NeverEvaluatesPredicateWhenSourceIsEmpty(bool isAsync)
    {
        var source = Array.Empty<int>();
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.SkipLastWhile(BreakingFunc.Of<int, bool>()),
            isAsync
                ? asyncSource.SkipLastWhile(BreakingFunc.OfAsync<int, bool>())
                : asyncSource.SkipLastWhile(BreakingFunc.Of<int, bool>()));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task KeepsNonTrailingItemsThatMatchPredicate(bool isAsync)
    {
        var source = new[] { 1, 2, 0, 0, 3, 4, 0, 0 };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.SkipLastWhile(x => x == 0),
            isAsync
                ? asyncSource.SkipLastWhile(async (number, _) => number == 0)
                : asyncSource.SkipLastWhile(number => number == 0));
    }
}

