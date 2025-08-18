namespace EducativeIo.Projects.Network
{
    public class Feature
    {
        public static int TotalTime(int mainService, int[] parents, int[] delays)
        {
            int nParents = parents.Length;
            if (
                nParents <= 1
                || nParents != delays.Length
                || mainService < 0
                || mainService >= nParents
            )
            {
                return 0;
            }

            int result = 0;
            Dictionary<int, List<int>> childrenMap = new Dictionary<int, List<int>>();

            for (int i = 0; i < nParents; i++)
            {
                if (childrenMap.TryGetValue(parents[i], out List<int>? children))
                {
                    children.Add(i);
                }
                else
                {
                    childrenMap[parents[i]] = new List<int> { i };
                }
            }

            Queue<(int service, int delay)> treeQ = new Queue<(int service, int delay)>();
            treeQ.Enqueue((mainService, delays[mainService]));
            while (treeQ.Count > 0)
            {
                (int service, int delay) = treeQ.Dequeue();
                result = Math.Max(result, delay);
                if (!childrenMap.TryGetValue(service, out List<int>? children))
                {
                    continue;
                }

                foreach (int child in children)
                {
                    treeQ.Enqueue((child, delay + delays[child]));
                }
            }

            return result;
        }
    }
}
