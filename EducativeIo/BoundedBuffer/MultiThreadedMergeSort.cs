namespace EducativeIo.BoundedBuffer;

public class MultiThreadedMergeSort
{
    private readonly int[] _arr;

    public MultiThreadedMergeSort(int size)
    {
        _arr = new int[size];
    }

    public void Sort(int start, int end, int[] arr)
    {
        if (start == end) return;
        int mid = (start + end) / 2;

        Sort(start, mid, arr);
        Sort(mid + 1, end, arr);

        // Thread leftThread = new Thread(() => Sort(start, mid, arr));
        // Thread rightThread = new Thread(() => Sort(mid + 1, end, arr));

        // leftThread.Start();
        // rightThread.Start();

        // leftThread.Join();
        // rightThread.Join();

        int left = start;
        int right = mid + 1;
        int k;

        for (k = start; k <= end; k++)
        {
            _arr[k] = arr[k];
        }

        for (k = start; k <= end; k++)
        {
            if (left <= mid && right <= end)
            {
                arr[k] = Math.Min(_arr[left], _arr[right]);
                if (arr[k] == _arr[left])
                {
                    left++;
                }
                else
                {
                    right++;
                }
            }
            else if (left <= mid && right > end)
            {
                arr[k] = _arr[left];
                left++;
            }
            else
            {
                arr[k] = _arr[right];
                right++;
            }
        }
    }
}
