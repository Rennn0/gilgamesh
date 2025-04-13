namespace EducativeIo.Chapter4;
public class Challenge6
{
    public static int evaluatePostFix(string exp, char splitter = ',')
    {
        Stack<int> stack = new Stack<int>();
        string[] tokens = exp.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < tokens.Length; i++)
        {
            if (!int.TryParse(tokens[i], out int num))
            {
                int right = stack.Pop();
                int left = stack.Pop();
                switch (tokens[i])
                {
                    case "+":
                        stack.Push(left + right);
                        break;
                    case "-":
                        stack.Push(left - right);
                        break;
                    case "*":
                        stack.Push(left * right);
                        break;
                    case "/":
                        stack.Push(left / right);
                        break;
                    default:
                        throw new Exception("Invalid operator");
                }
            }
            else
            {
                stack.Push(num);
            }
        }

        return stack.Pop();
    }
}