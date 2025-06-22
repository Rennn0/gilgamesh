namespace EducativeIo.Projects.Google
{
    public class Feature
    {
        public int MaximumSubarraySum(int[] arr)
        {
            int gMax = arr[0];
            int lMax = arr[0];

            for (int i = 1; i < arr.Length; i++)
            {
                if (lMax < 0)
                {
                    lMax = arr[i];
                }
                else
                {
                    lMax += arr[i];
                }

                if (gMax < lMax)
                {
                    gMax = lMax;
                }
            }

            return gMax;
        }

        public List<string> BinaryNums(int n)
        {
            Queue<string> ints = new Queue<string>();
            List<string> nums = new List<string>();
            ints.Enqueue("1");

            for (int i = 0; i < n; i++)
            {
                string s = ints.Dequeue();
                nums.Add(s);
                ints.Enqueue($"{s}0");
                ints.Enqueue($"{s}1");
            }

            return nums;
        }
    }
}