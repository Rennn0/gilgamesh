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
        if (start == end)
            return;
        int mid = (start + end) / 2;

        Thread leftThread = new Thread(() => Sort(start, mid, arr));
        Thread rightThread = new Thread(() => Sort(mid + 1, end, arr));

        leftThread.Start();
        rightThread.Start();

        leftThread.Join();
        rightThread.Join();

        int i = start;
        int j = mid + 1;
        int k = start;

        for (; k <= end; k++)
        {
            _arr[k] = arr[k];
        }

        k = start;
        while (k <= end)
        {
            if (i <= mid && j <= end)
            {
                arr[k] = Math.Min(_arr[i], _arr[j]);
                if (arr[k] == _arr[i])
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }
            else if (i <= mid && j > end)
            {
                arr[k] = _arr[i];
                i++;
            }
            else
            {
                arr[k] = _arr[j];
                j++;
            }

            k++;
        }
    }
}
