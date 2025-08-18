using System.Reflection.Metadata;
using EducativeIo.Hash;

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

        public static List<int> GetDevices(NetworkNode root, NetworkNode server, int ttl)
        {
            Dictionary<int, List<int>> neighbors = new Dictionary<int, List<int>>();
            Dfs(null, root, neighbors);

            List<int> bfs = new List<int>() { server.Data };
            HashSet<int> lookup = new HashSet<int>(bfs);

            for (int i = 0; i < ttl; i++)
            {
                List<int> temp = new List<int>();
                foreach (int node in bfs)
                {
                    foreach (int n in neighbors[node])
                    {
                        if (lookup.Contains(n))
                        {
                            continue;
                        }
                        temp.Add(n);
                    }
                }

                bfs = temp;
                lookup.UnionWith(bfs);
            }

            return bfs;
        }

        private static void Dfs(
            NetworkNode? parent,
            NetworkNode? child,
            Dictionary<int, List<int>> neighbors
        )
        {
            if (child is null)
            {
                return;
            }

            if (parent is not null)
            {
                if (neighbors.TryGetValue(parent.Data, out List<int>? pVal))
                {
                    pVal.Add(child.Data);
                }
                else
                {
                    neighbors[parent.Data] = new List<int> { child.Data };
                }

                if (neighbors.TryGetValue(child.Data, out List<int>? cVal))
                {
                    cVal.Add(parent.Data);
                }
                else
                {
                    neighbors[child.Data] = new List<int> { parent.Data };
                }
            }

            for (int i = 0; i < child.Children.Count; i++)
            {
                Dfs(child, child.Children[i], neighbors);
            }
        }

        public class NetworkNode
        {
            public int Data { get; }
            public List<NetworkNode> Children { get; }

            public NetworkNode(int data)
            {
                Data = data;
                Children = new List<NetworkNode>();
            }
        }
    }
}
