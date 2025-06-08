namespace EducativeIo.Projects.Facebook
{
    public partial class Feature
    {
        public int FriendCicles(bool[][] friends)
        {
            int n = friends.Length;
            if (n == 0) return -1;

            int numCicles = 0;
            bool[] visited = new bool[n];

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    visited[i] = true;
                    DFS(friends, visited, i);
                    numCicles++;
                }
            }

            return numCicles;
        }

        public void DFS(bool[][] friends, bool[] visited, int v)
        {
            for (int i = 0; i < friends.Length; i++)
            {
                if (friends[v][i] && !visited[i] && i != v)
                {
                    visited[i] = true;
                    DFS(friends, visited, i);
                }
            }
        }
    }
}