namespace EducativeIo.StackQueue
{
    public class NewStack
    {
        private readonly Stack<int> m_stack;
        private readonly Stack<int> m_minStack;
        private readonly int m_size;

        public NewStack(int size)
        {
            m_stack = new Stack<int>(size);
            m_minStack = new Stack<int>(size);
            this.m_size = size;
        }

        public int Pop()
        {
            if (m_stack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            m_minStack.Pop();
            return m_stack.Pop();
        }

        public void Push(int value)
        {
            if (m_stack.Count == m_size)
            {
                throw new Exception("Stack is full");
            }

            m_stack.Push(value);
            if (m_minStack.Count != 0 && value > m_minStack.Peek())
            {
                m_minStack.Push(m_minStack.Peek());
            }
            else
            {
                m_minStack.Push(value);
            }
        }

        public int Min()
        {
            if (m_minStack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            return m_minStack.Peek();
        }
    }
}