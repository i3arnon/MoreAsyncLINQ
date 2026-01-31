using MoreLinq;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ.Tests;

public class ForEachTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.ForEachAsync<int>(null!, _ => { }));
        Assert.Throws<ArgumentNullException>("action", () => Empty<int>().ForEachAsync((Action<int>)null!));
        
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.ForEachAsync<int>(null!, (_, _) => { }));
        Assert.Throws<ArgumentNullException>("action", () => Empty<int>().ForEachAsync((Action<int, int>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.ForEachAsync<int>(null!, (_, _) => ValueTask.CompletedTask));
        Assert.Throws<ArgumentNullException>("action", () => Empty<int>().ForEachAsync((Func<int, CancellationToken, ValueTask>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.ForEachAsync<int>(null!, (_, _, _) => ValueTask.CompletedTask));
        Assert.Throws<ArgumentNullException>("action", () => Empty<int>().ForEachAsync((Func<int, int, CancellationToken, ValueTask>)null!));
    }

    [Fact]
    public async Task EmptySequence()
    {
        await Empty<int>().ForEachAsync(BreakingFunc.OfAction<int>());
        await Empty<int>().ForEachAsync(BreakingFunc.OfAsyncAction<int>());
        await Empty<int>().ForEachAsync(BreakingFunc.OfAction<int, int>());
        await Empty<int>().ForEachAsync(BreakingFunc.OfAsyncAction<int, int>());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ForEachWithSequence(bool isAsync)
    {
        var source = new[] { 1, 2, 3 };
        var asyncSource = source.ToAsyncEnumerable();

        var expected = new List<int>();
        source.ForEach(expected.Add);

        var actual = new List<int>();
        if (isAsync)
        {
            await asyncSource.ForEachAsync(async (item, _) => actual.Add(item));
        }
        else
        {
            await asyncSource.ForEachAsync(actual.Add);
        }

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ForEachIndexedWithSequence(bool isAsync)
    {
        var source = new[] { 9, 7, 8 };
        var asyncSource = source.ToAsyncEnumerable();

        var expectedValues = new List<int>();
        var expectedIndices = new List<int>();
        source.ForEach((x, index) =>
        {
            expectedValues.Add(x);
            expectedIndices.Add(index);
        });

        var actualValues = new List<int>();
        var actualIndices = new List<int>();
        if (isAsync)
        {
            await asyncSource.ForEachAsync(async (item, index, _) =>
            {
                actualValues.Add(item);
                actualIndices.Add(index);
            });
        }
        else
        {
            await asyncSource.ForEachAsync((x, index) =>
            {
                actualValues.Add(x);
                actualIndices.Add(index);
            });
        }

        Assert.Equal(expectedValues, actualValues);
        Assert.Equal(expectedIndices, actualIndices);
    }
}

