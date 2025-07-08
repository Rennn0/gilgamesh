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
    }
}