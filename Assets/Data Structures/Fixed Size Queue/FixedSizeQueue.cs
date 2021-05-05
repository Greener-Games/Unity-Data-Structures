using System.Collections.Concurrent;

namespace GG.DataStructures
{
    public class FixedSizeQueue<T> : ConcurrentQueue<T>
    {
        readonly int size;

        public FixedSizeQueue(int size)
        {
            this.size = size;
        }

        /// <summary>
        /// Add an item to the back of the queue, if the queue exceeds the maximum aloud items the oldest item is removed
        /// </summary>
        /// <param name="obj"></param>
        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            if (Count > size)
            {
                // Remove one of the oldest added items.
                TryDequeue(out T _);
            }
        }

        /// <summary>
        /// Removes the oldest item from the queue and returns it
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Dequeue(out T obj)
        {
            return TryDequeue(out obj);
        }

        /// <summary>
        /// Clear the queue
        /// </summary>
        public void Clear()
        {
            while (TryDequeue(out T _))
            {
                // do nothing
            }
        }
    }
}