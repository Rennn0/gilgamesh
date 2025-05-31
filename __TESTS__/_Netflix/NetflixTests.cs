using EducativeIo.Projects.Netflix;

namespace __TESTS__._Netflix
{
    [TestClass]
    public class NetflixTests
    {
        [TestMethod]
        public void MedianAge()
        {
            Netflix netflix = new Netflix();
            netflix.InsertAge(10);
            netflix.InsertAge(22);
            netflix.InsertAge(25);
            netflix.InsertAge(30);

            float median = netflix.MedianAge();
            Assert.AreEqual(23.5, median);

            netflix.InsertAge(35);
            median = netflix.MedianAge();
            Assert.AreEqual(25, median);
        }

        [TestMethod]
        public void LruCache()
        {
            Netflix.LruCache cache = new Netflix.LruCache(3);
            cache.Add(1);
            cache.Add(2);
            cache.Add(3);
            int[] recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, recentlyUsed);

            cache.Add(4);
            cache.Add(5);
            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, recentlyUsed);

            cache.Add(4);
            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 3, 5, 4 }, recentlyUsed);

            cache.Add(4);
            cache.Add(4);
            cache.Add(1);

            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 5, 4, 1 }, recentlyUsed);
        }

        [TestMethod]
        public void LfuCache()
        {
            Netflix.LfuCache cache = new Netflix.LfuCache(3);
            cache.Add(1);
            cache.Add(1);
            cache.Add(1);
            cache.Add(2);
            cache.Add(3);

            int[] nodes = cache.GetCache();
            string[] freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, nodes);
            CollectionAssert.AreEqual(new[] { "1:3", "2:1", "3:1" }, freq);

            cache.Add(3);
            cache.Add(3);
            cache.Add(4);

            nodes = cache.GetCache();
            freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 1, 3, 4 }, nodes);
            CollectionAssert.AreEqual(new[] { "1:3", "3:3", "4:1" }, freq);

            cache.Add(4);
            cache.Add(4);
            cache.Add(9);

            nodes = cache.GetCache();
            freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 3, 4, 9 }, nodes);
            CollectionAssert.AreEqual(new[] { "3:3", "4:3", "9:1" }, freq);
        }
    }
}