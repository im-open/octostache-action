using System;
using System.Collections.Generic;

namespace OctostacheCmd
{
    public static class DictionaryExtensions
    {
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (var entry in dictionary)
            {
                action(entry);
            }
        }
    }
}
