using EducativeIo.Heap;

namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        private readonly MaxHeap<int> _max = new MaxHeap<int>();
        private readonly MinHeap<int> _min = new MinHeap<int>();

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
        }

        public float MedianAge()
        {
            if (_max.Size() == _min.Size())
            {
                return (_max.GetMax() + _min.GetMin()) / 2f;
            }
            return _max.GetMax();
        }
    }
}