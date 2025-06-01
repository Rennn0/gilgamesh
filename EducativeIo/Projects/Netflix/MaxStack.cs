namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        public class MinStack
        {
            private readonly Stack<int> _mainStack;
            private readonly Stack<int> _maxStack;
            public MinStack()
            {
                _mainStack = new Stack<int>();
                _maxStack = new Stack<int>();
            }

            public void Push(int value)
            {
                _mainStack.Push(value);

                if (_maxStack.Count > 0 && _maxStack.Peek() >= value)
                {
                    _maxStack.Push(_maxStack.Peek());
                }
                else
                {
                    _maxStack.Push(value);
                }
            }

            public int Pop()
            {
                _maxStack.Pop();
                return _mainStack.Pop();
            }
            public int GetMax() => _maxStack.Peek();

        }
    }
}