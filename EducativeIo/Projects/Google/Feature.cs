using EducativeIo.Heap;

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

        public void SortStack(Stack<int> unsortedStack)
        {
            if (unsortedStack.Count > 0)
            {
                int val = unsortedStack.Pop();
                SortStack(unsortedStack);
                Insert(unsortedStack, val);
            }

            static void Insert(Stack<int> stack, int val)
            {
                if (stack.Count == 0 || val < stack.Peek())
                {
                    stack.Push(val);
                }
                else
                {
                    int temp = stack.Pop();
                    Insert(stack, val);
                    stack.Push(temp);
                }
            }
        }

        public int[] NextGreaterElement(int[] arr)
        {
            int[] res = new int[arr.Length];
            Stack<int> s = new Stack<int>();

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                int top = s.Count > 0 ? s.Peek() : -1;

                while (s.Count > 0 && top <= arr[i])
                {
                    s.Pop();
                    top = s.Count > 0 ? s.Peek() : -1;
                }

                res[i] = s.Count > 0 ? top : -1;
                s.Push(arr[i]);
            }

            return res;
        }

        public int Islands(int[][] matrix)
        {
            int row = matrix.Length;
            int col = matrix[0].Length;
            int islands = 0;

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (matrix[r][c] == 1)
                    {
                        islands++;
                        DoDfs(matrix, r, c);
                    }
                }
            }

            return islands;
            void DoDfs(int[][] matrix, int row, int column)
            {
                if (row < 0 || row >= matrix.Length || column < 0 || column >= matrix[row].Length || matrix[row][column] != 1)
                {
                    return;
                }

                matrix[row][column] = 0;
                DoDfs(matrix, row - 1, column);
                DoDfs(matrix, row + 1, column);
                DoDfs(matrix, row, column - 1);
                DoDfs(matrix, row, column + 1);
            }
        }

        // 1,2,3,4,5,6,7,8  --(3)->  4,5,6,7,8,1,2,3
        public void RotateLeft(int[] arr, int n)
        {
            n %= arr.Length;
            Rotate(arr, 0, n - 1); // 3,2,1,4,5,6,7,8
            Rotate(arr, n, arr.Length - 1); // 3,2,1,8,7,6,5,4
            Rotate(arr, 0, arr.Length - 1);// 4,5,6,7,8,1,2,3
        }

        // 1,2,3,4,5,6,7,8  --(3)->  6,7,8,1,2,3,4,5
        public void RotateRight(int[] arr, int n)
        {
            n %= arr.Length;
            Rotate(arr, 0, arr.Length - 1); // 8,7,6,5,4,3,2,1
            Rotate(arr, 0, n - 1); // 6,7,8,5,4,3,2,1
            Rotate(arr, n, arr.Length - 1); // 6,7,8,1,2,3,4,5
        }

        private void Rotate(int[] arr, int l, int r)
        {
            while (l < r)
            {
                (arr[l], arr[r]) = (arr[r], arr[l]);
                l++;
                r--;
            }
        }

        public void MergeSort(int[] arr, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int mid = start + (end - start) / 2;

            MergeSort(arr, start, mid);
            MergeSort(arr, mid + 1, end);

            int left = start;
            int right = mid + 1;
            int[] temp = new int[arr.Length];

            for (int i = start; i <= end; i++)
            {
                temp[i] = arr[i];
            }

            for (int i = start; i <= end; i++)
            {
                if (left <= mid && right <= end)
                {
                    arr[i] = temp[left] < temp[right] ? temp[left++] : temp[right++];
                }
                else if (left <= mid && right > end)
                {
                    arr[i] = temp[left++];
                }
                else
                {
                    arr[i] = temp[right++];
                }
            }
        }

        public int MinMeetingRooms(int[][] meetingTimes)
        {
            if (meetingTimes.Length == 0)
            {
                return 0;
            }
            meetingTimes = meetingTimes.OrderBy(x => x[0]).ToArray();
            MinHeap<int> minHeap = new MinHeap<int>();
            minHeap.Insert(meetingTimes[0][1]);

            for (int i = 1; i < meetingTimes.Length; i++)
            {
                int beginning = meetingTimes[i][0];
                int ending = meetingTimes[i][1];
                int earliestEnding = minHeap.GetMin();
                if (earliestEnding <= beginning)
                {
                    minHeap.RemoveMin();
                }
                minHeap.Insert(ending);
            }

            return minHeap.Size();
        }


        public int SubrectangleSum(int[][] matrix)
        {
            int rows = matrix.Length;
            int cols = matrix.Length;

            int maxSum = int.MinValue;
            for (int row = 0; row < rows; row++)
            {
                int[] temp = new int[cols];

                for (int bottomRow = row; bottomRow < rows; bottomRow++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        temp[col] += matrix[bottomRow][col];
                    }

                    int currentMax = Kadane(temp);
                    maxSum = Math.Max(maxSum, currentMax);
                }
            }


            return maxSum;
        }

        public int Kadane(int[] arr)
        {
            int localMax = 0;
            int globalMax = int.MinValue;

            for (int k = 0; k < arr.Length; k++)
            {
                localMax += arr[k];
                if (localMax > globalMax)
                {
                    globalMax = localMax;
                }
                if (localMax < 0)
                {
                    localMax = 0;
                }
            }
            return globalMax;
        }

        public List<List<int>> MergeMeetings(int[][] meetings)
        {
            meetings = meetings.OrderBy(x => x[0]).ToArray();
            List<List<int>> result = new List<List<int>>();

            foreach (int[] meeting in meetings)
            {
                if (result.Count == 0 || result[^1][1] < meeting[0])
                {
                    result.Add(meeting.ToList());
                }
                else
                {
                    result[^1][1] = Math.Max(result[^1][1], meeting[1]);
                }
            }

            return result;
        }

        public bool CheckMeetings(int[][] meetings, int[] newMeeting)
        {
            MeetingTree meetingTree = new MeetingTree();
            foreach (int[] meeting in meetings)
            {
                meetingTree.Add(meeting[0], meeting[1]);
            }
            return meetingTree.Add(newMeeting[0], newMeeting[1]);
        }

        public class MeetingTree
        {
            internal class MeetingTreeNode
            {
                public int Start
                {
                    get;
                }
                public int End
                {
                    get;
                }

                public MeetingTreeNode? Left
                {
                    get; internal set;
                }
                public MeetingTreeNode? Right
                {
                    get; internal set;
                }

                public MeetingTreeNode(int s, int e)
                {
                    Start = s;
                    End = e;
                }
            }
            private MeetingTreeNode? Root
            {
                get; set;
            }
            public MeetingTree()
            {
                Root = null;
            }
            public bool Add(int start, int end)
            {
                if (Root is null)
                {
                    Root = new MeetingTreeNode(start, end);
                    return true;
                }

                return Add(Root, new MeetingTreeNode(start, end));
            }
            private bool Add(MeetingTreeNode currentTree, MeetingTreeNode newTree)
            {
                if (newTree.Start >= currentTree.End)
                {
                    if (currentTree.Right is null)
                    {
                        currentTree.Right = newTree;
                        return true;
                    }
                    return Add(currentTree.Right, newTree);
                }
                else if (newTree.End <= currentTree.Start)
                {
                    if (currentTree.Left is null)
                    {
                        currentTree.Left = newTree;
                        return true;
                    }
                    return Add(currentTree.Left, newTree);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}