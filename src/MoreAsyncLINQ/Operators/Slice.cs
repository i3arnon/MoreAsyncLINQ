using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Extracts a contiguous count of elements from a sequence at a particular zero-based starting index
        /// </summary>
        /// <remarks>
        /// If the starting position or count specified result in slice extending past the end of the sequence,
        /// it will return all elements up to that point. There is no guarantee that the resulting sequence will
        /// contain the number of elements requested - it may have anywhere from 0 to <paramref name="count"/>.<br/>
        /// This method is implemented in an optimized manner for any sequence implementing <c>IList{T}</c>.<br/>
        /// The result of Slice() is identical to: <c>sequence.Skip(startIndex).Take(count)</c>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <param name="source">The sequence from which to extract elements</param>
        /// <param name="startIndex">The zero-based index at which to begin slicing</param>
        /// <param name="count">The number of items to slice out of the index</param>
        /// <returns>A new sequence containing any elements sliced out from the source sequence</returns>
        public static IAsyncEnumerable<TSource> Slice<TSource>(
            this IAsyncEnumerable<TSource> source,
            int startIndex,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.Skip(startIndex).Take(count);
        }
    }
}