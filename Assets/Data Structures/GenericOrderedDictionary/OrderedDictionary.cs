#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace GG.DataStructures
{
	/// <summary>
	///     Represents a generic collection of key/value pairs that are ordered independently of the key and value.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
	[Serializable]
    public class OrderedDictionary<TKey, TValue>
    {
	    /// <summary>
	    ///     unsorted dictionary of items
	    /// </summary>
	    [SerializeField]
        Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

	    /// <summary>
	    ///     sored list of items via comparator
	    /// </summary>
	    [SerializeField]
        List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();

        readonly Func<KeyValuePair<TKey, TValue>, object>[] comparer;

        public int Count => list.Count;

        public List<TKey> Keys => dictionary.Keys.ToList();
        public List<TValue> OrderedValues => list.Select(t => t.Value).ToList();

        public OrderedDictionary()
        {
        }

        public OrderedDictionary(Func<KeyValuePair<TKey, TValue>, object> comparer)
        {
            this.comparer = new[] {comparer};
        }

        public OrderedDictionary(params Func<KeyValuePair<TKey, TValue>, object>[] predicate)
        {
            comparer = predicate;
        }

        public int Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            list.Add(new KeyValuePair<TKey, TValue>(key, value));
            Sort();
            return Count - 1;
        }

        public void Insert(int index, TKey key, TValue value)
        {
            if (index > Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            dictionary.Add(key, value);
            list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
            Sort();
        }

        public bool Remove(TKey key)
        {
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            int index = IndexOfKey(key);
            if (index >= 0)
            {
                if (dictionary.Remove(key))
                {
                    list.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "'index' must be non-negative and less than the size of the collection");
            }

            TKey key = list[index].Key;

            list.RemoveAt(index);
            dictionary.Remove(key);
        }

        public KeyValuePair<TKey, TValue> First(Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            return list.FirstOrDefault(predicate);
        }

        public KeyValuePair<TKey, TValue> FirstOrDefault(Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            return list.FirstOrDefault(predicate);
        }

        public TValue First(Func<TValue, bool> predicate)
        {
            return OrderedValues.First(predicate);
        }

        public TValue FirstOrDefault(Func<TValue, bool> predicate)
        {
            return OrderedValues.FirstOrDefault(predicate);
        }

        void Sort()
        {
            if (comparer.Length > 0)
            {
                IOrderedEnumerable<KeyValuePair<TKey, TValue>> x = list.OrderBy(comparer[0]);
                if (comparer.Length > 1)
                {
                    for (int i = 1; i < comparer.Length - 1; i++)
                    {
                        x = x.ThenBy(comparer[i]);
                    }

                    list = x.ToList();
                }
                else
                {
                    list = x.ToList();
                }
            }
        }

        /// <summary>
        ///     Runs a particular orderby/then by, does not save this as part of the ordered list
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<KeyValuePair<TKey, TValue>> Sort(params Func<KeyValuePair<TKey, TValue>, object>[] predicate)
        {
            if (predicate.Length >= 1)
            {
                IOrderedEnumerable<KeyValuePair<TKey, TValue>> x = list.OrderBy(predicate[0]);
                for (int i = 1; i < predicate.Length - 1; i++)
                {
                    x = x.ThenBy(predicate[i]);
                }

                return x.ToList();
            }

            return list;
        }

        /// <summary>
        ///     return value at index
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public TValue this[int index]
        {
            get => list[index].Value;

            set
            {
                if (index >= Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", "'index' must be non-negative and less than the size of the collection");
                }

                TKey key = list[index].Key;

                list[index] = new KeyValuePair<TKey, TValue>(key, value);
                dictionary[key] = value;
                Sort();
            }
        }

        /// <summary>
        ///     Return value for key
        /// </summary>
        /// <param name="key"></param>
        public TValue this[TKey key]
        {
            get => dictionary[key];
            set
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                    list[IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    Add(key, value);
                }

                Sort();
            }
        }

        public void Clear()
        {
            dictionary.Clear();
            list.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public int IndexOfKey(TKey key)
        {
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            int index = list.FindIndex(x => x.Key.Equals(key));
            return index;
        }
    }
}