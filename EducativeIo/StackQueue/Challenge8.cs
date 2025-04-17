namespace EducativeIo.StackQueue;

public class Challenge8
{
    public static bool IsBalanced(string exp)
    {
        Dictionary<char, char> brackets = new Dictionary<char, char>
        {
            { '(', ')' },
            { '{', '}' },
            { '[', ']' },
        };
        Stack<char> stack = new Stack<char>();

        for (int i = 0; i < exp.Length; i++)
        {
            if (brackets.ContainsKey(exp[i]))
            {
                stack.Push(exp[i]);
            }
            else if (brackets.ContainsValue(exp[i]))
            {
                if (stack.Count == 0 || brackets[stack.Pop()] != exp[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
