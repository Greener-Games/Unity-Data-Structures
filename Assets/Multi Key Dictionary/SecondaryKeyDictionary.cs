using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GG.DataStructures
{
    [Serializable]
    public class SecondaryKeyDictionary<T1, T2, TV>
    {
        public Dictionary<T1, TV> primaryDictionary = new Dictionary<T1, TV>();
        public Dictionary<T2, T1> secondaryKeyLink = new Dictionary<T2, T1>();

        public SecondaryKeyDictionary()
        {
        }

        public SecondaryKeyDictionary(SecondaryKeyDictionary<T1, T2, TV> dictionary)
        {
            primaryDictionary = new Dictionary<T1, TV>(dictionary.primaryDictionary);
            secondaryKeyLink = new Dictionary<T2, T1>(dictionary.secondaryKeyLink);
        }
        

        public List<T1> PrimaryKeys => primaryDictionary.Keys.ToList();
        public List<T2> SecondaryKeys => secondaryKeyLink.Keys.ToList();
        public List<TV> Values => primaryDictionary.Values.ToList();

        public T2 GetSecondaryKey(T1 key) => secondaryKeyLink.FirstOrDefault(x => x.Value.Equals(key)).Key;

        public int Count => Values.Count();

        
        public TV this[T1 primary] => GetValueFromPrimary(primary);

        public TV this[T2 secondary] => GetValueFromSecondary(secondary);
        
        /// <summary>
        ///     This is used to attempt to grab from primary first, if not match will attempt to find from secondary keys
        ///     in the case of both type being the same, pass them both in through this to attempt to grab either
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public TV this[T1 primary, T2 secondary] => GetValueFromEither(primary, secondary);
        

        
        /// <summary>
        ///     Gets the value based on the primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public TV GetValueFromPrimary(T1 primaryKey)
        {
            if (primaryDictionary.ContainsKey(primaryKey))
            {
                return primaryDictionary[primaryKey];
            }

            throw new KeyNotFoundException("primary key not found");
        }

        /// <summary>
        ///     Gets the value from the secondary key
        /// </summary>
        /// <param name="secondaryKey"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public TV GetValueFromSecondary(T2 secondaryKey)
        {
            if (secondaryKeyLink.ContainsKey(secondaryKey))
            {
                T1 primarykey = secondaryKeyLink[secondaryKey];
                return GetValueFromPrimary(primarykey);
            }

            throw new KeyNotFoundException("Secondary not found");
        }

        /// <summary>
        ///     Try get value from either based searching with primary key first
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public TV GetValueFromEither(T1 primary, T2 secondary)
        {
            if (primaryDictionary.ContainsKey(primary))
            {
                return GetValueFromPrimary(primary);
            }

            if (secondaryKeyLink.ContainsKey(secondary))
            {
                return GetValueFromSecondary(secondary);
            }

            throw new KeyNotFoundException("Key not found");
        }
        
        /// <summary>
        ///     Gets the value from the secondary key
        /// </summary>
        /// <param name="secondaryKey"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public T1 GetPrimaryFromSecondary(T2 secondaryKey)
        {
            if (secondaryKeyLink.ContainsKey(secondaryKey))
            {
                return secondaryKeyLink[secondaryKey];
            }

            throw new KeyNotFoundException("Secondary not found");
        }

        /// <summary>
        ///     Add an entry with only a primary key, can be linked at a later time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Add(T1 key, TV value)
        {
            if (primaryDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Primary key already exist");
            }

            primaryDictionary.Add(key, value);
        }

        /// <summary>
        ///     add an entry with a primary and secondary key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="secondaryKey"></param>
        public void Add(T1 key, TV value, T2 secondaryKey)
        {
            Add(key, value);

            LinkSecondaryKey(key, secondaryKey);
        }

        public void RemoveUsingPrimary(T1 key)
        {
            if (!primaryDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("No item with primary key");
            }

            T2 secondary;
            if (GetSecondaryKey(key, out secondary))
            {
                Remove(secondary);
            }
        }
        
        public void RemoveUsingSecondary(T2 key)
        {
            if (!secondaryKeyLink.ContainsKey(key))
            {
                throw new InvalidOperationException("No item with primary key");
            }

            primaryDictionary.Remove(secondaryKeyLink[key]);
            secondaryKeyLink.Remove(key);
        }

        /// <summary>
        ///     Remove an entry from the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Remove(T1 key)
        {
            RemoveUsingPrimary(key);
        }

        /// <summary>
        ///     Remove an entry from the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Remove(T2 key)
        {
RemoveUsingSecondary(key);
        }

        /// <summary>
        ///     Link a secondary key to a primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="secondaryKey"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void LinkSecondaryKey(T1 primaryKey, T2 secondaryKey)
        {
            if (primaryDictionary.ContainsKey(primaryKey))
            {
                if (secondaryKeyLink.ContainsKey(secondaryKey))
                {
                    //adding a key to an already existing secondary key, this will override the previous key
                    Debug.LogWarning("Secondary key already exists, replacing orginal key with new one");
                    secondaryKeyLink[secondaryKey] = primaryKey;
                }
                else
                {
                    secondaryKeyLink.Add(secondaryKey, primaryKey);
                }
            }
            else
            {
                throw new InvalidOperationException("Secondary key already exist");
            }
        }

        /// <summary>
        /// Converts the key from the primary to the key for secondary
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        bool GetSecondaryKey(T1 primaryKey, out T2 returnValue)
        {
            try
            {
                returnValue = secondaryKeyLink.FirstOrDefault(x => x.Value.Equals(primaryKey)).Key;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Unable to get secondary value return default: {e}");
                returnValue = default(T2);
                return false;
            }
        }

        public bool ContainsPrimaryKey(T1 primaryKey)
        {
            return primaryDictionary.ContainsKey(primaryKey);
        }

        public bool ContainsSecondaryKey(T2 secondaryKey)
        {
            if (secondaryKeyLink.ContainsKey(secondaryKey))
            {
                T1 primaryKey = secondaryKeyLink[secondaryKey];

                return primaryDictionary.ContainsKey(primaryKey);
            }

            return false;
        }

        public bool ContainsKey(T1 key)
        {
            return ContainsPrimaryKey(key);
        }

        public bool ContainsKey(T2 secondaryKey)
        {
            return ContainsSecondaryKey(secondaryKey);
        }

        public void Clear()
        {
            primaryDictionary.Clear();
            secondaryKeyLink.Clear();
        }
    }
}