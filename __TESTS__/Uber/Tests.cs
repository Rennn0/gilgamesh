using EducativeIo.Projects.Uber;
using Microsoft.AspNetCore.Routing.Constraints;

namespace __TESTS__.Uber
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Location()
        {
            Feature feature = new();
            Feature.Location a = new(-1, 1);
            Feature.Location b = new(1, 3);
            Feature.Location c = new(2, 2);
            Feature.Location d = new(1, -1);
            Feature.Location[] locations = [a, b, c, d];
            List<Feature.Location> top1 = feature.FindClosestDrivers(locations, 1);
            List<Feature.Location> top2 = feature.FindClosestDrivers(locations, 2);
            List<Feature.Location> top3 = feature.FindClosestDrivers(locations, 3);

            Assert.AreEqual(a, top1[0]);
            Assert.AreEqual(a, top2[0]);
            Assert.AreEqual(d, top2[1]);
            Assert.AreEqual(c, top3[0]);
            Assert.AreEqual(a, top3[1]);
            Assert.AreEqual(d, top3[2]);
        }

        [TestMethod]
        public void PathCost()
        {
            Feature feature = new();
            Assert.AreEqual(22, feature.PathCost([1, 2, 1, 3, 1, 2, 1, 4, 1, 0, 0, 2, 1, 4]));
            Assert.AreEqual(10, feature.PathCost([3, 2, 1, 0, 2, 1, 3, 2, 3, 3]));
        }

        [TestMethod]
        public void SelectPath()
        {
            Feature feature = new();
            List<List<string>> map = [
                ["a","d"],
                ["a","b"],
                ["a","u"],
                ["d","u"],
                ["b","u"],
                ["b","e"],
                ["e","u"]
            ];
            List<string> drivers = ["a", "b", "e", "d"];
            double[] costs = [3d, 4d, 17d, 40d, 10d, 1d, 3d];

            double[] cost = feature.GetTotalCost(map, costs, drivers, "u");
        }

        [TestMethod]
        public void KClosest()
        {
            int[][] res1 = new Solution().KClosest([[1, 3], [-2, 2]], 1);
            Assert.AreEqual(1, res1.Length);
            Assert.AreEqual(-2, res1[0][0]);
            Assert.AreEqual(2, res1[0][1]);
        }

        [TestMethod]
        public void TrapWater() => Assert.AreEqual(6, new Solution().TrapWater([0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]));

        [TestMethod]
        public void Frequencies()
        {
            Dictionary<int, int> frequencies = new Dictionary<int, int>();
            int[] distribution1 = new int[] { 1, 12, 3, 4, 15 };
            foreach (int d in distribution1)
            {
                frequencies[d] = 0;
            }
            Solution.WeightedProbability sol = new Solution.WeightedProbability(distribution1);
            for (int i = 0; i < 100; i++)
            {
                int index = sol.PickIndex();
                if (index >= 0 && index < distribution1.Length)
                {
                    int element = distribution1[index];
                    frequencies[element] += 1;
                }
            }

            Console.WriteLine(string.Join(',', frequencies.Keys));
            Console.WriteLine(string.Join(',', frequencies.Values));

            Console.WriteLine("BEFORE");
            using (Array<int> nums = new Array<int>([4, 3, 1, 9, 4]))
            {
                foreach (int num in nums)
                {
                    Console.WriteLine(num);
                }
            }
            Console.WriteLine("AFTER");

        }
    }
}
