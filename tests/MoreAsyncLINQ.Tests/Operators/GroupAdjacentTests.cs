using static MoreLinq.Extensions.GroupAdjacentExtension;

namespace MoreAsyncLINQ.Tests;

public class GroupAdjacentTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        var source = AsyncEnumerable.Empty<int>();

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int>(null!, x => x));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int>(null!, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, x => x, x => x));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, x => x, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, x => x, (_, e) => e.Count()));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, x => x, (_, e) => e.Count(), EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!, x => x));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!, x => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!, (_, e) => e.Count()));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, int>)null!, (_, e) => e.Count(), EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("elementSelector", () => source.GroupAdjacent(x => x, (Func<int, int>)null!));
        Assert.Throws<ArgumentNullException>("elementSelector", () => source.GroupAdjacent(x => x, (Func<int, int>)null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("resultSelector", () => source.GroupAdjacent(x => x, (Func<int, IEnumerable<int>, int>)null!));
        Assert.Throws<ArgumentNullException>("resultSelector", () => source.GroupAdjacent(x => x, (Func<int, IEnumerable<int>, int>)null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int>(null!, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int>(null!, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, async (x, _) => x, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, async (x, _) => x, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, async (x, _) => x, async (_, e, _) => e.Count()));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.GroupAdjacent<int, int, int>(null!, async (x, _) => x, async (_, e, _) => e.Count(), EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!, async (x, _) => x));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!, async (x, _) => x, EqualityComparer<int>.Default));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!, async (_, e, _) => e.Count()));
        Assert.Throws<ArgumentNullException>("keySelector", () => source.GroupAdjacent((Func<int, CancellationToken, ValueTask<int>>)null!, async (_, e, _) => e.Count(), EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("elementSelector", () => source.GroupAdjacent(async (x, _) => x, (Func<int, CancellationToken, ValueTask<int>>)null!));
        Assert.Throws<ArgumentNullException>("elementSelector", () => source.GroupAdjacent(async (x, _) => x, (Func<int, CancellationToken, ValueTask<int>>)null!, EqualityComparer<int>.Default));

        Assert.Throws<ArgumentNullException>("resultSelector", () => source.GroupAdjacent(async (x, _) => x, (Func<int, IEnumerable<int>, CancellationToken, ValueTask<int>>)null!));
        Assert.Throws<ArgumentNullException>("resultSelector", () => source.GroupAdjacent(async (x, _) => x, (Func<int, IEnumerable<int>, CancellationToken, ValueTask<int>>)null!, EqualityComparer<int>.Default));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EmptySequence(bool isAsync)
    {
        var source = AsyncEnumerable.Empty<int>();

        var groupings =
            isAsync
                ? source.GroupAdjacent(element => element)
                : source.GroupAdjacent(async (element, _) => element);
        
        AssertKnownEmpty(groupings);
    }

    [Fact]
    public void IsLazy()
    {
        var bs = new BreakingSequence<object>();

        var bf = BreakingFunc.Of<object, int>();
        var bfo = BreakingFunc.Of<object, object>();
        var bfg = BreakingFunc.Of<int, IEnumerable<object>, IEnumerable<object>>();
        
        var bfAsync = BreakingFunc.OfAsync<object, int>();
        var bfoAsync = BreakingFunc.OfAsync<object, object>();
        var bfgAsync = BreakingFunc.OfAsync<int, IEnumerable<object>, IEnumerable<object>>();
        
        _ = bs.GroupAdjacent(bf);
        _ = bs.GroupAdjacent(bf, bfo);
        _ = bs.GroupAdjacent(bf, bfo, EqualityComparer<int>.Default);
        _ = bs.GroupAdjacent(bf, EqualityComparer<int>.Default);
        _ = bs.GroupAdjacent(bf, bfg);
        _ = bs.GroupAdjacent(bf, bfg, EqualityComparer<int>.Default);
        
        _ = bs.GroupAdjacent(bfAsync);
        _ = bs.GroupAdjacent(bfAsync, bfoAsync);
        _ = bs.GroupAdjacent(bfAsync, bfoAsync, EqualityComparer<int>.Default);
        _ = bs.GroupAdjacent(bfAsync, EqualityComparer<int>.Default);
        _ = bs.GroupAdjacent(bfAsync, bfgAsync);
        _ = bs.GroupAdjacent(bfAsync, bfgAsync, EqualityComparer<int>.Default);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequence(bool isAsync)
    {
        const string one = "one";
        const string two = "two";
        const string three = "three";
        const string four = "four";
        const string five = "five";
        const string six = "six";
        const string seven = "seven";
        const string eight = "eight";
        const string nine = "nine";
        const string ten = "ten";

        var source = new[] { one, two, three, four, five, six, seven, eight, nine, ten };
        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(element => element.Length)
                : asyncSource.GroupAdjacent(async (element, _) => element.Length);
        
        await AssertEqual(
            source.GroupAdjacent(element => element.Length),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceComparer(bool isAsync)
    {
        var source = new[] { "foo", "FOO", "Foo", "bar", "BAR", "Bar" };
        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(element => element, StringComparer.OrdinalIgnoreCase)
                : asyncSource.GroupAdjacent(async (element, _) => element, StringComparer.OrdinalIgnoreCase);

        await AssertEqual(
            source.GroupAdjacent(element => element, StringComparer.OrdinalIgnoreCase),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceElementSelector(bool isAsync)
    {
        var source =
            new[]
            {
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 789 },
                new { Month = 2, Value = 987 },
                new { Month = 2, Value = 654 },
                new { Month = 2, Value = 321 },
                new { Month = 3, Value = 789 },
                new { Month = 3, Value = 456 },
                new { Month = 3, Value = 123 },
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 781 },
            };

        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(
                    element => element.Month,
                    element => element.Value * 2)
                : asyncSource.GroupAdjacent(
                    async (element, _) => element.Month,
                    async (element, _) => element.Value * 2);

        await AssertEqual(
            source.GroupAdjacent(
                element => element.Month,
                element => element.Value * 2),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceElementSelectorComparer(bool isAsync)
    {
        var source =
            new[]
            {
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 789 },
                new { Month = "feb", Value = 987 },
                new { Month = "Feb", Value = 654 },
                new { Month = "FEB", Value = 321 },
                new { Month = "mar", Value = 789 },
                new { Month = "Mar", Value = 456 },
                new { Month = "MAR", Value = 123 },
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 781 },
            };

        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(
                    element => element.Month,
                    element => element.Value * 2,
                    StringComparer.OrdinalIgnoreCase)
                : asyncSource.GroupAdjacent(
                    async (element, _) => element.Month,
                    async (element, _) => element.Value * 2,
                    StringComparer.OrdinalIgnoreCase);
        
        await AssertEqual(
            source.GroupAdjacent(
                element => element.Month,
                element => element.Value * 2,
                StringComparer.OrdinalIgnoreCase),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceResultSelector(bool isAsync)
    {
        var source =
            new[]
            {
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 789 },
                new { Month = 2, Value = 987 },
                new { Month = 2, Value = 654 },
                new { Month = 2, Value = 321 },
                new { Month = 3, Value = 789 },
                new { Month = 3, Value = 456 },
                new { Month = 3, Value = 123 },
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 781 },
            };

        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(
                    element => element.Month,
                    (_, elements) => elements.Sum(element => element.Value))
                : asyncSource.GroupAdjacent(
                    async (element, _) => element.Month,
                    async (_, elements, _) => elements.Sum(element => element.Value));

        await AssertEqual(
            source.GroupAdjacent(
                element => element.Month,
                (_, elements) => elements.Sum(element => element.Value)),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceResultSelectorComparer(bool isAsync)
    {
        var source =
            new[]
            {
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 789 },
                new { Month = "feb", Value = 987 },
                new { Month = "Feb", Value = 654 },
                new { Month = "FEB", Value = 321 },
                new { Month = "mar", Value = 789 },
                new { Month = "Mar", Value = 456 },
                new { Month = "MAR", Value = 123 },
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 781 },
            };

        var asyncSource = source.ToAsyncEnumerable();

        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(
                    element => element.Month,
                    (_, elements) => elements.Sum(element => element.Value),
                    StringComparer.OrdinalIgnoreCase)
                : asyncSource.GroupAdjacent(
                    async (element, _) => element.Month,
                    async (_, elements, _) => elements.Sum(element => element.Value),
                    StringComparer.OrdinalIgnoreCase);

        await AssertEqual(
            source.GroupAdjacent(
                element => element.Month,
                (_, elements) => elements.Sum(element => element.Value),
                StringComparer.OrdinalIgnoreCase),
            groupings);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SourceSequenceWithSomeNullKeys(bool isAsync)
    {
        var source =
            Enumerable.
                Range(1, 5).
                SelectMany(index => Enumerable.Repeat((int?)index, index).Append(null)).
                ToList();

        var asyncSource = source.ToAsyncEnumerable();
        
        var groupings =
            isAsync
                ? asyncSource.GroupAdjacent(element => element)
                : asyncSource.GroupAdjacent(async (element, _) => element);

        await AssertEqual(
            source.GroupAdjacent(element => element),
            groupings);
    }
}