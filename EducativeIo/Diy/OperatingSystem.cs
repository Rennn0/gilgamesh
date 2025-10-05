namespace EducativeIo.Diy;

public class OsNode<T>
{
    public OsNode()
    {
    }

    public OsNode(T value)
    {
        Value = value;
    }

    public OsNode(T value, OsNode<T> left, OsNode<T> right)
    {
        Value = value;
        Left = left;
        Right = right;
    }

    public T? Value
    {
        get;
        set;
    }

    public OsNode<T>? Left
    {
        get;
        set;
    }

    public OsNode<T>? Right
    {
        get;
        set;
    }

    public static OsNode<T>? InvertBinaryTree<T>(OsNode<T>? root)
    {
        if (root is null)
        {
            return root;
        }

        OsNode<T>? left = InvertBinaryTree(root.Left);
        OsNode<T>? right = InvertBinaryTree(root.Right);

        root.Left = right;
        root.Right = left;
        return root;
    }

    public static int MaxPathSum(OsNode<int>? root)
    {
        int maxSum = int.MinValue;
        recurse(root);
        return maxSum;

        int recurse(OsNode<int>? node)
        {
            if (node is null)
            {
                return 0;
            }

            int left = recurse(node.Left);
            int right = recurse(node.Right);

            int price = node.Value;
            if (left > 0)
            {
                price += left;
            }

            if (right > 0)
            {
                price += right;
            }

            maxSum = Math.Max(maxSum, price);

            return node.Value + Math.Max(left, right);
        }
    }

    public static List<List<T?>> ZigZagTraversal(OsNode<T>? root)
    {
        if (root is null)
        {
            return new List<List<T?>>();
        }

        LinkedList<OsNode<T>> deque = new LinkedList<OsNode<T>>();
        List<List<T?>> result = new List<List<T?>>();
        bool leftToRight = true;
        deque.AddFirst(root);
        while (deque.Count > 0)
        {
            int size = deque.Count;
            result.Add(new List<T?>());
            for (int i = 0; i < size; i++)
            {
                if (leftToRight)
                {
                    OsNode<T> node = deque.First!.Value;
                    deque.RemoveFirst();
                    result[^1].Add(node.Value);
                    if (node.Left is not null)
                    {
                        deque.AddLast(node.Left);
                    }

                    if (node.Right is not null)
                    {
                        deque.AddLast(node.Right);
                    }
                }
                else
                {
                    OsNode<T> node = deque.Last!.Value;
                    deque.RemoveLast();
                    result[^1].Add(node.Value);
                    if (node.Right is not null)
                    {
                        deque.AddFirst(node.Right);
                    }

                    if (node.Left is not null)
                    {
                        deque.AddFirst(node.Left);
                    }
                }
            }

            leftToRight = !leftToRight;
        }

        return result;
    }
}

public class OperatingSystem
{
    /// <summary>
    ///     Allocate Space
    /// </summary>
    /// <param name="processes">processes mb size</param>
    /// <param name="newP">new process mb size</param>
    /// <returns></returns>
    public int PossibleSpaceAllocation(int[] processes, int newP)
    {
        int sum = 0;
        int count = 0;
        Dictionary<int, int> dict = new Dictionary<int, int> { [0] = 1 };

        foreach (int t in processes)
        {
            sum += t;
            if (dict.TryGetValue(sum - newP, out int c))
            {
                count += c;
            }

            dict[sum] = dict.TryGetValue(sum, out int s) ? s + 1 : 1;
        }

        return count;
    }

    public int GetMissingProcessId(int[] processes, int nthProcess)
    {
        return getMissingProcessId(0, processes.Length);

        int getMissingProcessId(int left, int right)
        {
            if (left + 1 == right)
            {
                return processes[left] + nthProcess;
            }

            int mid = (left + right) / 2;
            int missingProcesses = processes[mid] - processes[left] - (mid - left);
            if (nthProcess > missingProcesses)
            {
                nthProcess -= missingProcesses;
                return getMissingProcessId(mid, right);
            }

            return getMissingProcessId(left, mid);
        }
    }
}