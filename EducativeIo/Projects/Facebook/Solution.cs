namespace EducativeIo.Projects.Facebook
{
    public partial class Solution
    {
        public int NumIslands(string[][] grid)
        {
            if (grid.Length.Equals(0) || grid[0].Length.Equals(0)) return -1;

            int islands = 0;
            int row = grid.Length;
            int col = grid[0].Length;

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (grid[r][c].Equals("1"))
                    {
                        islands++;
                        DFS(grid, r, c);
                    }
                }
            }

            return islands;
        }

        private void DFS(string[][] grid, int r, int c)
        {
            int row = grid.Length;
            int col = grid[0].Length;

            if (r < 0 || r >= row || c < 0 || c >= col || !grid[r][c].Equals("1")) return;

            grid[r][c] = "0";

            DFS(grid, r - 1, c);
            DFS(grid, r + 1, c);
            DFS(grid, r, c - 1);
            DFS(grid, r, c + 1);
        }
    }
}