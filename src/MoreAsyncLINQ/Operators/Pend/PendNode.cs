using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ.Pend
{
    internal abstract class PendNode<T> : IAsyncEnumerable<T>
    {
        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var appendIndex = 0;

            var append1 = default(T);
            var append2 = default(T);
            var append3 = default(T);
            var append4 = default(T);
            T[]? appends = null;

            var current = this;
            for (; current is Item item; current = item.Next)
            {
                if (item.IsPrepend)
                {
                    yield return item.Value;

                    continue;
                }

                if (appends is null)
                {
                    if (appendIndex == 0 && item.AppendCount > 4)
                    {
                        appends = new T[item.AppendCount];
                    }
                    else
                    {
                        switch (appendIndex)
                        {
                            case 0:
                                append1 = item.Value;
                                break;
                            case 1:
                                append2 = item.Value;
                                break;
                            case 2:
                                append3 = item.Value;
                                break;
                            case 3:
                                append4 = item.Value;
                                break;
                            default: throw new IndexOutOfRangeException();
                        }

                        appendIndex++;
                        continue;
                    }
                }

                appends[appendIndex] = item.Value;
                appendIndex++;
            }

            var source = (Source) current;
            await foreach (var element in source.Value.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;
            }

            if (appends is null)
            {
                if (appendIndex == 4)
                {
                    yield return append4!;

                    appendIndex--;
                }

                if (appendIndex == 3)
                {
                    yield return append3!;

                    appendIndex--;
                }

                if (appendIndex == 2)
                {
                    yield return append2!;

                    appendIndex--;
                }

                if (appendIndex == 1)
                {
                    yield return append1!;
                }

                yield break;
            }

            for (appendIndex--; appendIndex >= 0; appendIndex--)
            {
                yield return appends[appendIndex];
            }
        }

        public PendNode<T> Append(T item) => new Item(item, isPrepend: false, this);
        public PendNode<T> Prepend(T item) => new Item(item, isPrepend: true, this);
        public static PendNode<T> WithSource(IAsyncEnumerable<T> source) => new Source(source);

        private sealed class Item : PendNode<T>
        {
            public int AppendCount { get; }
            public bool IsPrepend { get; }
            public PendNode<T> Next { get; }
            public T Value { get; }

            public Item(T value, bool isPrepend, PendNode<T> next)
            {
                AppendCount =
                    next is Item nextItem
                        ? nextItem.AppendCount + (isPrepend ? 0 : 1)
                        : 1;
                IsPrepend = isPrepend;
                Next = next;
                Value = value;
            }
        }

        private sealed class Source : PendNode<T>
        {
            public IAsyncEnumerable<T> Value { get; }

            public Source(IAsyncEnumerable<T> value)
            {
                Value = value;
            }
        }
    }
}