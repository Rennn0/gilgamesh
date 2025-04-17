namespace EducativeIo.StackQueue
{
    public class NewStack
    {
        private readonly Stack<int> _stack;
        private readonly Stack<int> _minStack;
        private readonly int _size;

        public NewStack(int size)
        {
            _stack = new Stack<int>(size);
            _minStack = new Stack<int>(size);
            this._size = size;
        }

        public int Pop()
        {
            if (_stack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            _minStack.Pop();
            return _stack.Pop();
        }

        public void Push(int value)
        {
            if (_stack.Count == _size)
            {
                throw new Exception("Stack is full");
            }

            _stack.Push(value);
            if (_minStack.Count != 0 && value > _minStack.Peek())
            {
                _minStack.Push(_minStack.Peek());
            }
            else
            {
                _minStack.Push(value);
            }
        }

        public int Min()
        {
            if (_minStack.Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            return _minStack.Peek();
        }
    }
}
