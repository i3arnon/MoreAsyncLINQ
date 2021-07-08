using System.Collections.Generic;
using static System.Linq.Enumerable;

namespace MoreAsyncLINQ
{
    internal abstract class Extrema<TStore, T>
    {
        public abstract void AddItem(ref TStore store, int? limit, T item);
        public abstract IEnumerable<T> GetEnumerable(TStore store);
        public abstract TStore InitializeStore();
        public abstract void ResetStore(ref TStore store);
    }

    internal static class Extrema<T>
    {
        public static Extrema<List<T>?, T> First { get; } = new FirstExtrema();
        public static Extrema<Queue<T>?, T> Last { get; } = new LastExtrema();

        private sealed class FirstExtrema : Extrema<List<T>?, T>
        {
            public override void AddItem(ref List<T>? store, int? limit, T item)
            {
                if (store is not null && limit is not null && store.Count == limit)
                {
                    return;
                }

                store ??= new List<T>();
                store.Add(item);
            }

            public override IEnumerable<T> GetEnumerable(List<T>? store) => store ?? Empty<T>();
            public override List<T>? InitializeStore() => null;
            public override void ResetStore(ref List<T>? store) => store = null;
        }

        private sealed class LastExtrema : Extrema<Queue<T>?, T>
        {
            public override void AddItem(ref Queue<T>? store, int? limit, T item)
            {
                if (store is not null && limit is not null && store.Count == limit)
                {
                    store.Dequeue();
                }

                store ??= new Queue<T>();
                store.Enqueue(item);
            }

            public override IEnumerable<T> GetEnumerable(Queue<T>? store) => store ?? Empty<T>();
            public override Queue<T>? InitializeStore() => null;
            public override void ResetStore(ref Queue<T>? store) => store = null;
        }
    }

    internal sealed class Extremum<T> : Extrema<(bool, T), T>
    {
        public static Extremum<T> First { get; } = new Extremum<T>(poppable: false);
        public static Extremum<T> Last { get; } = new Extremum<T>(poppable: true);

        private readonly bool _poppable;

        private Extremum(bool poppable)
        {
            _poppable = poppable;
        }

        public override void AddItem(ref (bool, T) store, int? limit, T item)
        {
            if (!_poppable && store is (true, _))
            {
                return;
            }

            store = (true, item);
        }

        public override IEnumerable<T> GetEnumerable((bool, T) store) =>
            store is (true, var element)
                ? Repeat(element, count: 1)
                : Empty<T>();

        public override (bool, T) InitializeStore() => default;
        public override void ResetStore(ref (bool, T) store) => store = default;
    }
}