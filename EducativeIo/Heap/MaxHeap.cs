namespace EducativeIo.Heap;
public class MaxHeap<T> where T : IComparable<T>
{
    private List<T> h;
    void PerlocateUp(int i)
    {
        if (i <= 0) return;
        else if (h[Parent(i)].CompareTo(h[i]) < 0)
        {
            (h[i], h[Parent(i)]) = (h[Parent(i)], h[i]);
            PerlocateUp(Parent(i));
        }
    }
    void MaxHeapify(int i)
    {
        int max = i;
        int lChild = LChild(i);
        int rChild = RChild(i);
        if (lChild < Size() && h[lChild].CompareTo(h[max]) > 0)
        {
            max = lChild;
        }
        if (rChild < Size() && h[rChild].CompareTo(h[max]) > 0)
        {
            max = rChild;
        }
        if (max != i)
        {
            (h[max], h[i]) = (h[i], h[max]);
            MaxHeapify(max);
        }
    }

    public MaxHeap()
    {
        h = new List<T>();
    }
    public int Size()
    {
        return h.Count;
    }
    public T GetMax()
    {
        return Size() <= 0 ? (T)Convert.ChangeType(-1, typeof(T)) : h[0];
    }
    public void Insert(T key)
    {
        h.Add(key);
        PerlocateUp(Size() - 1);
    }
    public void RemoveMax()
    {
        if (Size() == 1)
        {
            h.RemoveAt(h.Count - 1);
        }
        else if (Size() > 1)
        {
            (h[0], h[Size() - 1]) = (h[Size() - 1], h[0]);
            h.RemoveAt(Size() - 1);

            MaxHeapify(0);
        }
    }
    public void BuildHeap(T[] arr)
    {
        h.AddRange(arr);
        for (int i = (Size() - 1) / 2; i >= 0; i--)
        {
            MaxHeapify(i);
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
}