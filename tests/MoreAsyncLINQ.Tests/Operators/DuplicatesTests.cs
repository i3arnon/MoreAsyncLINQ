using MoreLinq;

namespace MoreAsyncLINQ.Tests;

public class DuplicatesTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.Duplicates<int>(null!));
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.Duplicates(null!, EqualityComparer<int>.Default));
    }

    [Fact]
    public void EmptySequence()
    {
        var source = AsyncEnumerable.Empty<int>();

        var duplicates = source.Duplicates();

        AssertKnownEmpty(duplicates);
    }

    [Fact]
    public void IsLazy()
    {
        var bs = new BreakingSequence<object>();

        _ = bs.Duplicates();
        _ = bs.Duplicates(EqualityComparer<object>.Default);
    }

    [Fact]
    public async Task StreamsDuplicatesAsTheyAreDiscovered()
    {
        var source = Source();
        var asyncSource = Source().ToAsyncEnumerable();

        await AssertEqual(
            source.Duplicates().Take(1),
            asyncSource.Duplicates().Take(1));

        return;

        static IEnumerable<string> Source()
        {
            yield return "DUPLICATED_STRING";
            yield return "DUPLICATED_STRING";
            throw new InvalidOperationException("Should not reach here");
        }
    }

    [Fact]
    public async Task SequenceWithoutDuplicatesReturnsEmptySequence()
    {
        var source = new[] { "FirstElement", "SecondElement", "ThirdElement" };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Duplicates(),
            asyncSource.Duplicates());
    }

    [Fact]
    public async Task SequenceWithDuplicatesReturnsDuplicates()
    {
        var source =
            new[]
            {
                "FirstElement",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "ThirdElement"
            };

        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Duplicates(),
            asyncSource.Duplicates());
    }

    [Fact]
    public async Task SequenceWithMultipleDuplicatesReturnsOneInstanceOfEachDuplicate()
    {
        var source =
            new[]
            {
                "FirstElement",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "ThirdElement",
                "SECOND_DUPLICATED_STRING",
                "SECOND_DUPLICATED_STRING"
            };

        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Duplicates(),
            asyncSource.Duplicates());
    }

    [Fact]
    public async Task SequenceWithDuplicatesUsingComparerThatAlwaysReturnsFalseReturnsEmptySequence()
    {
        var source = new[] { "DUPLICATED_STRING", "DUPLICATED_STRING", "DUPLICATED_STRING" };
        var asyncSource = source.ToAsyncEnumerable();

        var comparer = new DelegatingEqualityComparer<string>((_, _) => false, _ => 0);

        await AssertEqual(
            source.Duplicates(comparer),
            asyncSource.Duplicates(comparer));
    }

    [Fact]
    public async Task SequenceWithDuplicatesUsingCaseInsensitiveComparer()
    {
        var source = new[] { "foo", "FOO", "bar", "Bar", "baz" };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Duplicates(StringComparer.OrdinalIgnoreCase),
            asyncSource.Duplicates(StringComparer.OrdinalIgnoreCase));
    }

    private sealed class DelegatingEqualityComparer<T>(
        Func<T, T, bool> equals,
        Func<T, int> getHashCode) : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y) => equals(x!, y!);
        public int GetHashCode(T obj) => getHashCode(obj);
    }
}
