using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;

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

        public void AddEdge(int from, int to)
        {
            Edges.Add(new Edge(from, to, Directed));
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

        public void ResetVisited()
        {
            foreach (var v in Vertices)
                v.Visited = false;
        }
    }
}
