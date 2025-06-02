using EducativeIo.Projects.Netflix;
using Newtonsoft.Json.Bson;

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

        [TestMethod]
        public void MaxStack()
        {
            Netflix.MinStack stack = new Netflix.MinStack();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Assert.AreEqual(3, stack.GetMax());

            stack.Pop();
            stack.Push(4);
            stack.Push(1);

            Assert.AreEqual(4, stack.GetMax());

            stack.Pop();
            stack.Pop();

            Assert.AreEqual(2, stack.GetMax());
        }

        [TestMethod]
        public void Session()
        {
            Netflix.MinStack stack = new Netflix.MinStack();
            Assert.IsTrue(stack.VerifySession(new int[] { 1, 2, 3, 4, 5 }, new int[] { 4, 5, 3, 2, 1 }));
            Assert.IsFalse(stack.VerifySession(new int[] { 1, 2, 3, 4, 5 }, new int[] { 4, 5, 3, 1, 2 }));
        }


        [TestMethod]
        public void Combinations()
        {
            Netflix.MovieCombinations comb = new Netflix.MovieCombinations();
            List<string> c1 = comb.Combinations(new string[] { "Action", "Horror" });
            Console.WriteLine(string.Join(" ", c1));
        }

        [TestMethod]
        public void Permutations()
        {
            Netflix.MovieCombinations combinations = new Netflix.MovieCombinations();
            string[] Input = new string[3] { "A", "B", "C" };
            var Output = combinations.Permutations(Input);
            System.Console.Write("Output 1: [");
            for (int i = 0; i < Output.Count; i++)
            {
                System.Console.Write("[");
                System.Console.Write(string.Join(", ", Output[i]));
                System.Console.Write("]");
            }
            System.Console.WriteLine("]");
        }

        [TestMethod]
        public void ContinueWatching()
        {
            Netflix.ContinueWatching cw = new Netflix.ContinueWatching();
            cw.Add("a");
            cw.Add("b");
            cw.Add("c");
            cw.Add("b");
            cw.Add("a");
            cw.Add("b");

            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("a", cw.GetMovie());
            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("c", cw.GetMovie());
            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("a", cw.GetMovie());
        }
    }
}