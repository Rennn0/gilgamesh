using System.Runtime.CompilerServices;
using EducativeIo.Heap;

namespace EducativeIo.Projects.Uber
{
    public class Feature
    {
        public class Location(int x, int y) : IComparable<Location>
        {
            public int GetDistance() => x * x + y * y;
            public int CompareTo(Location? other) => GetDistance().CompareTo(other?.GetDistance());
        }

        public List<Location> FindClosestDrivers(Location[] locations, int k)
        {
            MaxHeap<Location> locationHeap = new MaxHeap<Location>();
            for (int i = 0; i < k; i++)
            {
                locationHeap.Insert(locations[i]);
            }

            for (int i = k; i < locations.Length; i++)
            {
                if (locations[i].GetDistance() < locationHeap.GetMax().GetDistance())
                {
                    locationHeap.Poll();
                    locationHeap.Insert(locations[i]);
                }
            }

            return locationHeap.AsList();
        }

        public int PathCost(int[] elevationMap)
        {
            int cost = 0;
            int size = elevationMap.Length;
            Span<int> leftMax = stackalloc int[size];
            Span<int> rightMax = stackalloc int[size];
            leftMax[0] = elevationMap[0];
            rightMax[size - 1] = elevationMap[size - 1];

            for (int i = 1; i < size; i++)
            {
                leftMax[i] = Math.Max(elevationMap[i], leftMax[i - 1]);
            }

            for (int i = size - 2; i >= 0; i--)
            {
                rightMax[i] = Math.Max(elevationMap[i], rightMax[i + 1]);
            }

            for (int i = 0; i < size; i++)
            {
                cost += Math.Min(leftMax[i], rightMax[i]) - elevationMap[i];
            }

            return cost;
        }
    }
}