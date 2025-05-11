using EducativeIo.Heap;

namespace __TESTS__;
[TestClass]
public class HeapTests
{
    [TestMethod]
    public void Build()
    {
        MaxHeap<int> heap = new();
        heap.BuildHeap([4, 0, 5, 1, 2, 3, 15]);

        MinHeap<int> minHeap = new();
        minHeap.BuildHeap([4, 0, 5, 1, 2, 3, 15]);
        minHeap.Insert(-5);
    }
}