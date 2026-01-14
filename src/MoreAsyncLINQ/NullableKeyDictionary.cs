using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreAsyncLINQ
{
    internal sealed class NullableKeyDictionary<TKey, TValue>
    {
        private readonly Dictionary<ValueTuple<TKey>, TValue> _dictionary;

        public NullableKeyDictionary(IEqualityComparer<TKey> comparer)
        {
            var keyComparer =
                ReferenceEquals(comparer, EqualityComparer<TKey>.Default)
                    ? null
                    : new ValueTupleItemComparer<TKey>(comparer);
            
            _dictionary = new Dictionary<ValueTuple<TKey>, TValue>(keyComparer);
        }

        public TValue this[TKey key]
        {
            get => _dictionary[ValueTuple.Create(key)];
            set => _dictionary[ValueTuple.Create(key)] = value;
        }
        
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
            _dictionary.TryGetValue(ValueTuple.Create(key), out value);

        private sealed class ValueTupleItemComparer<T>(IEqualityComparer<T> comparer) : IEqualityComparer<ValueTuple<T>>
        {
            public bool Equals(ValueTuple<T> first, ValueTuple<T> second) =>
                comparer.Equals(first.Item1, second.Item1);

            public int GetHashCode(ValueTuple<T> valueTuple) =>
                valueTuple.Item1 is { } value
                    ? comparer.GetHashCode(value)
                    : 0;
        }
    }
}