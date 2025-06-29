namespace EducativeIo.Projects.StockScrapper
{
    public class Feature
    {
        public class TreeNode
        {
            public string Data;
            public List<TreeNode> Children;
            public TreeNode(string data)
            {
                Data = data;
                Children = new List<TreeNode>();
            }
        }

        public List<List<string>> Traverse(TreeNode root)
        {
            List<List<string>> result = new List<List<string>>();
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                int lvlSize = q.Count;
                List<string> thisLvl = new List<string>(lvlSize);
                for (int i = 0; i < lvlSize; i++)
                {
                    TreeNode node = q.Dequeue();
                    thisLvl.Add(node.Data);
                    node.Children.ForEach(n => q.Enqueue(n));
                }
                result.Add(thisLvl);
            }

            return result;
        }
    }
}