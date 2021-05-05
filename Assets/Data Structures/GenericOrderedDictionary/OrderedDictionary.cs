using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace GG.DataStructures
{
	/// <summary>
	///     Represents a generic collection of key/value pairs that are ordered independently of the key and value.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
	public class OrderedDictionary<TKey, TValue>
    {
	    Dictionary<TKey, TValue> dictionary;
        List<KeyValuePair<TKey, TValue>> list;
        readonly Func<KeyValuePair<TKey, TValue>, object> comparer;
        
        public int Count => List.Count;
        public List<TKey> Keys => dictionary.Keys.ToList();
        public List<TValue> OrderedValues => List.Select(t => t.Value).ToList();

        public OrderedDictionary(Func<KeyValuePair<TKey, TValue>, object>  comparer = null)
        {
	        this.comparer = comparer;
        }
	    
	    Dictionary<TKey, TValue> Dictionary => dictionary ?? (dictionary = new Dictionary<TKey, TValue>());
	    
	    List<KeyValuePair<TKey, TValue>> List => list ?? (list = new List<KeyValuePair<TKey, TValue>>());

	    public int Add(TKey key, TValue value)
	    {
		    Dictionary.Add(key, value);
		    List.Add(new KeyValuePair<TKey, TValue>(key, value));
		    Sort();
		    return Count - 1;
	    }

	    public void Insert(int index, TKey key, TValue value)
        {
            if (index > Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Dictionary.Add(key, value);
            List.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
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
			    if (Dictionary.Remove(key))
			    {
				    List.RemoveAt(index);
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

            TKey key = List[index].Key;

            List.RemoveAt(index);
            Dictionary.Remove(key);
        }

	    void Sort()
	    {
		    list = list.OrderBy(comparer).ToList();

	    }
	    
	    public TValue this[int index]
        {
            get => List[index].Value;

            set
            {
                if (index >= Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", "'index' must be non-negative and less than the size of the collection");
                }

                TKey key = List[index].Key;

                List[index] = new KeyValuePair<TKey, TValue>(key, value);
                Dictionary[key] = value;
                Sort();
            }
        }
	    
	    public TValue this[TKey key]
	    {
		    get => Dictionary[key];
		    set
		    {
			    if (Dictionary.ContainsKey(key))
			    {
				    Dictionary[key] = value;
				    List[IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
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
            Dictionary.Clear();
            List.Clear();
        }
	    
	    public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
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