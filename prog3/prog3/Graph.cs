using System.Collections.Generic;
using System.Linq;

namespace prog2
{
    public class Graph
    {
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; private set; }
        public bool Directed { get; private set; }

        public Graph(bool directed)
        {
            Directed = directed;
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }

        public void AddVertex(string name, int x, int y)
        {
            Vertices.Add(new Vertex(name, x, y));
        }

        // OLD signature kept: weight default = 1
        public void AddEdge(int from, int to, int weight = 1)
        {
            Edges.Add(new Edge(from, to, Directed, weight));
            if (!Directed) // для неорієнтованого додамо симетрію
                Edges.Add(new Edge(to, from, Directed, weight));
        }

        public List<int> GetAdjList(int v)
        {
            List<int> adj = new List<int>();
            foreach (var e in Edges)
            {
                if (e.From == v) adj.Add(e.To);
                if (!Directed && e.To == v) adj.Add(e.From);
            }
            return adj.OrderBy(x => x).ToList();
        }

        // NEW: сусіди з вагами для алгоритмів
        public IEnumerable<(int to, int w)> GetAdjWithWeights(int v)
        {
            foreach (var e in Edges)
                if (e.From == v) yield return (e.To, e.Weight);
        }

        // NEW: матриця ваг для Флойда
        public int[,] ToWeightMatrix(int inf = int.MaxValue / 4)
        {
            int n = Vertices.Count;
            int[,] m = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = (i == j) ? 0 : inf;

            foreach (var e in Edges)
                if (e.Weight < m[e.From, e.To])
                    m[e.From, e.To] = e.Weight;

            return m;
        }

        public void ResetVisited()
        {
            foreach (var v in Vertices)
                v.Visited = false;
        }
    }
}
