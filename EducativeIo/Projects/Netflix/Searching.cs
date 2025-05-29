using System.Text;

namespace EducativeIo.Projects.Netflix;

/// <summary>
///     Group Similar Titles
/// </summary>
public partial class Netflix
{
    private readonly Dictionary<string, LinkedList<string>> _anagrams = new Dictionary<string, LinkedList<string>>();

    private const int AlphabetSize = 26;

    /// <summary>
    ///     Create vector for anagram, example: "abbcc" => "#1#2#3#0#0...."
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected virtual string CreateVector(string input)
    {
        StringBuilder builder = new StringBuilder();

        int[] vector = new int[AlphabetSize];
        foreach (
            int index in input.Select(GetIndex).Where(index => index is >= 0 and < AlphabetSize)
        )
        {
            vector[index]++;
        }

        for (int i = 0; i < AlphabetSize; i++)
        {
            builder.Append($"#{vector[i]}");
        }

        return builder.ToString();
    }

    protected virtual int GetIndex(char c) => c - 'a';

    public virtual void AddRange(IEnumerable<string> input) => input.ToList().ForEach(Add);

    public virtual void Add(string input)
    {
        string vector = CreateVector(input);

        if (_anagrams.TryGetValue(vector, out LinkedList<string>? anagram))
        {
            anagram.AddFirst(input);
        }
        else
        {
            _anagrams[vector] = new LinkedList<string>([input]);
        }
    }

    public virtual IEnumerable<string> Search(string input)
    {
        string vector = CreateVector(input);
        if (!_anagrams.TryGetValue(vector, out LinkedList<string>? anagram))
            return [];
        return anagram.OrderBy(title => title.Equals(input, StringComparison.Ordinal) ? 0 : 1);
    }
}
