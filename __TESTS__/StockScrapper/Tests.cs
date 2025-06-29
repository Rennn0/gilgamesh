using EducativeIo.Projects.StockScrapper;

namespace __TESTS__.StockScrapper
{
    [TestClass]
    public class StockScrapperTests
    {
        [TestMethod]
        public void Traverse()
        {
            Feature feature = new Feature();
            Feature.TreeNode treeNode = new Feature.TreeNode("body");
            treeNode.Children.Add(new Feature.TreeNode("<nav"));
            treeNode.Children.Add(new Feature.TreeNode("<h1"));
            treeNode.Children.Add(new Feature.TreeNode("h1>"));
            treeNode.Children.Add(new Feature.TreeNode("nav>"));
            treeNode.Children[0].Children.Add(new Feature.TreeNode("li1"));
            treeNode.Children[0].Children.Add(new Feature.TreeNode("li2"));
            treeNode.Children[1].Children.Add(new Feature.TreeNode("p1"));
            treeNode.Children[1].Children.Add(new Feature.TreeNode("p2"));
            treeNode.Children[1].Children[1].Children.Add(new Feature.TreeNode("p2"));
            treeNode.Children[1].Children[1].Children.Add(new Feature.TreeNode("p3"));
            List<List<string>> res = feature.Traverse(treeNode);
        }

        [TestMethod]
        public void LowestCommonAncestor()
        {
            Feature feature = new Feature();
            Feature.TreeNode root = 1;
            root.Children.Add(22);
            root.Children.Add(34);
            root.Children[0].Children.Add(31);
            root.Children[0].Children.Add(10);
            root.Children[1].Children.Add(45);
            root.Children[1].Children.Add(122);
            root.Children[0].Children[0].Children.Add(77);
            root.Children[0].Children[1].Children.Add(23);
            root.Children[0].Children[1].Children.Add(43);
            root.Children[1].Children[1].Children.Add(9);
            root.Children[0].Children[1].Children[1].Children.Add(143);
            root.Children[0].Children[1].Children[1].Children.Add(64);

            Feature.TreeNode a = root.Children[0].Children[0].Children[0]; // 77
            Feature.TreeNode b = root.Children[0].Children[1].Children[1].Children[1]; // 64

            Assert.AreEqual(22, feature.LowestCommonAncestor(root, a, b));
        }
    }

}