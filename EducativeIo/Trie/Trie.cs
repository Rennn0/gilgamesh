namespace EducativeIo.Trie;

public partial class Trie
{
    private readonly TrieNode m_root;

    public Trie(TrieNode root)
    {
        m_root = root;
    }

    public Trie()
    {
        m_root = new TrieNode();
    }

    public int GetIndex(char t)
    {
        return t - 'a';
    }

    public void InsertNode(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        key = key.ToLower();
        TrieNode? pCrawl = m_root;
        for (int level = 0; level < key.Length; level++)
        {
            int index = GetIndex(key[level]);
            pCrawl![index] ??= new TrieNode();
            pCrawl = pCrawl[index];
        }

        pCrawl!.MarkAsLeaf();
    }

    public bool SearchNode(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        key = key.ToLower();
        TrieNode? pCrawl = m_root;

        for (int level = 0; level < key.Length; level++)
        {
            int index = GetIndex(key[level]);

            if (pCrawl![index] == null)
                return false;
            pCrawl = pCrawl[index];
        }

        return (pCrawl != null) && (pCrawl!.IsEndWord());
    }

    public bool DeleteNode(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        key = key.ToLower();
        return DeleteNode(m_root, key, 0);
    }

    private bool DeleteNode(TrieNode? node, string key, int level)
    {
        bool deletedSelf = false;

        if (node == null)
            return deletedSelf;

        // base case: if we have reached the end of the key
        if (level == key.Length)
        {
            //If there are no nodes ahead of this node in this path
            //Then we can delete this node
            if (HasNoChildren(node))
            {
                node = null;
            }
            //If there are nodes ahead of currentNode in this path
            //Then we cannot delete currentNode. We simply unmark this as leaf
            else
            {
                node.UnMarkAsLeaf();
            }

            deletedSelf = true;
        }
        else
        {
            TrieNode? child = node[GetIndex(key[level])];
            bool childDeleted = DeleteNode(child, key, level + 1);
            if (childDeleted)
            {
                //Making children pointer also null: since child is deleted
                node[GetIndex(key[level])] = null;
                //If currentNode is leaf node that means currntNode is part of another key
                //and hence we can not delete this node, and it's parent path nodes
                //If childNode is deleted but if currentNode has more children than currentNode must be part of another key.
                //So, we cannot delete currenNode
                if (node.IsEndWord() || !HasNoChildren(node))
                {
                    deletedSelf = false;
                }
                else
                {
                    deletedSelf = true;
                    node = null;
                }
            }
            else
            {
                deletedSelf = false;
            }
        }

        return deletedSelf;
    }

    private bool HasNoChildren(TrieNode node)
    {
        for (int i = 0; i < TrieNode.Size; i++)
        {
            if (node[i] != null)
                return false;
        }

        return true;
    }
}
