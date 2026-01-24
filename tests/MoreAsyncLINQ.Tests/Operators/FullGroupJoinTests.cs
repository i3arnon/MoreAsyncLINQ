using static MoreLinq.Extensions.FullGroupJoinExtension;

namespace MoreAsyncLINQ.Tests;

public class FullGroupJoinTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        var first = AsyncEnumerable.Empty<int>();
        var second = AsyncEnumerable.Empty<int>();

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int>(null!, second, x => x, x => x));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int>(null!, x => x, x => x));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, x => x));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, x => x, null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int>(null!, second, x => x, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int>(null!, x => x, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, x => x, null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int, int>(null!, second, x => x, x => x, (_, _, _) => 0));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int, int>(null!, x => x, x => x, (_, _, _) => 0));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, x => x, (_, _, _) => 0));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, x => x, null!, (_, _, _) => 0));
        Assert.Throws<ArgumentNullException>("resultSelector", () => first.FullGroupJoin<int, int, int, int>(second, x => x, x => x, null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int, int>(null!, second, x => x, x => x, (_, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int, int>(null!, x => x, x => x, (_, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, x => x, (_, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, x => x, null!, (_, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("resultSelector", () => first.FullGroupJoin<int, int, int, int>(second, x => x, x => x, null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int>(null!, second, async (x, _) => x, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int>(null!, async (x, _) => x, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, async (x, _) => x, null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int>(null!, second, async (x, _) => x, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int>(null!, async (x, _) => x, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, async (x, _) => x, null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int, int>(null!, second, async (x, _) => x, async (x, _) => x, async (_, _, _, _) => 0));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int, int>(null!, async (x, _) => x, async (x, _) => x, async (_, _, _, _) => 0));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, async (x, _) => x, async (_, _, _, _) => 0));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, async (x, _) => x, null!, async (_, _, _, _) => 0));
        Assert.Throws<ArgumentNullException>("resultSelector", () => first.FullGroupJoin<int, int, int, int>(second, async (x, _) => x, async (x, _) => x, null!));

        Assert.Throws<ArgumentNullException>("first", () => MoreAsyncEnumerable.FullGroupJoin<int, int, int, int>(null!, second, async (x, _) => x, async (x, _) => x, async (_, _, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("second", () => first.FullGroupJoin<int, int, int, int>(null!, async (x, _) => x, async (x, _) => x, async (_, _, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("firstKeySelector", () => first.FullGroupJoin(second, null!, async (x, _) => x, async (_, _, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("secondKeySelector", () => first.FullGroupJoin(second, async (x, _) => x, null!, async (_, _, _, _) => 0, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("resultSelector", () => first.FullGroupJoin<int, int, int, int>(second, async (x, _) => x, async (x, _) => x, null!, EqualityComparer<int>.Default));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public void EmptySequence(bool isAsync)
    {
        var first = AsyncEnumerable.Empty<int>();
        var second = AsyncEnumerable.Empty<int>();

        var result =
            isAsync
                ? first.FullGroupJoin(
                    second,
                    element => element,
                    element => element)
                : first.FullGroupJoin(
                    second,
                    async (element, _) => element,
                    async (element, _) => element);

        AssertKnownEmpty(result);
    }

    [Fact]
    public void IsLazy()
    {
        var bs = new BreakingSequence<int>();

        var bf = BreakingFunc.Of<int, int>();
        var bfr = BreakingFunc.Of<int, IEnumerable<int>, IEnumerable<int>, int>();

        var bfAsync = BreakingFunc.OfAsync<int, int>();
        var bfrAsync = BreakingFunc.OfAsync<int, IEnumerable<int>, IEnumerable<int>, int>();

        _ = bs.FullGroupJoin(bs, bf, bf);
        _ = bs.FullGroupJoin(bs, bf, bf, EqualityComparer<int>.Default);
        _ = bs.FullGroupJoin(bs, bf, bf, bfr);
        _ = bs.FullGroupJoin(bs, bf, bf, bfr, EqualityComparer<int>.Default);

        _ = bs.FullGroupJoin(bs, bfAsync, bfAsync);
        _ = bs.FullGroupJoin(bs, bfAsync, bfAsync, EqualityComparer<int>.Default);
        _ = bs.FullGroupJoin(bs, bfAsync, bfAsync, bfrAsync);
        _ = bs.FullGroupJoin(bs, bfAsync, bfAsync, bfrAsync, EqualityComparer<int>.Default);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Results(bool isAsync)
    {
        var first = new[] { 1, 2 };
        var second = new[] { 2, 3 };

        await TestFullGroupJoin(first, second, isAsync);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EmptyLeft(bool isAsync)
    {
        var first = Array.Empty<int>();
        var second = new[] { 2, 3 };
        
        await TestFullGroupJoin(first, second, isAsync);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EmptyRight(bool isAsync)
    {
        var first = new[] { 2, 3 };
        var second = Array.Empty<int>();

        await TestFullGroupJoin(first, second, isAsync);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PreservesOrder(bool isAsync)
    {
        var first =
            new[]
            {
                (3, 1),
                (1, 1),
                (2, 1),
                (1, 2),
                (1, 3),
                (3, 2),
                (1, 4),
                (3, 3)
            };
        var second =
            new[]
            {
                (4, 1),
                (3, 1),
                (2, 1),
                (0, 1),
                (3, 0)
            };

        var firstAsync = first.ToAsyncEnumerable();
        var secondAsync = second.ToAsyncEnumerable();

        var result =
            isAsync
                ? await firstAsync.
                    FullGroupJoin(
                        secondAsync,
                        tuple => tuple.Item1,
                        tuple => tuple.Item1).
                    ToListAsync()
                : await firstAsync.
                    FullGroupJoin(
                        secondAsync,
                        async (tuple, _) => tuple.Item1,
                        async (tuple, _) => tuple.Item1).
                    ToListAsync();

        Assert.Equal(
            [3, 1, 2, 4, 0],
            result.Select(tuple => tuple.Key));

        foreach (var (key, firstResult, secondResult) in result)
        {
            Assert.Equal(
                first.Where(tuple => tuple.Item1 == key),
                firstResult);
            Assert.Equal(
                second.Where(tuple => tuple.Item1 == key),
                secondResult);
        }
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task WithComparer(bool isAsync)
    {
        var first = new[] { "a", "B" };
        var second = new[] { "b", "C" };

        await TestFullGroupJoin(first, second, isAsync, StringComparer.OrdinalIgnoreCase);
    }

    private async Task TestFullGroupJoin<T>(
        IReadOnlyCollection<T> first,
        IReadOnlyCollection<T> second,
        bool isAsync,
        IEqualityComparer<T>? comparer = null)
        where T : notnull
    {
        var firstAsync = first.ToAsyncEnumerable();
        var secondAsync = second.ToAsyncEnumerable();

        var actual =
            isAsync
                ? firstAsync.FullGroupJoin(
                    secondAsync,
                    element => element,
                    element => element,
                    comparer)
                : firstAsync.FullGroupJoin(
                    secondAsync,
                    async (element, _) => element,
                    async (element, _) => element,
                    comparer);

        var expected =
            first.FullGroupJoin(
                second,
                element => element,
                element => element,
                comparer);

        var expectedDictionary = expected.ToDictionary(tuple => tuple.Key);
        var actualDictionary = await actual.ToDictionaryAsync(tuple => tuple.Key);

        Assert.Equal(expectedDictionary.Keys, actualDictionary.Keys);
        foreach (var key in expectedDictionary.Keys)
        {
            Assert.Equal(expectedDictionary[key].First, actualDictionary[key].First);
            Assert.Equal(expectedDictionary[key].Second, actualDictionary[key].Second);
        }
    }
}
