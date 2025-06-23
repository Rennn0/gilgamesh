namespace EducativeIo.Heap;

public class MinHeap<T> where T : IComparable<T>
{
    private List<T> h;

    public void Poll()
    {
        if (Size() == 1)
        {
            h.RemoveAt(0);
        }
        else
        {
            (h[0], h[Size() - 1]) = (h[Size() - 1], h[0]);
            h.RemoveAt(Size() - 1);
            MinHeapify(0);
        }
    }

    void PerlocateUp(int i)
    {
        if (i <= 0) return;
        else if (h[Parent(i)].CompareTo(h[i]) > 0)
        {
            (h[i], h[Parent(i)]) = (h[Parent(i)], h[i]);
            PerlocateUp(Parent(i));
        }
    }
    void MinHeapify(int i)
    {
        int min = i;
        int lChild = LChild(i);
        int rChild = RChild(i);
        if (lChild < Size() && h[lChild].CompareTo(h[min]) < 0)
        {
            min = lChild;
        }
        if (rChild < Size() && h[rChild].CompareTo(h[min]) < 0)
        {
            min = rChild;
        }
        if (min != i)
        {
            (h[min], h[i]) = (h[i], h[min]);
            MinHeapify(min);
        }
    }

    public MinHeap()
    {
        h = new List<T>();
    }
    public int Size() => h.Count;
    public T GetMin() => Size() <= 0 ? (T) Convert.ChangeType(-1, typeof(T)) : h[0];
    public void Insert(T key)
    {
        h.Add(key);
        PerlocateUp(Size() - 1);
    }
    public void RemoveMin()
    {
        if (Size() == 1)
        {
            h.RemoveAt(h.Count - 1);
        }
        else if (Size() > 1)
        {
            (h[0], h[Size() - 1]) = (h[Size() - 1], h[0]);
            h.RemoveAt(Size() - 1);

            MinHeapify(0);
        }
    }
    public void BuildHeap(T[] arr)
    {
        h.AddRange(arr);
        for (int i = (Size() - 1) / 2; i >= 0; i--)
        {
            MinHeapify(i);
        }
    }
    public int Parent(int i)
    {
        return (i - 1) / 2;
    }
    public int LChild(int i)
    {
        return i * 2 + 1;
    }
    public int RChild(int i)
    {
        return i * 2 + 2;
    }
    public string AsString() => string.Join(',', h);
}