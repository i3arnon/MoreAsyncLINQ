using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace MoreAsyncLINQ;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static partial class MoreAsyncEnumerable
{
    private static bool IsKnownEmpty<TResult>([NoEnumeration] this IAsyncEnumerable<TResult> source) =>
        ReferenceEquals(
            source,
            AsyncEnumerable.Empty<TResult>());
}