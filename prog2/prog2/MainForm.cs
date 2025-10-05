using System;
using System.Collections.Generic;
using System.Drawing;
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
            this.Text = "DFS / BFS Visualization";
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
            g.DrawString(v.Name, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, v.Position.X - 8, v.Position.Y - 8);
        }

        private void DrawEdge(Vertex from, Vertex to, bool directed)
        {
            if (from == to && directed) // self-loop
            {
                DrawSelfLoop(from);
                return;
            }

            // Коротша лінія, щоб не “влазила” в кола
            var dx = to.Position.X - from.Position.X;
            var dy = to.Position.Y - from.Position.Y;
            var len = Math.Sqrt(dx * dx + dy * dy);
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

            if (directed)
            {
                using (Pen p = new Pen(Color.Black, 2))
                {
                    p.CustomEndCap = new AdjustableArrowCap(8, 8, true);
                    g.DrawLine(p, start, end);
                }
            }
            else
            {
                using (Pen p = new Pen(Color.Black, 2))
                    g.DrawLine(p, start, end);
            }
        }

        private void DrawSelfLoop(Vertex v)
        {
            int r = 20; // радіус петлі
            using (Pen p = new Pen(Color.Black, 2))
            {
                p.CustomEndCap = new AdjustableArrowCap(6, 6, true);
                Rectangle loopRect = new Rectangle(
                    v.Position.X + Radius - r,
                    v.Position.Y - Radius - r,
                    r * 2,
                    r * 2
                );
                g.DrawArc(p, loopRect, 180, 270); // малюємо частину кола
            }
        }

        private void DrawGraph()
        {
            g.Clear(Color.White);
            if (graph == null) return;

            // Спочатку малюємо ребра
            foreach (var e in graph.Edges)
                DrawEdge(graph.Vertices[e.From], graph.Vertices[e.To], graph.Directed);

            // Потім вершини
            foreach (var v in graph.Vertices)
                DrawVertex(v, Brushes.LightGray);
        }

        // ----------------- Побудова графів -----------------
        private void CreateDirectedGraph()
        {
            graph = new Graph(true);

            // Координати як на зображенні
            graph.AddVertex("a", 200, 350);
            graph.AddVertex("b", 450, 350);
            graph.AddVertex("c", 150, 200);
            graph.AddVertex("d", 400, 200);
            graph.AddVertex("e", 275, 250);
            graph.AddVertex("f", 325, 430);

            // Ребра з першого графіка
            graph.AddEdge(2, 0); // c → a
            graph.AddEdge(2, 3); // c → d
            graph.AddEdge(2, 4); // c → e
            graph.AddEdge(3, 4); // d → e
            graph.AddEdge(3, 1); // d → b
            graph.AddEdge(0, 5); // a → f
            graph.AddEdge(1, 5); // b → f
            graph.AddEdge(5, 1); // f → b (зворотна)
            graph.AddEdge(0, 0); // a → a (self-loop)
            graph.AddEdge(1, 1); // b → b (self-loop)

            DrawGraph();
        }

        private void CreateUndirectedGraph()
        {
            graph = new Graph(false);

            // Координати як на другому зображенні
            graph.AddVertex("a", 200, 200);
            graph.AddVertex("b", 400, 200);
            graph.AddVertex("c", 150, 320);
            graph.AddVertex("d", 450, 320);
            graph.AddVertex("e", 300, 250);
            graph.AddVertex("f", 300, 420);

            // Ребра як на другій схемі
            graph.AddEdge(0, 1); // a-b
            graph.AddEdge(0, 2); // a-c
            graph.AddEdge(0, 4); // a-e
            graph.AddEdge(1, 4); // b-e
            graph.AddEdge(1, 3); // b-d
            graph.AddEdge(2, 5); // c-f
            graph.AddEdge(3, 5); // d-f

            DrawGraph();
        }

        // ----------------- Алгоритми -----------------
        private async void BtnDFS_Click(object sender, EventArgs e)
        {
            if (graph == null) return;
            graph.ResetVisited();
            List<int> stack = new List<int> { 0 };

            while (stack.Count > 0)
            {
                int v = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                if (graph.Vertices[v].Visited) continue;
                graph.Vertices[v].Visited = true;

                DrawGraph();
                DrawVertex(graph.Vertices[v], Brushes.LightGreen);
                await Task.Delay(800);

                foreach (int u in graph.GetAdjList(v))
                    if (!graph.Vertices[u].Visited)
                        stack.Add(u);
            }
        }

        private async void BtnBFS_Click(object sender, EventArgs e)
        {
            if (graph == null) return;
            graph.ResetVisited();
            Queue<int> q = new Queue<int>();
            q.Enqueue(0);
            graph.Vertices[0].Visited = true;

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                DrawGraph();
                DrawVertex(graph.Vertices[v], Brushes.LightBlue);
                await Task.Delay(800);

                foreach (int u in graph.GetAdjList(v))
                {
                    if (!graph.Vertices[u].Visited)
                    {
                        graph.Vertices[u].Visited = true;
                        q.Enqueue(u);
                    }
                }
            }
        }

        // ----------------- Події кнопок -----------------
        private void BtnDirected_Click(object sender, EventArgs e) => CreateDirectedGraph();
        private void BtnUndirected_Click(object sender, EventArgs e) => CreateUndirectedGraph();
    }
}
