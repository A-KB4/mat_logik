// MainForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace prog2
{
    public partial class MainForm : Form
    {
        private Graph graph;
        private Graphics g;
        private const int Radius = 25;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "DFS / BFS Visualization + Shortest Paths";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            g = panelCanvas.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            this.DoubleBuffered = true;
        }

        // ----------------- Малювання -----------------
        private void DrawVertex(Vertex v, Brush brush)
        {
            g.FillEllipse(brush, v.Position.X - Radius, v.Position.Y - Radius, Radius * 2, Radius * 2);
            g.DrawEllipse(Pens.Black, v.Position.X - Radius, v.Position.Y - Radius, Radius * 2, Radius * 2);
            using (var f = new Font("Arial", 10, FontStyle.Bold))
                g.DrawString(v.Name, f, Brushes.Black, v.Position.X - 8, v.Position.Y - 8);
        }

        private void DrawEdge(Vertex from, Vertex to, bool directed, int weight)
        {
            if (from == to && directed) { DrawSelfLoop(from); return; }

            var dx = to.Position.X - from.Position.X;
            var dy = to.Position.Y - from.Position.Y;
            var len = Math.Sqrt(dx * dx + dy * dy);
            if (len == 0) len = 1;
            var ux = dx / len;
            var uy = dy / len;

            var start = new PointF(
                (float)(from.Position.X + ux * Radius),
                (float)(from.Position.Y + uy * Radius)
            );

            var end = new PointF(
                (float)(to.Position.X - ux * Radius),
                (float)(to.Position.Y - uy * Radius)
            );

            using (Pen p = new Pen(Color.Black, 2))
            {
                if (directed) p.CustomEndCap = new AdjustableArrowCap(8, 8, true);
                g.DrawLine(p, start, end);
            }

            // Підпис ваги на середині ребра
            var mid = new PointF((start.X + end.X) / 2f, (start.Y + end.Y) / 2f);
            using (var f = new Font("Consolas", 9))
                g.DrawString(weight.ToString(), f, Brushes.DarkRed, mid);
        }

        private void DrawSelfLoop(Vertex v)
        {
            int r = 20;
            using (Pen p = new Pen(Color.Black, 2))
            {
                p.CustomEndCap = new AdjustableArrowCap(6, 6, true);
                var loopRect = new Rectangle(
                    v.Position.X + Radius - r,
                    v.Position.Y - Radius - r,
                    r * 2, r * 2
                );
                g.DrawArc(p, loopRect, 180, 270);
            }
        }

        private void DrawGraph()
        {
            g.Clear(Color.White);
            if (graph == null) return;

            // Ребра
            foreach (var e in graph.Edges)
                DrawEdge(graph.Vertices[e.From], graph.Vertices[e.To], graph.Directed, e.Weight);

            // Вершини
            foreach (var v in graph.Vertices)
                DrawVertex(v, Brushes.LightGray);
        }

        // ----------------- Лог -----------------
        private void ClearLog() => txtLog.Clear();

        private void Log(string s)
        {
            if (txtLog.TextLength > 0) txtLog.AppendText(Environment.NewLine);
            txtLog.AppendText(s);
        }

        private void LogHeader(string t)
        {
            Log(new string('─', 34));
            Log(t);
            Log(new string('─', 34));
        }

        private string VName(int i) => graph.Vertices[i].Name;

        // ----------------- Побудова графів (приклад з вагами; підстав свої) -----------------
        private void CreateDirectedGraph()
        {
            graph = new Graph(true);

            // Розміщення як у твоїй попередній формі
            graph.AddVertex("a", 200, 350);
            graph.AddVertex("b", 450, 350);
            graph.AddVertex("c", 150, 200);
            graph.AddVertex("d", 400, 200);
            graph.AddVertex("e", 275, 250);
            graph.AddVertex("f", 325, 430);

            // Ваги (під свою задачу можна змінити тут)
            graph.AddEdge(2, 0, 6);   // c→a
            graph.AddEdge(2, 3, 11);  // c→d
            graph.AddEdge(2, 4, 2);   // c→e
            graph.AddEdge(3, 4, 3);   // d→e
            graph.AddEdge(3, 1, 15);  // d→b
            graph.AddEdge(0, 5, 5);   // a→f
            graph.AddEdge(1, 5, 7);   // b→f
            graph.AddEdge(5, 1, 1);   // f→b

            DrawGraph();
            ClearLog();
            LogHeader("Орієнтований граф (з вагами) створено");
        }

        private void CreateUndirectedGraph()
        {
            graph = new Graph(false);

            graph.AddVertex("a", 200, 200);
            graph.AddVertex("b", 400, 200);
            graph.AddVertex("c", 150, 320);
            graph.AddVertex("d", 450, 320);
            graph.AddVertex("e", 300, 250);
            graph.AddVertex("f", 300, 420);

            graph.AddEdge(0, 1, 4); // a-b
            graph.AddEdge(0, 2, 3); // a-c
            graph.AddEdge(0, 4, 6); // a-e
            graph.AddEdge(1, 4, 2); // b-e
            graph.AddEdge(1, 3, 8); // b-d
            graph.AddEdge(2, 5, 5); // c-f
            graph.AddEdge(3, 5, 7); // d-f

            DrawGraph();
            ClearLog();
            LogHeader("Неорієнтований граф (з вагами) створено");
        }

        // ----------------- DFS / BFS -----------------
        private async void BtnDFS_Click(object sender, EventArgs e)
        {
            if (graph == null || graph.Vertices.Count == 0) return;

            graph.ResetVisited();
            ClearLog();
            LogHeader("DFS (stack)");

            var stack = new List<int> { 0 };
            var order = new List<int>();
            int step = 1;

            while (stack.Count > 0)
            {
                int v = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                Log($"Pop → {VName(v)}; Stack: [{string.Join(", ", stack.Select(VName))}]");

                if (graph.Vertices[v].Visited) { Log($"↷ {VName(v)} visited"); continue; }

                graph.Vertices[v].Visited = true;
                order.Add(v);

                DrawGraph();
                DrawVertex(graph.Vertices[v], Brushes.LightGreen);
                Log($"[{step++}] Visit {VName(v)}");

                var adj = graph.GetAdjList(v);
                adj.Reverse(); // щоб лівіші індекси відвідувались раніше
                foreach (int u in adj)
                    if (!graph.Vertices[u].Visited)
                    {
                        stack.Add(u);
                        Log($"  push {VName(u)}");
                    }

                await Task.Delay(600);
            }

            Log("Order: " + string.Join(" → ", order.Select(VName)));
        }

        private async void BtnBFS_Click(object sender, EventArgs e)
        {
            if (graph == null || graph.Vertices.Count == 0) return;

            graph.ResetVisited();
            ClearLog();
            LogHeader("BFS (queue)");

            var q = new Queue<int>();
            var order = new List<int>();
            int step = 1;

            q.Enqueue(0);
            graph.Vertices[0].Visited = true;
            Log($"enqueue {VName(0)}");

            while (q.Count > 0)
            {
                int v = q.Dequeue();

                DrawGraph();
                DrawVertex(graph.Vertices[v], Brushes.LightBlue);
                Log($"dequeue → {VName(v)}");
                Log($"[{step++}] Visit {VName(v)}");
                order.Add(v);

                foreach (int u in graph.GetAdjList(v))
                    if (!graph.Vertices[u].Visited)
                    {
                        graph.Vertices[u].Visited = true;
                        q.Enqueue(u);
                        Log($"  enqueue {VName(u)}");
                    }

                await Task.Delay(600);
            }

            Log("Order: " + string.Join(" → ", order.Select(VName)));
        }

        // ----------------- Дейкстра -----------------
        private void BtnDijkstra_Click(object sender, EventArgs e)
        {
            if (graph == null || graph.Vertices.Count == 0) return;

            ClearLog();
            LogHeader("Dijkstra from 'a'");

            int src = 0; // 'a'
            var res = Algorithms.Dijkstra(graph, src);
            int[] dist = res.dist;
            int[] prev = res.prev;

            for (int v = 0; v < graph.Vertices.Count; v++)
            {
                var path = Algorithms.ReconstructPath(prev, src, v);
                string sPath = path.Count == 0 ? "немає шляху" : string.Join("→", path.Select(VName));
                string sDist = dist[v] >= int.MaxValue / 4 ? "∞" : dist[v].ToString();
                Log($"a → {VName(v)} : dist = {sDist}; path = {sPath}");
            }
        }

        // ----------------- Флойд–Уоршелл -----------------
        private void BtnFloyd_Click(object sender, EventArgs e)
        {
            if (graph == null || graph.Vertices.Count == 0) return;

            ClearLog();
            LogHeader("Floyd–Warshall (all pairs)");

            var res = Algorithms.FloydWarshall(graph);
            int[,] dist = res.dist;
            int[,] next = res.next;
            int n = graph.Vertices.Count;

            Log("Matrix dist[i,j]:");
            for (int i = 0; i < n; i++)
            {
                string[] row = new string[n];
                for (int j = 0; j < n; j++)
                    row[j] = dist[i, j] >= int.MaxValue / 4 ? "∞" : dist[i, j].ToString();
                Log(string.Join("\t", row));
            }

            Log("Paths from a:");
            for (int j = 0; j < n; j++)
            {
                var path = Algorithms.FloydPath(0, j, next, dist);
                string sPath = path.Count == 0 ? "немає шляху" : string.Join("→", path.Select(VName));
                Log($"a → {VName(j)} : {sPath}");
            }
        }

        // ----------------- Події кнопок побудови -----------------
        private void BtnDirected_Click(object sender, EventArgs e) => CreateDirectedGraph();
        private void BtnUndirected_Click(object sender, EventArgs e) => CreateUndirectedGraph();
    }
}
