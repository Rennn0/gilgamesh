namespace EducativeIo.Chapter4;
public class Solution
{
    public static int[] nextGreaterElement(int[] arr, int size)
    {

        Stack<int> stack = new Stack<int>();

        int[] result = new int[size];

        int next, top;
        for (int i = size - 1; i >= 0; i--)
        {

            next = arr[i]; //potential nextGreaterElement
            if (stack.Count > 0)
            {
                top = stack.Peek();
            }
            else
            {
                top = -1;
            }

            while (stack.Count != 0 && top <= next)
            {

                stack.Pop();
                if (stack.Count > 0)
                {
                    top = stack.Peek();
                }
                else
                {
                    top = -1;
                }
            }

            if (stack.Count != 0)
                result[i] = stack.Peek();
            else
                result[i] = -1;

            stack.Push(next);

        }

        return result;
    }
}
