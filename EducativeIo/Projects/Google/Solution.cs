namespace EducativeIo.Projects.Google
{
    public class Solution
    {
        // 1 2 3 4 5 6 7 8
        // + + + +          (1)
        //   + + + +        (2)
        //       + + + + +  (1)
        //         + +      (3)
        //         + + + +  (3)
        //           + +    (2)

        public int[][] MergeIntervals(int[][] intervals)
        {
            List<List<int>> result = new List<List<int>>();
            intervals = intervals.OrderBy(i => i[0]).ToArray();
            result.Add(intervals[0].ToList());

            for (int i = 1; i < intervals.Length; i++)
            {
                if (intervals[i][0] <= result[^1][1])
                {
                    result[^1][1] = Math.Max(intervals[i][1], result[^1][1]);
                }
                else
                {
                    result.Add(intervals[i].ToList());
                }
            }

            return result.Select(l => l.ToArray()).ToArray();
        }
    }
}