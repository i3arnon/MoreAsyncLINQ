using System.Globalization;
using MoreLinq;

namespace MoreAsyncLINQ.Tests;

public class ChooseTests : AsyncEnumerableTests
{
    [Fact]
    public void InvalidInputs_Throws()
    {
        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.Choose<int, int>(null!, _ => (false, 0)));
        Assert.Throws<ArgumentNullException>("chooser", () => AsyncEnumerable.Empty<int>().Choose((Func<int, (bool, int)>)null!));

        Assert.Throws<ArgumentNullException>("source", () => MoreAsyncEnumerable.Choose<int, int>(null!, (_, _) => ValueTask.FromResult((false, 0))));
        Assert.Throws<ArgumentNullException>("chooser", () => AsyncEnumerable.Empty<int>().Choose((Func<int, CancellationToken, ValueTask<(bool, int)>>)null!));
    }

    [Fact]
    public void EmptySequence()
    {
        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Choose(BreakingFunc.Of<int, (bool, int)>()));
        AssertKnownEmpty(AsyncEnumerable.Empty<int>().Choose(BreakingFunc.OfAsync<int, (bool, int)>()));
    }

    [Fact]
    public void IsLazy()
    {
        var bs = new BreakingSequence<object>();

        _ = bs.Choose(BreakingFunc.Of<object, (bool, object)>());
        _ = bs.Choose(BreakingFunc.OfAsync<object, (bool, object)>());
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task None(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Choose(_ => (false, 0)),
            isAsync
                ? asyncSource.Choose(async (_, _) => (false, 0))
                : asyncSource.Choose(_ => (false, 0)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThoseParsable(bool isAsync)
    {
        var source = "O,l,2,3,4,S,6,7,B,9".Split(',');
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Choose(value => (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number), number)),
            isAsync
                ? asyncSource.Choose(async (value, _) => (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number), number))
                : asyncSource.Choose(value => (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number), number)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThoseThatAreIntegers(bool isAsync)
    {
        var source = new int?[] { 0, 1, 2, null, 4, null, 6, null, null, 9 };
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Choose(value => value is { } number ? (true, number) : (false, 0)),
            isAsync
                ? asyncSource.Choose(async (value, _) => value is { } number ? (true, number) : (false, 0))
                : asyncSource.Choose(value => value is { } number ? (true, number) : (false, 0)));
    }

    [Theory]
    [MemberData(nameof(IsAsync))]
    public async Task ThoseEven(bool isAsync)
    {
        var source = Enumerable.Range(1, 10).ToList();
        var asyncSource = source.ToAsyncEnumerable();

        await AssertEqual(
            source.Choose(value => value % 2 is 0 ? (true, value) : (false, 0)),
            isAsync
                ? asyncSource.Choose(async (value, _) => value % 2 is 0 ? (true, value) : (false, 0))
                : asyncSource.Choose(value => value % 2 is 0 ? (true, value) : (false, 0)));
    }
}

