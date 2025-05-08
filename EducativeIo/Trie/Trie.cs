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

    public int TotalWords() => TotalWords(m_root);

    private int TotalWords(TrieNode? node)
    {
        if (node == null)
            return 0;

        int totalWords = 0;
        if (node.IsEndWord())
            totalWords++;

        for (int i = 0; i < TrieNode.Size; i++)
        {
            totalWords += TotalWords(node[i]);
        }

        return totalWords;
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

        if (level == key.Length) // base case
        {
            if (HasNoChildren(node))
            {
                deletedSelf = true;
                node = null;
            }
            else
            {
                deletedSelf = false;
                node.UnMarkAsLeaf();
            }
        }
        else
        {
            TrieNode? child = node?[GetIndex(key[level])];
            bool childDeleted = DeleteNode(child, key, level + 1);

            if (childDeleted)
            {
                node![GetIndex(key[level])] = null;

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
