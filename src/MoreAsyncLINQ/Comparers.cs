using System.Collections.Generic;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ
{
    internal static class Comparers
    {
        public static IComparer<T> Get<T>(
            IComparer<T>? comparer = default,
            OrderByDirection direction = Ascending)
        {
            comparer ??= Comparer<T>.Default;
            return direction == Descending
                ? new ReverseComparer<T>(comparer)
                : comparer;
        }

        private sealed class ReverseComparer<T>(IComparer<T> comparer) : IComparer<T>
        {
#if NET5_0_OR_GREATER
            public int Compare(T? first, T? second) =>
#else
            public int Compare(T first, T second) =>
#endif
                comparer.Compare(first, second) switch
                {
                    > 0 => -1,
                    0 => 0,
                    < 0 => 1
                };
        }
    }
}