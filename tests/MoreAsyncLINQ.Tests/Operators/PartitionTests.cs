using MoreLinq;

namespace MoreAsyncLINQ.Tests;

public class PartitionTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync<int>(null!, _ => true));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().PartitionAsync((Func<int, bool>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync<int>(null!, (_, _) => ValueTask.FromResult(true)));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().PartitionAsync((Func<int, CancellationToken, ValueTask<bool>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, _ => true, (Func<IEnumerable<int>, IEnumerable<int>, int>)((_, _) => 0)));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().PartitionAsync(null!, (_, _) => 0));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, (_, _) => ValueTask.FromResult(true), (Func<IEnumerable<int>, IEnumerable<int>, CancellationToken, ValueTask<int>>)((_, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("predicate", () => AsyncEnumerable.Empty<int>().PartitionAsync(null!, (_, _, _) => ValueTask.FromResult(0)));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, (Func<IEnumerable<int>, IEnumerable<int>, int>)((_, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<bool, int>>().PartitionAsync((Func<IEnumerable<int>, IEnumerable<int>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, (Func<IEnumerable<int>, IEnumerable<int>, CancellationToken, ValueTask<int>>)((_, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<bool, int>>().PartitionAsync((Func<IEnumerable<int>, IEnumerable<int>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, int>)((_, _, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<bool?, int>>().PartitionAsync((Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, CancellationToken, ValueTask<int>>)((_, _, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<bool?, int>>().PartitionAsync((Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, null, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, null, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, null, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, null, (Func<IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, 2, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _, _, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, 2, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, 2, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, 2, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, 2, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)((_, _, _, _) => 0)));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, 2, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.PartitionAsync(null!, 0, 1, 2, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)((_, _, _, _, _) => ValueTask.FromResult(0))));
        Assert.Throws<ArgumentNullException>("resultSelector", () => AsyncEnumerable.Empty<IGrouping<int, int>>().PartitionAsync(0, 1, 2, null, (Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<IGrouping<int, int>>, CancellationToken, ValueTask<int>>)null!));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task EmptySequence(bool isAsync)
    {
        var source = AsyncEnumerable.Empty<int>();

        var (evens, odds) =
            isAsync
                ? await source.PartitionAsync(async (number, _) => number % 2 == 0)
                : await source.PartitionAsync(number => number % 2 == 0);

        Assert.Empty(evens);
        Assert.Empty(odds);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task Partition(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        AssertEqual(
            source.Partition(number => number % 2 == 0),
            isAsync
                ? await asyncSource.PartitionAsync(async (number, _) => number % 2 == 0)
                : await asyncSource.PartitionAsync(number => number % 2 == 0));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionWithResultSelector(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        AssertEqual(
            source.Partition(number => number % 2 == 0, ValueTuple.Create),
            isAsync
                ? await asyncSource.PartitionAsync(async (number, _) => number % 2 == 0, async (@true, @false, _) => (@true, @false))
                : await asyncSource.PartitionAsync(number => number % 2 == 0, ValueTuple.Create));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionBooleanGrouping(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        AssertEqual(
            source.GroupBy(number => number % 2 == 0).Partition(ValueTuple.Create),
            isAsync
                ? await asyncSource.GroupBy(async (number, _) => number % 2 == 0).PartitionAsync(async (@true, @false, _) => (@true, @false))
                : await asyncSource.GroupBy(number => number % 2 == 0).PartitionAsync(ValueTuple.Create));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionNullableBooleanGrouping(bool isAsync)
    {
        var source = new int?[] { 1, 2, 3, null, 5, 6, 7, null, 9, 10 };
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.GroupBy(number => number != null ? number < 5 : (bool?)null).Partition(ValueTuple.Create);

        var actual =
            isAsync
                ? await asyncSource.GroupBy(async (number, _) => number != null ? number < 5 : (bool?)null).PartitionAsync(async (@true, @false, @null, _) => (@true, @false, @null))
                : await asyncSource.GroupBy(number => number != null ? number < 5 : (bool?)null).PartitionAsync(ValueTuple.Create);

        Assert.Equal(expected.Item1, actual.Item1);
        Assert.Equal(expected.Item2, actual.Item2);
        Assert.Equal(expected.Item3, actual.Item3);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithSingleKey(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.GroupBy(number => number % 3).Partition(0, ValueTuple.Create);

        var (m3, etc) =
            isAsync
                ? await asyncSource.GroupBy(async (number, _) => number % 3).PartitionAsync(0, async (g, e, _) => (g, e))
                : await asyncSource.GroupBy(number => number % 3).PartitionAsync(0, ValueTuple.Create);

        Assert.Equal(expected.Item1, m3);
        AssertEqual(expected.Item2.ToList(), etc.ToList());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithTwoKeys(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.GroupBy(x => x % 3).Partition(0, 1, ValueTuple.Create);

        var (m0, m1, etc) =
            isAsync
                ? await asyncSource.GroupBy(async (number, _) => number % 3).PartitionAsync(0, 1, async (g0, g1, e, _) => (g0, g1, e))
                : await asyncSource.GroupBy(x => x % 3).PartitionAsync(0, 1, ValueTuple.Create);

        Assert.Equal(expected.Item1, m0);
        Assert.Equal(expected.Item2, m1);
        AssertEqual(expected.Item3.ToList(), etc.ToList());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithThreeKeys(bool isAsync)
    {
        var source = Enumerable.Range(0, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        var expected = source.GroupBy(number => number % 3).Partition(0, 1, 2, ValueTuple.Create);

        var (m0, m1, m2, etc) =
            isAsync
                ? await asyncSource.GroupBy(async (number, _) => number % 3).PartitionAsync(0, 1, 2, async (g0, g1, g2, e, _) => (g0, g1, g2, e))
                : await asyncSource.GroupBy(number => number % 3).PartitionAsync(0, 1, 2, ValueTuple.Create);

        Assert.Equal(expected.Item1, m0);
        Assert.Equal(expected.Item2, m1);
        Assert.Equal(expected.Item3, m2);
        Assert.Empty(etc);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithSingleKeyWithComparer(bool isAsync)
    {
        var words = new[] { "foo", "bar", "FOO", "Bar" };
        var asyncWords = words.ToAsyncEnumerable();

        var expected = words.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).Partition("foo", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        var (foo, etc) =
            isAsync
                ? await asyncWords.GroupBy(async (word, _) => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", StringComparer.OrdinalIgnoreCase, async (g, e, _) => (g, e))
                : await asyncWords.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        Assert.Equal(expected.Item1, foo);
        AssertEqual(expected.Item2.ToList(), etc.ToList());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithTwoKeysWithComparer(bool isAsync)
    {
        var words = new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };
        var asyncWords = words.ToAsyncEnumerable();

        var expected = words.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        var (foos, bar, etc) =
            isAsync
                ? await asyncWords.GroupBy(async (word, _) => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", "bar", StringComparer.OrdinalIgnoreCase, async (g0, g1, e, _) => (g0, g1, e))
                : await asyncWords.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", "bar", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        Assert.Equal(expected.Item1, foos);
        Assert.Equal(expected.Item2, bar);
        AssertEqual(expected.Item3.ToList(), etc.ToList());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task PartitionGroupingWithThreeKeysWithComparer(bool isAsync)
    {
        var words = new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };
        var asyncWords = words.ToAsyncEnumerable();

        var expected = words.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).Partition("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        var (foos, bar, baz, etc) =
            isAsync
                ? await asyncWords.GroupBy(async (word, _) => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, async (g0, g1, g2, e, _) => (g0, g1, g2, e))
                : await asyncWords.GroupBy(word => word, StringComparer.OrdinalIgnoreCase).PartitionAsync("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

        Assert.Equal(expected.Item1, foos);
        Assert.Equal(expected.Item2, bar);
        Assert.Equal(expected.Item3, baz);
        AssertEqual(expected.Item4.ToList(), etc.ToList());
    }

    private static void AssertEqual<T>(
        (IEnumerable<T> True, IEnumerable<T> False) expected,
        (IEnumerable<T> True, IEnumerable<T> False) actual)
    {
        Assert.Equal(expected.True, actual.True);
        Assert.Equal(expected.False, actual.False);
    }

    private static void AssertEqual<T>(
        IReadOnlyList<IGrouping<T, T>> expected,
        IReadOnlyList<IGrouping<T, T>> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (var i = 0; i < actual.Count; i++)
        {
            Assert.Equal(expected[i].Key, actual[i].Key);
            Assert.Equal(expected[i].ToList(), actual[i].ToList());
        }
    }
}