using System.Runtime.Intrinsics.X86;
using System.Text;

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

    public bool IsFormationPossible(List<string> list, string word)
    {
        // while (word.Length > 0)
        // {
        //     bool found = false;
        //     foreach (string str in list)
        //     {
        //         if (word.StartsWith(str))
        //         {
        //             found = true;
        //             word = word.Substring(str.Length);
        //             break;
        //         }
        //     }

        //     if (!found)
        //         return false;
        // }
        // return word.Length == 0;

        // This is a more efficient way to check if the word can be formed, 2 WORDS ONLY
        Trie trie = new Trie();
        foreach (string str in list)
        {
            trie.InsertNode(str);
        }
        TrieNode? crawler = trie.m_root;
        for (int i = 0; i < word.Length; i++)
        {
            int index = trie.GetIndex(word[i]);
            if (crawler[index] is null)
                return false;
            else if (crawler[index].IsEndWord())
            {
                if (trie.SearchNode(word.Substring(i + 1)))
                    return true;

                return IsFormationPossible(list, word.Substring(i + 1));
            }

            crawler = crawler[index];
        }

        return false;
    }
    public List<string> SortArray(string[] arr)
    {
        Trie trie = new Trie();
        foreach (string word in arr)
        {
            trie.InsertNode(word);
        }

        return trie.FindWords();
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

    public List<string> FindWords() => FindWords(m_root);

    private List<string> FindWords(TrieNode? root)
    {
        List<string> words = new List<string>();
        string word = "";
        FindWords(root, words, ref word, 0);
        return words;
    }

    private void FindWords(TrieNode? root, List<string> words, ref string word, int level)
    {
        if (root == null)
            return;
        if (root.IsEndWord())
        {
            string temp = "";
            for (int i = 0; i < level; i++)
            {
                temp += word[i];
            }

            words.Add(temp);
        }

        for (int i = 0; i < TrieNode.Size; i++)
        {
            if (root[i] is null)
                continue;

            if (level < word.Length)
            {
                StringBuilder sb = new StringBuilder(word) { [level] = (char)('a' + i) };
                word = sb.ToString();
            }
            else
            {
                word += (char)('a' + i);
            }

            FindWords(root[i], words, ref word, level + 1);
        }
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
