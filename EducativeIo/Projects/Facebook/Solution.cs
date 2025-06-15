namespace EducativeIo.Projects.Facebook
{
    public partial class Solution
    {
        public static int NumIslands(string[][] grid)
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

        public static int FindProvincesNum(int[][] matrix)
        {
            if (matrix.Length.Equals(0) || matrix[0].Length.Equals(0)) return -1;
            bool[] visited = new bool[matrix.Length];
            int provinces = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                if (!visited[i])
                {
                    provinces++;
                    DFS(i);
                }
            }
            return provinces;
            void DFS(int city)
            {
                visited[city] = true;
                for (int i = 0; i < matrix.Length; i++)
                {
                    if (!visited[i] && matrix[city][i].Equals(1))
                    {
                        DFS(i);
                    }
                }
            }
        }

        public static int CountConnectedComp(int[][] edges, int vertices)
        {
            int[][] matrix = new int[vertices][];
            for (int i = 0; i < vertices; i++)
            {
                matrix[i] = new int[vertices];
                matrix[i][i] = 1;
            }

            for (int i = 0; i < edges.Length; i++)
            {
                matrix[edges[i][0]][edges[i][1]] = 1;
                matrix[edges[i][1]][edges[i][0]] = 1;
            }

            return FindProvincesNum(matrix);
        }

        public static int SearchRotated(int[] arr, int k)
        {
            if (arr.Length.Equals(0)) return -1;
            return SearchRotated(arr, 0, arr.Length - 1, k);
        }

        public static int SearchRotated(int[] arr, int l, int r, int k)
        {
            if (l > r) return -1;

            int mid = l + (r - l) / 2;

            if (arr[mid].Equals(k)) return mid;

            if (arr[l] <= arr[mid] && k <= arr[mid] && k >= arr[l])
                return SearchRotated(arr, l, mid - 1, k);
            else if (arr[r] >= arr[mid] && k >= arr[mid] && k <= arr[r])
                return SearchRotated(arr, mid + 1, r, k);
            else if (arr[l] >= arr[mid])
                return SearchRotated(arr, l, mid - 1, k);
            else if (arr[r] <= arr[mid])
                return SearchRotated(arr, mid + 1, r, k);
            return -1;
        }


        private static void DFS(string[][] grid, int r, int c)
        {
            int row = grid.Length;
            int col = grid[0].Length;

            if (r < 0 || r >= row || c < 0 || c >= col || !grid[r][c].Equals("1")) return;

            grid[r][c] = "0";

            DFS(grid, r - 1, c);
            DFS(grid, r + 1, c);
            DFS(grid, r, c - 1);
            DFS(grid, r, c + 1);

            DFS(grid, r - 1, c - 1);
            DFS(grid, r - 1, c + 1);
            DFS(grid, r + 1, c - 1);
            DFS(grid, r + 1, c + 1);
        }

    }

    public abstract class SomeDomain
    {
        public int SpecialNumber { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? SpecialDate { get; set; }

    }
}
