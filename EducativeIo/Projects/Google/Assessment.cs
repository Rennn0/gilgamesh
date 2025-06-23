public class Assesment
{
    public int SubrectangleSum(int[][] matrix)
    {
        // int sum = 0;
        // int len = matrix.Length;

        // int globalSum = matrix[0][0];
        // int localSum = matrix[0][0];

        // for (int row = 0; row < len; row++)
        // {
        //     for (int col = 0; col < len; col++)
        //     {
        //         if (localSum < 0)
        //         {
        //             localSum = matrix[row][col];
        //         }
        //         else
        //         {
        //             localSum += matrix[row][col];
        //         }

        //         if (globalSum < localSum)
        //         {
        //             globalSum = localSum;
        //         }
        //     }
        // }

        int row = matrix.Length;
        int col = matrix.Length;

        int maxSubmatrix = 0;

        for (int r = 0; r < row; r++)
        {

            for (int c = 0; c < col; c++)
            {

                for (int k = r; k < row; k++)
                {

                    for (int l = c; l < col; l++)
                    {

                        int sumSubmatrix = 0;

                        for (int m = r; m <= k; m++)
                        {
                            for (int n = c; n <= l; n++)
                            {
                                sumSubmatrix += matrix[m][n];
                            }
                        }

                        maxSubmatrix
                            = Math.Max(maxSubmatrix,
                                  sumSubmatrix);
                    }
                }
            }
        }
        return maxSubmatrix;
    }
}
