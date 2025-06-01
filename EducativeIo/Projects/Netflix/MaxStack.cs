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

            public bool VerifySession(IEnumerable<int> pushOp, IEnumerable<int> popOp)
            {
                if (pushOp.Count() != popOp.Count()) return false;

                Stack<int> stack = new Stack<int>();
                int i = 0;
                foreach (int op in pushOp)
                {
                    stack.Push(op);
                    while (stack.Count > 0 && stack.Peek() == popOp.ElementAt(i))
                    {
                        i++;
                        stack.Pop();
                    }
                }

                if (stack.Count == 0) return true;

                return false;
            }
        }
    }
}