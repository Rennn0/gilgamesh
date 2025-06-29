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

        public class MyCalendar
        {
            internal class CalendarNode
            {
                internal int Start
                {
                    get;
                }
                internal int End
                {
                    get;
                }
                internal CalendarNode? Before
                {
                    get; set;
                }
                internal CalendarNode? After
                {
                    get; set;
                }
                public CalendarNode(int s, int e)
                {
                    Start = s;
                    End = e;
                }
            }
            private CalendarNode? _calendar;
            public bool Book(int start, int end)
            {
                if (_calendar is null)
                {
                    _calendar = new CalendarNode(start, end);
                    return true;
                }

                return Book(new CalendarNode(start, end), _calendar);
            }

            private bool Book(CalendarNode node, CalendarNode root)
            {
                if (root.End <= node.Start)
                {
                    if (root.After is null)
                    {
                        root.After = node;
                        return true;
                    }
                    return Book(node, root.After);
                }

                if (root.Start >= node.End)
                {
                    if (root.Before is null)
                    {
                        root.Before = node;
                        return true;
                    }
                    return Book(node, root.Before);
                }
                return false;
            }
        }

        public int[][] InsertInterval(int[][] intervals, int[] newInterval)
        {
            List<List<int>> result = new List<List<int>>();
            foreach (int[] i in intervals)
            {
                result.Add(i.ToList());
            }
            result.Add(newInterval.ToList());

            int[][] res = MergeIntervals(result.Select(r => r.ToArray()).OrderBy(r => r[0]).ToArray());

            return res;
        }


        // 1 2 3 4 5 6 7 8 9 10
        // + + +   + +,+ + +
        //
        //   + + +   + +,+ + +
        public int[][] IntervalIntersection(int[][] a, int[][] b)
        {
            List<List<int>> result = new List<List<int>>();
            a = a.OrderBy(x => x[0]).ToArray();
            b = b.OrderBy(x => x[0]).ToArray();

            int i = 0;
            int j = 0;

            while (i < a.Length && j < b.Length)
            {
                int minEnd = Math.Min(a[i][1], b[j][1]);
                int maxStart = Math.Max(a[i][0], b[j][0]);

                if (maxStart < minEnd)
                {
                    result.Add(new List<int> { maxStart, minEnd });
                }

                if (a[i][1] < b[j][1])
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }

            return result.Select(r => r.ToArray()).ToArray();
        }
        public class Interval
        {
            public int start;
            public int end;

            public Interval()
            {
            }
            public Interval(int _start, int _end)
            {
                start = _start;
                end = _end;
            }
        }
        public List<Interval> EmployeeFreeTime(List<List<Interval>> schedule)
        {
            List<Interval> intervals = new List<Interval>();
            foreach (List<Interval> s in schedule)
            {
                intervals.AddRange(s);
            }

            intervals = intervals.OrderBy(x => x.start).ThenBy(x => x.end).ToList();
            List<Interval> result = new List<Interval>();
            if (intervals.Count == 0)
            {
                return result;
            }

            int lastMergedEnd = intervals[0].end;

            for (int i = 1; i < intervals.Count; i++)
            {
                if (lastMergedEnd >= intervals[i].start)
                {
                    lastMergedEnd = Math.Max(lastMergedEnd, intervals[i].end);
                }
                else
                {
                    result.Add(new Interval(lastMergedEnd, intervals[i].start));
                    lastMergedEnd = intervals[i].end;
                }
            }


            return result;
        }

        // -1 0 1 3 4 5 6 7
        public int LongestConsecutiveSequence(int[] arr)
        {   
            new Feature().MergeSort(arr, 0, arr.Length - 1);
            HashSet<int> hs = new HashSet<int>(arr);
            int g = 0;
            int c = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == arr[i - 1] + 1)
                {
                    c++;
                }
                else
                {
                    g = Math.Max(g, c);
                    c = 0;
                }
            }

            g = Math.Max(g, c);

            return g + 1;
        }
    }
}