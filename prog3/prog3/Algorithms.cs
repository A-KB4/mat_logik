
using System;
using System.Collections.Generic;

namespace prog2
{
    public static class Algorithms
    {
       
        public static (int[] dist, int[] prev) Dijkstra(Graph g, int src)
        {
            const int INF = int.MaxValue / 4;
            int n = g.Vertices.Count;

            int[] dist = new int[n];
            int[] prev = new int[n];
            bool[] used = new bool[n];

            for (int i = 0; i < n; i++)
            {
                dist[i] = INF;
                prev[i] = -1;
                used[i] = false;
            }
            dist[src] = 0;

            for (int it = 0; it < n; it++)
            {
                
                int v = -1;
                int best = INF;
                for (int i = 0; i < n; i++)
                {
                    if (!used[i] && dist[i] < best)
                    {
                        best = dist[i];
                        v = i;
                    }
                }
                if (v == -1) break; 
                used[v] = true;

                
                foreach (var nb in g.GetAdjWithWeights(v))
                {
                    int to = nb.to;
                    int w = nb.w;
                    if (dist[v] + w < dist[to])
                    {
                        dist[to] = dist[v] + w;
                        prev[to] = v;
                    }
                }
            }

            return (dist, prev);
        }

        public static List<int> ReconstructPath(int[] prev, int src, int dst)
        {
            var path = new List<int>();
            for (int v = dst; v != -1; v = prev[v]) path.Add(v);
            path.Reverse();
            if (path.Count == 0 || path[0] != src) return new List<int>(); // немає шляху
            return path;
        }

        // ---------- Floyd–Warshall (усі пари) ----------
        // next[i,j] = наступна вершина після i на шляху до j, або -1 якщо шляху немає
        public static (int[,] dist, int[,] next) FloydWarshall(Graph g)
        {
            const int INF = int.MaxValue / 4;
            int[,] dist = g.ToWeightMatrix(INF);
            int n = g.Vertices.Count;
            int[,] next = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    next[i, j] = (i != j && dist[i, j] < INF) ? j : -1;

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (dist[i, k] >= INF) continue;
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[k, j] >= INF) continue;
                        int nd = dist[i, k] + dist[k, j];
                        if (nd < dist[i, j])
                        {
                            dist[i, j] = nd;
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }
            return (dist, next);
        }

        public static List<int> FloydPath(int i, int j, int[,] next, int[,] dist)
        {
            const int INF = int.MaxValue / 4;
            if (i < 0 || j < 0 || next[i, j] == -1 || dist[i, j] >= INF) return new List<int>();
            var path = new List<int> { i };
            while (i != j)
            {
                i = next[i, j];
                if (i == -1) return new List<int>(); // перестраховка
                path.Add(i);
            }
            return path;
        }
    }
}
