namespace EducativeIo.BoundedBuffer;

public class MultiThreadedMergeSort
{
    public MultiThreadedMergeSort() { }

    public void Sort(int start, int end, int[] arr)
    {
        int[] temp = new int[arr.Length];
        Sort(start, end, arr, temp);
    }

    private void Sort(int start, int end, ref int[] arr, ref int[] temp)
    {
        if (start >= end)
            return;

        int mid = (start + end) / 2;
        Sort(start, mid, ref arr, ref temp);
        Sort(mid + 1, end, ref arr, ref temp);

        int l = start;
        int r = mid + 1;
        int i;

        for (i = start; i <= end; i++)
        {
            temp[i] = arr[i];
        }

        for (i = start; i <= end; i++)
        {
            if (l <= mid && r <= end)
            {
                arr[i] = temp[l] <= temp[r] ? temp[l++] : temp[r++];
            }
            else if (l <= mid && r > end)
            {
                arr[i] = temp[l++];
            }
            else
            {
                arr[i] = temp[r++];
            }
        }
    }

    private void Sort(int start, int end, int[] arr, int[] temp)
    {
        if (start >= end)
            return;

        int mid = (start + end) / 2;

        Thread tl = new Thread(() => Sort(start, mid, arr, temp));
        Thread tr = new Thread(() => Sort(mid + 1, end, arr, temp));
        tl.Start();
        tr.Start();
        tl.Join();
        tr.Join();

        int l = start;
        int r = mid + 1;
        int i;

        for (i = start; i <= end; i++)
        {
            temp[i] = arr[i];
        }

        for (i = start; i <= end; i++)
        {
            if (l <= mid && r <= end)
            {
                arr[i] = temp[l] <= temp[r] ? temp[l++] : temp[r++];
            }
            else if (l <= mid && r > end)
            {
                arr[i] = temp[l++];
            }
            else
            {
                arr[i] = temp[r++];
            }
        }
    }
}
