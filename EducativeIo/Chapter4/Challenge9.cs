namespace EducativeIo.Chapter4
{
    public class newStack
    {
        Stack<int> stack;
        Stack<int> minStack;
        int size;
        public newStack(int size)
        {
            stack = new Stack<int>(size);
            minStack = new Stack<int>(size);
            this.size = size;
        }

        public int pop()
        {
            if (stack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            minStack.Pop();
            return stack.Pop();
        }

        public void push(int value)
        {
            if (stack.Count == size)
            {
                throw new Exception("Stack is full");
            }
            stack.Push(value);
            if (minStack.Count != 0 && value > minStack.Peek())
            {
                minStack.Push(minStack.Peek());
            }
            else
            {
                minStack.Push(value);
            }
        }

        public int min()
        {
            if (minStack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }
            return minStack.Peek();
        }
    }
}