using EducativeIo.BoundedBuffer;
using EducativeIo.Heap;

namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        private readonly MaxHeap<int> _max = new MaxHeap<int>();
        private readonly MinHeap<int> _min = new MinHeap<int>();
        private readonly List<int> _justList = new List<int>();
        public void InsertAge(int age)
        {
            if (_max.Size() == 1 || _max.GetMax() >= age)
            {
                _max.Insert(age);
            }
            else
            {
                _min.Insert(age);
            }

            if (_max.Size() > _min.Size() + 1)
            {
                _min.Insert(_max.GetMax());
                _max.RemoveMax();
            }
            else if (_max.Size() < _min.Size())
            {
                _max.Insert(_min.GetMin());
                _min.RemoveMin();
            }

            // _justList.Add(age);
        }

        public float MedianAge()
        {
            // MultiThreadedMergeSort mtms = new MultiThreadedMergeSort();
            // int[] arr = _justList.ToArray();
            // mtms.Sort(0, _justList.Count - 1, arr);

            // if (arr.Length % 2 == 0)
            // {
            //     return (arr[arr.Length / 2] + arr[arr.Length / 2 - 1]) / 2f;
            // }
            // else
            // {
            //     return arr[arr.Length / 2];
            // }

            if (_max.Size() == _min.Size())
            {
                return (_max.GetMax() + _min.GetMin()) / 2f;
            }
            return _max.GetMax();
        }
    }
}