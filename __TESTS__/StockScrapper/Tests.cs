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
    }
}