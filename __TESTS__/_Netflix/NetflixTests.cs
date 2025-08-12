using EducativeIo.Projects.Facebook;
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
            Netflix.LruCache<int> cache = new Netflix.LruCache<int>(3);
            cache.Add("1", 1);
            cache.Add("2", 2);
            cache.Add("3", 3);

            int[] recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, recentlyUsed);

            cache.Add("otxi", 4);
            cache.Add("xuti", 5);
            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, recentlyUsed);

            cache.Add("otxi", 4);
            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 3, 5, 4 }, recentlyUsed);

            cache.Add("otxi", 4);
            cache.Add("otxi", 4);
            cache.Add("1", 1);

            recentlyUsed = cache.GetCache();
            CollectionAssert.AreEqual(new[] { 5, 4, 1 }, recentlyUsed);
        }

        [TestMethod]
        public void LfuCache()
        {
            Netflix.LfuCache<int> cache = new Netflix.LfuCache<int>(3);
            cache.Add("erti", 1);
            cache.Add("erti", 1);
            cache.Add("erti", 1);
            cache.Add("ori", 2);
            cache.Add("sami", 3);

            int[] nodes = cache.GetCache();
            string[] freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, nodes);
            CollectionAssert.AreEquivalent(new[] { "erti:3", "ori:1", "sami:1" }, freq);

            cache.Add("sami", 3);
            cache.Add("sami", 3);
            cache.Add("otxi", 4);

            nodes = cache.GetCache();
            freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 1, 3, 4 }, nodes);
            CollectionAssert.AreEquivalent(new[] { "erti:3", "sami:3", "otxi:1" }, freq);

            cache.Add("otxi", 4);
            cache.Add("otxi", 4);
            cache.Add("cxra", 9);

            nodes = cache.GetCache();
            freq = cache.GetFrequency();

            CollectionAssert.AreEqual(new[] { 3, 4, 9 }, nodes);
            CollectionAssert.AreEquivalent(new[] { "sami:3", "otxi:3", "cxra:1" }, freq);
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
            cw.Add("z");

            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("a", cw.GetMovie());
            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("z", cw.GetMovie());
            Assert.AreEqual("c", cw.GetMovie());
            Assert.AreEqual("b", cw.GetMovie());
            Assert.AreEqual("a", cw.GetMovie());
        }


        [TestMethod]
        public void GroupAnagrams()
        {
            Netflix.Solution solution = new Netflix.Solution();
            List<List<string>> matrix = solution.GroupAnagrams(["word", "sword", "drow", "rowd", "iced", "dice"]);
            matrix.ForEach(list => Console.WriteLine(string.Join(',', list)));

            CollectionAssert.AreEqual(new string[][] { ["word", "drow", "rowd"], ["sword"], ["iced", "dice"] }, matrix);
        }

        [TestMethod]
        public void Merge()
        {
            Netflix.Solution solution = new Netflix.Solution();
            LinkedList<int> link1 = new LinkedList<int>([4, 5, 1, 9]);
            LinkedList<int> link2 = new LinkedList<int>([14, 25, -1, 89]);
            LinkedList<int> link3 = new LinkedList<int>([4, 45,]);

            LinkedListNode<int>? sorted = solution.Merge([link1.First, link2.First, link3.First]);
            Console.WriteLine(string.Join(',', sorted?.List?.ToList() ?? []));

            CollectionAssert.AreEqual(new int[] { -1, 1, 4, 4, 5, 9, 14, 25, 45, 89 }, sorted?.List?.ToList());
        }

        [TestMethod]
        public void Median()
        {
            Netflix.Solution solution = new Netflix.Solution();
            solution.InsertNum(5);
            Assert.AreEqual(5, solution.FindMedian());
            solution.InsertNum(4);
            Assert.AreEqual(4.5, solution.FindMedian());
            solution.InsertNum(3);
            Assert.AreEqual(4, solution.FindMedian());
            solution.InsertNum(1);
            Assert.AreEqual(3.5, solution.FindMedian());
        }

        [TestMethod]
        public void MinMaxStack()
        {
            Netflix.Solution.MinMaxStack solution = new Netflix.Solution.MinMaxStack(3);
            solution.Push(5);
            solution.Push(1);
            solution.Push(10);
            Assert.AreEqual(1, solution.Min());
            Assert.AreEqual(10, solution.Max());

            solution.Pop();
            Assert.AreEqual(1, solution.Min());
            Assert.AreEqual(5, solution.Max());

            solution.Pop();
            Assert.AreEqual(5, solution.Min());
            Assert.AreEqual(5, solution.Max());

            solution.Push(2);
            Assert.AreEqual(2, solution.Min());
            Assert.AreEqual(5, solution.Max());

            solution.Pop();
            solution.Push(0);
            Assert.AreEqual(0, solution.Min());
            Assert.AreEqual(5, solution.Max());
        }

        [TestMethod]
        public void VerifySession()
        {
            Netflix.Solution session = new Netflix.Solution();
            Assert.IsTrue(session.VerifySession([1, 2, 3, 4, 5], [5, 4, 3, 2, 1]));
            Assert.IsTrue(session.VerifySession([1, 2, 3, 4, 5], [3, 2, 5, 4, 1]));
            Assert.IsFalse(session.VerifySession([1, 2, 3, 4, 5], [4, 5, 3, 1, 2]));
        }

        [TestMethod]
        public void MobileCombinations()
        {
            Netflix.Solution.Combinations solution = new Netflix.Solution.Combinations();
            string[] combinations = solution.LetterCombinatios("23");
            foreach (string combination in combinations)
            {
                Console.WriteLine(combination);
            }
        }

        [TestMethod]
        public void NumPermutations()
        {
            Netflix.Solution.Combinations solution = new Netflix.Solution.Combinations();
            List<List<int>> result = solution.Permutaions([1, 2, 3]);
            var expected = new List<List<int>>
            {
                new List<int> {1,2,3},
                new List<int> {1,3,2},
                new List<int> {2,1,3},
                new List<int> {2,3,1},
                new List<int> {3,1,2},
                new List<int> {3,2,1},
            };

            Assert.AreEqual(expected.Count, result.Count, "Permutation count mismatch");

            foreach (var perm in expected)
            {
                Assert.IsTrue(result.Any(r => r.SequenceEqual(perm)), $"Missing permutation: [{string.Join(",", perm)}]");
            }
        }

        [TestMethod]
        public void Freq()
        {
            Netflix.Solution.FreqStack solution = new Netflix.Solution.FreqStack();
            solution.Push(1);
            solution.Push(2);
            solution.Push(1);
            solution.Push(3);
            solution.Push(1);
            solution.Push(2);

            Assert.AreEqual(1, solution.Pop());
            Assert.AreEqual(2, solution.Pop());
            Assert.AreEqual(1, solution.Pop());
            Assert.AreEqual(3, solution.Pop());
            Assert.AreEqual(2, solution.Pop());
            Assert.AreEqual(1, solution.Pop());
        }

        [TestMethod]
        public void Subset()
        {
            Feature feature = new Feature();
            var res = feature.WordSubsets(["amazon", "apple", "facebook", "google", "leetcode"], ["e", "o"]);
        }

        [TestMethod]
        public void SearchInCircularList()
        {
            Feature feature = new Feature();
            int[] v1 = [6, 7, 1, 2, 3, 4, 5];
            int k1 = feature.SearchInCircularList(v1, 3); // 4
            int k2 = feature.SearchInCircularList(v1, 6); // 0

            int[] v2 = [4, 5, 6, 1, 2, 3];
            int k3 = feature.SearchInCircularList(v2, 3); // 5
            int k4 = feature.SearchInCircularList(v2, 6); // 2


            Assert.AreEqual(4, k1);
            Assert.AreEqual(0, k2);
            Assert.AreEqual(5, k3);
            Assert.AreEqual(2, k4);
        }

        [TestMethod]
        public void FlagWords()
        {
            Feature feature = new Feature();
            Assert.IsTrue(feature.IsFlagWord("moron", "morrrron"));
            Assert.IsTrue(feature.IsFlagWord("noob", "nnnnoob"));
            Assert.IsFalse(feature.IsFlagWord("nassar", "ass"));
        }

        [TestMethod]
        public void CombineMessages()
        {
            Feature feature = new Feature();
            List<List<string>> groups = feature.CombineMessages(["lmn", "mno", "azb", "bac", "yza", "bdfg"]);
            foreach (List<string> g in groups)
                Console.WriteLine("[{0}]", string.Join(", ", g));
        }
    }
}