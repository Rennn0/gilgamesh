using EducativeIo.Projects.Uber;

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
    }
}