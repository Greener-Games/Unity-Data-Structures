# Unity Data Structures

A collection of custom data structures for Unity, designed to be easy to use and integrate into your projects. This package is provided by Greener Games.

## Data Structures

This package includes the following data structures:

-   `FixedSizeQueue<T>`: A queue with a maximum size that discards the oldest element when a new element is added to a full queue.
-   `OrderedDictionary<TKey, TValue>`: A dictionary that also maintains an ordered list of its key-value pairs.
-   `SecondaryKeyDictionary<T1, T2, TV>`: A dictionary that allows accessing values by either a primary or a secondary key.

---

### `FixedSizeQueue<T>`

A thread-safe queue with a fixed size. When the queue is full, adding a new item removes the oldest item. It inherits from `System.Collections.Concurrent.ConcurrentQueue<T>`.

**Example:**

```csharp
using GG.DataStructures;
using UnityEngine;

public class FixedSizeQueueExample : MonoBehaviour
{
    void Start()
    {
        var queue = new FixedSizeQueue<string>(3);

        queue.Enqueue("A");
        queue.Enqueue("B");
        queue.Enqueue("C");

        // Queue is now: [A, B, C]

        queue.Enqueue("D");

        // Queue is now: [B, C, D] (A was dequeued)

        if (queue.Dequeue(out string value))
        {
            Debug.Log(value); // Outputs: B
        }
    }
}
```

---

### `OrderedDictionary<TKey, TValue>`

A dictionary that maintains an ordered list of its key-value pairs. The ordering is defined by a `comparer` function passed in the constructor. This is useful when you need both fast lookups by key and an ordered collection.

**Example:**

```csharp
using GG.DataStructures;
using System.Collections.Generic;
using UnityEngine;

public class OrderedDictionaryExample : MonoBehaviour
{
    void Start()
    {
        // Order by value (int)
        var scores = new OrderedDictionary<string, int>(kvp => kvp.Value);

        scores.Add("Player2", 50);
        scores.Add("Player1", 100);
        scores.Add("Player3", 25);

        // The dictionary is automatically sorted after each addition.
        // OrderedValues will be: [25, 50, 100]
        foreach (var score in scores.OrderedValues)
        {
            Debug.Log(score);
        }

        // Access by key
        Debug.Log(scores["Player1"]); // Outputs: 100
    }
}
```

---

### `SecondaryKeyDictionary<T1, T2, TV>`

A dictionary that allows accessing values by either a primary or a secondary key. This is useful when you have two unique identifiers for an object.

**Example:**

```csharp
using GG.DataStructures;
using UnityEngine;

public class SecondaryKeyDictionaryExample : MonoBehaviour
{
    void Start()
    {
        var users = new SecondaryKeyDictionary<int, string, string>();

        // Add a user with a primary key (ID) and a secondary key (username)
        users.Add(1, "Alice", "alice_username");
        users.Add(2, "Bob", "bob_username");

        // Access by primary key
        Debug.Log(users[1]); // Outputs: Alice

        // Access by secondary key
        Debug.Log(users["bob_username"]); // Outputs: Bob
    }
}
```

## Installation

To install this package, follow these steps:

1.  Open the Unity Package Manager (`Window > Package Manager`).
2.  Click the `+` button in the top-left corner.
3.  Select "Add package from git URL...".
4.  Enter the following URL: `https://github.com/Greener-Games/Data-Structures.git`
5.  Click "Add".
