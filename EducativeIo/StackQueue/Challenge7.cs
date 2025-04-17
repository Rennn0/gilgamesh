namespace EducativeIo.StackQueue;

public class Challenge7
{
    public static int[] NextGreaterElementStack(int[] arr, int size)
    {
        int[] result = new int[size];
        Stack<int> stack = new Stack<int>();

        for (int i = size - 1; i >= 0; i--)
        {
            int next = arr[i];

            int top = stack.Count > 0 ? stack.Peek() : -1;

            while (stack.Count > 0 && top <= next)
            {
                stack.Pop();
                top = stack.Count > 0 ? stack.Peek() : -1;
            }

            result[i] = stack.Count > 0 ? top : -1;
            stack.Push(next);
        }

        return result;
    }

    public static int[] NextGreaterElementBruteForce(int[] arr, int size)
    {
        int[] result = new int[size];
        Stack<int> stack = new Stack<int>();

        for (int i = 0; i < size - 1; i++)
        {
            bool found = false;
            for (int j = i + 1; j < size; j++)
            {
                if (arr[j] > arr[i])
                {
                    stack.Push(arr[j]);
                    found = !found;
                    break;
                }
            }

            if (!found)
            {
                stack.Push(-1);
            }
        }

        stack.Push(-1);

        for (int i = size - 1; i >= 0; i--)
        {
            result[i] = stack.Pop();
        }

        return result;
    }
}
