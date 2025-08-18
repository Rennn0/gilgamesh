namespace EducativeIo.Projects.Zoom
{
    public class Feature
    {
        public static void RotateMatrix(ref int[][] matrix)
        {
            int left = 0;
            int right = matrix.Length - 1;

            while (left < right)
            {
                for (int i = 0; i < right - left; i++)
                {
                    int top = left;
                    int bottom = right;
                    int topLeft = matrix[top][left + i];

                    // up
                    matrix[top][left + i] = matrix[bottom - i][left];
                    // left
                    matrix[bottom - i][left] = matrix[bottom][right - i];
                    // down
                    matrix[bottom][right - i] = matrix[top + i][right];
                    // right
                    matrix[top + i][right] = topLeft;
                }
                left++;
                right--;
            }
        }

        public static int JumpGame(int[] arr)
        {
            int jumps = 0;
            Dictionary<int, List<int>> valuesToIndexMap = new Dictionary<int, List<int>>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (valuesToIndexMap.TryGetValue(arr[i], out List<int>? indexes))
                {
                    indexes.Add(i);
                }
                else
                {
                    valuesToIndexMap[arr[i]] = [i];
                }
            }

            for (int i = 0; i < arr.Length; i++, jumps++)
            {
                int current = arr[i];
                int lastIndex = valuesToIndexMap[current][^1];
                if (lastIndex > i)
                {
                    i = lastIndex - 1;
                }
            }

            return jumps - 1;
        }

        public static int NumMathingSubSeq(string sample, string[] words)
        {
            Dictionary<char,uint> charMap = new Dictionary<char, uint>();
            int counter = 0;

            foreach (char c in sample)
            {
                if (charMap.TryGetValue(c, out uint value))
                {
                    charMap[c] = ++value;
                }
                else
                {
                    charMap.Add(c, 1);
                }
            }

            foreach (string word in words)
            {
                Dictionary<char, uint> tempMap = new Dictionary<char, uint>(charMap);
                bool isSubSeq = true;
                foreach (char c in word)
                {
                    if (tempMap.TryGetValue(c, out uint value) && value > 0)
                    {
                        tempMap[c]--;
                    }
                    else
                    {
                        isSubSeq = false;
                        break;
                    }
                }
                if (isSubSeq)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}