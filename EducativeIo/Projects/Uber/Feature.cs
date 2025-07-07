using System.Runtime.CompilerServices;
using System.Text;
using EducativeIo.Heap;
using RabbitMQ.Client;

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

        public double[] GetTotalCost(List<List<string>> map, double[] costs, List<string> drivers, string user)
        {
            Dictionary<string, Dictionary<string, double>> city = new Dictionary<string, Dictionary<string, double>>();
            for (int i = 0; i < map.Count; i++)
            {
                List<string> checkpoints = map[i];
                string source = checkpoints[0];
                string destionation = checkpoints[1];
                double pathCost = costs[i];
                if (!city.ContainsKey(source))
                {
                    city[source] = new Dictionary<string, double>();
                }

                if (!city.ContainsKey(destionation))
                {
                    city[destionation] = new Dictionary<string, double>();
                }
                city[source][destionation] = pathCost;
                city[destionation][source] = pathCost;
            }

            double[] results = new double[drivers.Count];

            for (int i = 0; i < drivers.Count; i++)
            {
                string driver = drivers[i];
                if (!city.ContainsKey(driver) || !city.ContainsKey(user))
                {
                    results[i] = -1d;
                }
                else
                {
                    HashSet<string> visited = new HashSet<string>();
                    results[i] = BacktrackCity(city, driver, user, 0, visited);
                }
            }

            return results;
        }

        private double BacktrackCity(Dictionary<string, Dictionary<string, double>> city, string driver, string user, double accumulatedSum, HashSet<string> visited)
        {
            visited.Add(driver);
            double ret = -1d;

            Dictionary<string, double> adjacents = city[driver];
            if (adjacents.TryGetValue(user, out double value))
            {
                ret = accumulatedSum + value;
            }
            else
            {
                foreach (KeyValuePair<string, double> kvp in adjacents)
                {
                    string nextNode = kvp.Key;
                    if (visited.Contains(nextNode))
                    {
                        continue;
                    }
                    ret = BacktrackCity(city, nextNode, user, accumulatedSum + kvp.Value, visited);
                    if (ret != -1)
                    {
                        break;
                    }
                }
            }

            visited.Remove(driver);
            return ret;
        }

        public int KthHighestRank(int[] ranks, int k)
        {
            MinHeap<int> mh = new MinHeap<int>();
            for (int i = 0; i < k; i++)
            {
                mh.Insert(ranks[i]);
            }

            for (int i = k; i < ranks.Length; i++)
            {
                if (ranks[i] > mh.GetMin())
                {
                    mh.Poll();
                    mh.Insert(ranks[i]);
                }
            }

            return mh.GetMin();
        }

        public int OptimalPath(int[][] grid)
        {
            int row = grid.Length;
            int col = grid[0].Length;

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (r > 0 && c > 0)
                    {
                        grid[r][c] = Math.Min(grid[r][c] + grid[r - 1][c], grid[r][c] + grid[r][c - 1]);
                    }
                    else if (r > 0 || c > 0)
                    {
                        if (c > 0)
                        {
                            grid[r][c] += grid[r][c - 1];
                        }
                        else
                        {
                            grid[r][c] += grid[r - 1][c];
                        }
                    }
                }
            }

            return grid[row - 1][col - 1];
        }
    }
}