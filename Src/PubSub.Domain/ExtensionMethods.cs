using System.Collections.Concurrent;

namespace PubSub.Domain
{
    internal static class ExtensionMethods
    {
        internal static void AddOrUpdate<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
        {
            dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }
    }
}
