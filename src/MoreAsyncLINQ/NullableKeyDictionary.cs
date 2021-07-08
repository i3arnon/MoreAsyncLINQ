using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreAsyncLINQ
{
    internal sealed class NullableKeyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private (bool exists, TValue value) _nullKey;

        public NullableKeyDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
            _nullKey = default;
        }

        public TValue this[TKey key]
        {
            set
            {
                if (key is null)
                {
                    _nullKey = (true, value);
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (key is not null)
            {
                return _dictionary.TryGetValue(key, out value);
            }

            if (_nullKey.exists)
            {
                value = _nullKey.value;
                return true;
            }

            value = default;
            return false;
        }
    }
}