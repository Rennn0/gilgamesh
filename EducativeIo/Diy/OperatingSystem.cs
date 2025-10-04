namespace EducativeIo.Diy;

public abstract class OsNode<T>
{
    protected OsNode()
    {
    }

    protected OsNode(T value)
    {
        Value = value;
    }

    protected OsNode(T value, OsNode<T> left, OsNode<T> right)
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
        private set;
    }

    public OsNode<T>? Right
    {
        get;
        private set;
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