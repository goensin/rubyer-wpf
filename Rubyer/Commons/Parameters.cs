using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rubyer.Commons
{
    /// <summary>
    /// 传输参数
    /// </summary>
    public class Parameters : IParameters, IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        private readonly List<KeyValuePair<string, object>> entries = new List<KeyValuePair<string, object>>();

        /// <inheritdoc/>
        public object this[string key]
        {
            get
            {
                foreach (KeyValuePair<string, object> entry in entries)
                {
                    if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return entry.Value;
                    }
                }

                return null;
            }
        }

        /// <inheritdoc/>
        public int Count => entries.Count;

        /// <inheritdoc/>
        public IEnumerable<string> Keys => entries.Select((KeyValuePair<string, object> x) => x.Key);

        /// <inheritdoc/>
        public void Add(string key, object value) => entries.Add(new KeyValuePair<string, object>(key, value));

        /// <inheritdoc/>
        public bool ContainsKey(string key) => Keys.Contains(key);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => entries.GetEnumerator();

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            foreach (KeyValuePair<string, object> parameter in entries)
            {
                if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (parameter.Value is T value)
                    {
                        return value;
                    }

                    throw new InvalidCastException("Unable to convert the value. ");
                }
            }

            return default(T);
        }

        /// <inheritdoc/>
        public bool TryGetValue<T>(string key, out T value)
        {
            foreach (KeyValuePair<string, object> parameter in entries)
            {
                if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (parameter.Value is T result)
                    {
                        value = result;
                        return true;
                    }
                }
            }

            value = default(T);
            return false;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
