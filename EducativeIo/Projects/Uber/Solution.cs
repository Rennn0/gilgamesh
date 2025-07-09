using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;
using EducativeIo.Heap;

namespace EducativeIo.Projects.Uber
{
    public class Solution
    {
        private struct Point : IComparable<Point>
        {
            public int X
            {
                get;
                set;
            }

            public int Y
            {
                get;
                set;
            }
            public int Distance => X * X + Y * Y;
            public int CompareTo(Point other) => Distance - other.Distance;

        }

        // 4  5  6  1  2  2
        //
        public int[][] KClosest(int[][] points, int k)
        {
            int[][] result = new int[k][];

            MaxHeap<Point> ph = new MaxHeap<Point>();
            for (int i = 0; i < k; i++)
            {
                ph.Insert(new Point { X = points[i][0], Y = points[i][1] });
            }

            for (int i = k; i < points.Length; i++)
            {
                Point p = new Point { X = points[i][0], Y = points[i][1] };
                if (ph.GetMax().Distance > p.Distance)
                {
                    ph.Poll();
                    ph.Insert(p);
                }
            }

            for (int i = 0; i < k; i++)
            {
                Point p = ph.GetMax();
                result[i] = [p.X, p.Y];
                ph.Poll();
            }

            return result;
        }

        public int TrapWater(int[] elevationMap)
        {
            int water = 0;
            int size = elevationMap.Length;
            int[] leftMax = new int[size];
            int[] rightMax = new int[size];
            leftMax[0] = elevationMap[0];
            rightMax[size - 1] = elevationMap[size - 1];

            for (int i = 1; i < size; i++)
            {
                leftMax[i] = Math.Max(leftMax[i - 1], elevationMap[i]);
            }

            for (int i = size - 2; i >= 0; i--)
            {
                rightMax[i] = Math.Max(rightMax[i + 1], elevationMap[i]);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                water += Math.Min(leftMax[i], rightMax[i]) - elevationMap[i];
            }

            return water;
        }

        public class WeightedProbability
        {
            private int[] _cumulativeProb;
            private int[] _original;
            private int _totalSum;
            private readonly Random _random;
            public WeightedProbability(int[] weights)
            {
                _original = weights;
                _random = new Random();
                _cumulativeProb = new int[weights.Length];
                _cumulativeProb[0] = weights[0];
                for (int i = 1; i < weights.Length; i++)
                {
                    _cumulativeProb[i] = _cumulativeProb[i - 1] + weights[i];
                }
                _totalSum = _cumulativeProb[_cumulativeProb.Length - 1];
            }

            public int PickIndex()
            {
                int target = _random.Next(1, _totalSum + 1);
                int left = 0;
                int right = _cumulativeProb.Length - 1;
                while (left < right)
                {
                    int mid = left + (right - left) / 2;
                    if (_cumulativeProb[mid] < target)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid;
                    }
                }

                return left;
            }
        }

        public int FindKthLargest(int[] arr, int k)
        {
            MinHeap<int> mh = new MinHeap<int>();
            for (int i = 0; i < k; i++)
            {
                mh.Insert(arr[i]);
            }

            for (int i = k; i < arr.Length; i++)
            {
                if (mh.GetMin() < arr[i])
                {
                    mh.Poll();
                    mh.Insert(arr[i]);
                }
            }

            return mh.GetMin();
        }

        public int MinPathSum(int[][] arr)
        {
            int rows = arr.Length;
            int columns = arr[0].Length;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (row > 0 && col > 0)
                    {
                        arr[row][col] = Math.Min(arr[row][col] + arr[row - 1][col], arr[row][col] + arr[row][col - 1]);
                    }
                    else if (row > 0 || col > 0)
                    {
                        if (row > 0)
                        {
                            arr[row][col] += arr[row - 1][col];
                        }
                        else
                        {
                            arr[row][col] += arr[row][col - 1];
                        }
                    }
                }
            }

            return arr[rows - 1][columns - 1];
        }
    }

    public class Array<T> : IDisposable, IEnumerable<T>
    {
        private readonly T[] _array;
        private readonly int _length;
        public Array(int length)
        {
            _length = length;
            _array = ArrayPool<T>.Shared.Rent(length);
        }

        public Array(IEnumerable<T> source)
        {
            T[] temp = source.ToArray();
            _length = temp.Length;
            _array = ArrayPool<T>.Shared.Rent(_length);
            Array.Copy(temp, _array, _length);
        }

        public T this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }
        public ReadOnlyCollection<T> Source => _array.AsReadOnly();
        public void Dispose()
        {
            Console.WriteLine("DISPOSED");
            GC.SuppressFinalize(this);
            ArrayPool<T>.Shared.Return(_array, true);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
            {
                yield return _array[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}