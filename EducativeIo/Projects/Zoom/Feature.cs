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
    }
}