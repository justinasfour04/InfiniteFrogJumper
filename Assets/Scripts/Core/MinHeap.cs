using System.Collections;
using UnityEngine;

namespace FrogJump.Core
{
  class MinHeapComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      if ((int)x < (int)y)
      {
        return -1;
      }
      else if ((int)x == (int)y)
      {
        return 0;
      }
      else
      {
        return 1;
      }
    }
  }
  class MinHeap : IEnumerable
  {
    private SortedList items;

    public object this[int i]
    {
      get { return items[i]; }
    }

    public int Count { get { return items.Count; } }

    public bool Full { get { return items.Count == items.Capacity; } }

    public bool IsEmpty { get { return items.Count == 0; } }

    public void Clear() => items.Clear();

    public bool Contains(int key) => items.ContainsKey(key);

    public object Peek() => items[0];

    public MinHeap()
    {
      items = new SortedList(new MinHeapComparer());
    }

    public MinHeap(int capacity)
    {
      items = new SortedList(new MinHeapComparer(), capacity);
    }

    public void Push(int priority, GameObject item)
    {
      items.Add(priority, item);
    }

    public GameObject Pop()
    {
      var item = items[0];
      items.Remove(0);
      return (GameObject)item;
    }

    public IEnumerator GetEnumerator()
    {
      return items.GetEnumerator();
    }
  }
}