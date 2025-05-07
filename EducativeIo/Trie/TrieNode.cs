using System.Diagnostics;

namespace EducativeIo.Trie;

[DebuggerDisplay("{DebugView}")]
public class TrieNode
{
    public const int Size = 26;
    private readonly TrieNode?[] m_children;
    private bool m_isEndOfWord;

    public TrieNode? this[int i]
    {
        get => m_children[i];
        set => m_children[i] = value;
    }

    public TrieNode()
    {
        m_isEndOfWord = false;
        m_children = new TrieNode[Size];
        for (int i = 0; i < Size; i++)
        {
            m_children[i] = null;
        }
    }

    public bool IsEndWord() => m_isEndOfWord;

    public void MarkAsLeaf() => m_isEndOfWord = true;

    public void UnMarkAsLeaf() => m_isEndOfWord = false;

    private string DebugView
    {
        get
        {
            List<string> nonNullIndices = new List<string>();
            for (int i = 0; i < Size; i++)
            {
                if (m_children[i] == null)
                    continue;

                char c = (char)('a' + i);
                nonNullIndices.Add($"[{i}]{c}");
            }

            return $"{string.Join(", ", nonNullIndices)}, IsEnd: {m_isEndOfWord}";
        }
    }

    public override string ToString() => DebugView;
}
