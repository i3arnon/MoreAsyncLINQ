#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0009 // Member access should be qualified
#pragma warning disable IDE1006 // Naming rule violation

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

/// <summary>
/// A <see cref="ILookup{TKey,TElement}"/> implementation that preserves insertion order
/// </summary>
/// <remarks>
/// This implementation preserves insertion order of keys and elements within each <see
/// cref="IEnumerable{T}"/>. Copied and modified from
/// <c><a href="https://github.com/morelinq/MoreLINQ/blob/v4.4.0/MoreLinq/Lookup.cs">Lookup.cs</a></c>
/// </remarks>
internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
{
    private readonly IEqualityComparer<TKey> _comparer;
    private Grouping<TKey, TElement>[] _groupings;
    private Grouping<TKey, TElement>? _lastGrouping;

    internal static async ValueTask<Lookup<TKey, TElement>> CreateForJoinAsync(
        IAsyncEnumerable<TElement> source,
        Func<TElement, TKey> keySelector,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken)
    {
        var lookup = new Lookup<TKey, TElement>(comparer);

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            var key = keySelector(element);
            if (key is not null)
            {
                var grouping = Assume.NotNull(lookup.GetGrouping(key, create: true));
                grouping.Add(element);
            }
        }

        return lookup;
    }

    internal static async ValueTask<Lookup<TKey, TElement>> CreateForJoinAsync(
        IAsyncEnumerable<TElement> source,
        Func<TElement, CancellationToken, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken)
    {
        var lookup = new Lookup<TKey, TElement>(comparer);

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            var key = await keySelector(element, cancellationToken);
            if (key is not null)
            {
                var grouping = Assume.NotNull(lookup.GetGrouping(key, create: true));
                grouping.Add(element);
            }
        }

        return lookup;
    }

    private Lookup(IEqualityComparer<TKey>? comparer)
    {
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
        _groupings = new Grouping<TKey, TElement>[7];
    }

    public int Count { get; private set; }

    public IEnumerable<TElement> this[TKey key]
    {
        get
        {
            var grouping = GetGrouping(key, create: false);
            return grouping ?? Enumerable.Empty<TElement>();
        }
    }

    public bool Contains(TKey key) => GetGrouping(key, create: false) is not null;

    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
        var grouping = _lastGrouping;
        if (grouping is not null)
        {
            do
            {
                grouping = Assume.NotNull(grouping._next);
                yield return grouping;
            } while (grouping != _lastGrouping);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int InternalGetHashCode(TKey key) =>
        key is null
            ? 0
            : _comparer.GetHashCode(key) & 0x7FFFFFFF;

    private Grouping<TKey, TElement>? GetGrouping(TKey key, bool create)
    {
        var hashCode = InternalGetHashCode(key);
        for (var g = _groupings[hashCode % _groupings.Length]; g is not null; g = g._hashNext)
        {
            if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
            {
                return g;
            }
        }

        if (create)
        {
            if (Count == _groupings.Length)
            {
                Resize();
            }

            var index = hashCode % _groupings.Length;
            var g = new Grouping<TKey, TElement>(key, hashCode);
            g._hashNext = _groupings[index];
            _groupings[index] = g;
            if (_lastGrouping is null)
            {
                g._next = g;
            }
            else
            {
                g._next = _lastGrouping._next;
                _lastGrouping._next = g;
            }

            _lastGrouping = g;
            Count++;
            return g;
        }

        return null;
    }

    private void Resize()
    {
        var newSize = checked((Count * 2) + 1);
        var newGroupings = new Grouping<TKey, TElement>[newSize];
        var g = Assume.NotNull(_lastGrouping);
        do
        {
            g = Assume.NotNull(g._next);
            var index = g._hashCode % newSize;
            g._hashNext = newGroupings[index];
            newGroupings[index] = g;
        } while (g != _lastGrouping);

        _groupings = newGroupings;
    }
}

// Modified from:
// https://github.com/morelinq/MoreLINQ/blob/v4.4.0/MoreLinq/Lookup.cs
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
{
    internal readonly TKey _key;
    internal readonly int _hashCode;
    private TElement[] _elements;
    private int _count;
    internal Grouping<TKey, TElement>? _hashNext;
    internal Grouping<TKey, TElement>? _next;

    internal Grouping(TKey key, int hashCode)
    {
        _key = key;
        _hashCode = hashCode;
        _elements = new TElement[1];
    }

    internal void Add(TElement element)
    {
        if (_elements.Length == _count)
        {
            Array.Resize(ref _elements, checked(_count * 2));
        }

        _elements[_count] = element;
        _count++;
    }

    internal void Trim()
    {
        if (_elements.Length != _count)
        {
            Array.Resize(ref _elements, _count);
        }
    }

    public IEnumerator<TElement> GetEnumerator()
    {
        for (var i = 0; i < _count; i++)
        {
            yield return _elements[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TKey Key => _key;

    int ICollection<TElement>.Count => _count;

    bool ICollection<TElement>.IsReadOnly => true;

    bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

    void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
        Array.Copy(_elements, 0, array, arrayIndex, _count);

    int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

    TElement IList<TElement>.this[int index]
    {
        get => index < 0 || index >= _count
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : _elements[index];

        set => ThrowModificationNotSupportedException();
    }

    void ICollection<TElement>.Add(TElement item) => ThrowModificationNotSupportedException();
    void ICollection<TElement>.Clear() => ThrowModificationNotSupportedException();

    bool ICollection<TElement>.Remove(TElement item)
    {
        ThrowModificationNotSupportedException();
        return false;
    }

    void IList<TElement>.Insert(int index, TElement item) => ThrowModificationNotSupportedException();
    void IList<TElement>.RemoveAt(int index) => ThrowModificationNotSupportedException();

    [DoesNotReturn]
    private static void ThrowModificationNotSupportedException() => throw new NotSupportedException("Grouping is immutable.");
}

internal static class Assume
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>(T? obj) where T : class
    {
        Debug.Assert(obj is not null);
        return obj!;
    }
}