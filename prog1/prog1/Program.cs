using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

class Edge
{
    public int U;
    public int V;
    public Edge(int u, int v) { U = u; V = v; }
}

class Program
{
    // P/Invoke для встановлення коду сторінки Windows-консолі
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleOutputCP(uint wCodePageID);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleCP(uint wCodePageID);

    static void Main()
    {
        // Спроба переключити консоль на UTF-8 (щоб українські літери відображались)
        try
        {
            SetConsoleOutputCP(65001);
            SetConsoleCP(65001);

            // Через reflection — щоб уникнути проблем при старих таргетах, де властивість може не бути доступна на етапі компіляції
            var propOut = typeof(Console).GetProperty("OutputEncoding", BindingFlags.Static | BindingFlags.Public);
            var propIn = typeof(Console).GetProperty("InputEncoding", BindingFlags.Static | BindingFlags.Public);
            if (propOut != null && propOut.CanWrite) propOut.SetValue(null, Encoding.UTF8, null);
            if (propIn != null && propIn.CanWrite) propIn.SetValue(null, Encoding.UTF8, null);
        }
        catch
        {
            // ігноруємо — програма й так працюватиме, можливо консоль відобразить кирилицю неправильно
        }

        string[] vertices = { "a", "b", "c", "d", "e", "f" };

        // Неорієнтований граф (приклад)
        int[,] adjMatrixUndirected = {
            {0,1,1,0,1,0},
            {1,0,0,1,1,0},
            {1,0,0,1,1,1},
            {0,1,1,0,0,1},
            {1,1,1,0,0,0},
            {0,0,1,1,0,0}
        };

        // Орієнтований граф (приклад)
        int[,] adjMatrixDirected = {
            {0,0,0,0,0,1},
            {0,0,0,1,0,0},
            {1,0,0,1,1,0},
            {0,0,0,0,0,0},
            {1,0,0,0,0,0},
            {0,1,0,0,0,1}
        };

        SolveGraph(adjMatrixUndirected, vertices, false, "НЕОРІЄНТОВАНИЙ ГРАФ");
        SolveGraph(adjMatrixDirected, vertices, true, "ОРІЄНТОВАНИЙ ГРАФ");
    }

    static void SolveGraph(int[,] adjMatrix, string[] vertices, bool directed, string graphType)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 10) + " " + graphType + " " + new string('=', 10));
        Console.WriteLine();

        // ========== Завдання 1 ==========
        Console.WriteLine("--- ЗАВДАННЯ 1 ---\n");

        // 1а) Матриця суміжності
        PrintMatrix(adjMatrix, "1а) Матриця суміжності:");

        // 1б) Матриця інцидентності
        int[,] incidence = BuildIncidenceMatrix(adjMatrix, directed);
        PrintMatrix(incidence, "1б) Матриця інцидентності:");

        // 1в) Список ребер
        var edges = BuildEdgeListFromIncidence(incidence, directed);
        PrintEdgeList(edges, vertices, "1в) Список ребер:");

        // 1г) Список суміжності
        var adjList = BuildAdjacencyList(adjMatrix);
        PrintAdjacencyList(adjList, vertices, "1г) Список суміжності:");

        // ========== Завдання 2 ==========
        Console.WriteLine("--- ЗАВДАННЯ 2 ---\n");

        // 2а) За матрицею суміжності -> матриця інцидентності
        int[,] incFromAdj = BuildIncidenceMatrix(adjMatrix, directed);
        PrintMatrix(incFromAdj, "2а) З матриці суміжності -> матриця інцидентності:");

        // 2б) За матрицею інцидентності -> список ребер
        var edgesFromInc = BuildEdgeListFromIncidence(incFromAdj, directed);
        PrintEdgeList(edgesFromInc, vertices, "2б) З матриці інцидентності -> список ребер:");

        // 2в) За матрицею суміжності -> список суміжності
        var adjListFromAdj = BuildAdjacencyList(adjMatrix);
        PrintAdjacencyList(adjListFromAdj, vertices, "2в) З матриці суміжності -> список суміжності:");

        // 2г) За матрицею інцидентності -> матриця суміжності
        var adjFromInc = BuildAdjacencyMatrixFromIncidence(incFromAdj, directed);
        PrintMatrix(adjFromInc, "2г) З матриці інцидентності -> матриця суміжності:");

        // 2д) За матрицею суміжності -> список ребер
        var edgesFromAdj = BuildEdgeListFromAdjacency(adjMatrix, directed);
        PrintEdgeList(edgesFromAdj, vertices, "2д) З матриці суміжності -> список ребер:");

        // 2е) За матрицею інцидентності -> список суміжності
        var adjListFromInc = BuildAdjacencyList(adjFromInc);
        PrintAdjacencyList(adjListFromInc, vertices, "2е) З матриці інцидентності -> список суміжності:");
    }

    // ---------- Вивід ----------
    static void PrintMatrix(int[,] matrix, string title)
    {
        Console.WriteLine(title);
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                Console.Write(matrix[i, j] + " ");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static void PrintEdgeList(List<Edge> edges, string[] vertices, string title)
    {
        Console.WriteLine(title);
        if (edges.Count == 0) { Console.WriteLine("(порожній список)"); }
        foreach (var e in edges)
        {
            Console.WriteLine($"{vertices[e.U]} - {vertices[e.V]}");
        }
        Console.WriteLine();
    }

    static void PrintAdjacencyList(List<int>[] adj, string[] vertices, string title)
    {
        Console.WriteLine(title);
        for (int i = 0; i < adj.Length; i++)
        {
            Console.Write($"{vertices[i]}: ");
            if (adj[i].Count == 0) Console.WriteLine("(немає сусідів)");
            else Console.WriteLine(string.Join(", ", adj[i].ConvertAll(v => vertices[v])));
        }
        Console.WriteLine();
    }

    // ---------- Перетворення ----------
    static int[,] BuildIncidenceMatrix(int[,] adjMatrix, bool directed)
    {
        int n = adjMatrix.GetLength(0);
        var edges = new List<Edge>();

        for (int i = 0; i < n; i++)
        {
            int jStart = directed ? 0 : i;
            for (int j = jStart; j < n; j++)
            {
                int multiplicity = adjMatrix[i, j];
                for (int k = 0; k < multiplicity; k++)
                {
                    edges.Add(new Edge(i, j));
                }
            }
        }

        int[,] incidence = new int[n, edges.Count];
        for (int e = 0; e < edges.Count; e++)
        {
            int u = edges[e].U;
            int v = edges[e].V;
            if (!directed)
            {
                if (u == v) incidence[u, e] = 2; // петля
                else
                {
                    incidence[u, e] = 1;
                    incidence[v, e] = 1;
                }
            }
            else
            {
                if (u == v) incidence[u, e] = 2;
                else
                {
                    incidence[u, e] = -1; // вихід
                    incidence[v, e] = 1;  // вхід
                }
            }
        }

        return incidence;
    }

    static List<Edge> BuildEdgeListFromIncidence(int[,] incidence, bool directed)
    {
        int n = incidence.GetLength(0);
        int m = incidence.GetLength(1);
        var edges = new List<Edge>();

        for (int e = 0; e < m; e++)
        {
            int u = -1, v = -1;
            for (int i = 0; i < n; i++)
            {
                int val = incidence[i, e];
                if (!directed)
                {
                    if (val == 1)
                    {
                        if (u == -1) u = i;
                        else v = i;
                    }
                    else if (val == 2) { u = i; v = i; }
                }
                else
                {
                    if (val == -1) u = i;
                    else if (val == 1) v = i;
                    else if (val == 2) { u = i; v = i; }
                }
            }
            if (u != -1 && v != -1)
                edges.Add(new Edge(u, v));
        }

        return edges;
    }

    static List<int>[] BuildAdjacencyList(int[,] adjMatrix)
    {
        int n = adjMatrix.GetLength(0);
        var adj = new List<int>[n];
        for (int i = 0; i < n; i++) adj[i] = new List<int>();

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                for (int k = 0; k < adjMatrix[i, j]; k++)
                    adj[i].Add(j);

        return adj;
    }

    static int[,] BuildAdjacencyMatrixFromIncidence(int[,] incidence, bool directed)
    {
        var edges = BuildEdgeListFromIncidence(incidence, directed);
        int n = incidence.GetLength(0);
        int[,] adj = new int[n, n];
        foreach (var e in edges)
        {
            adj[e.U, e.V]++;
            if (!directed && e.U != e.V) adj[e.V, e.U]++;
        }
        return adj;
    }

    static List<Edge> BuildEdgeListFromAdjacency(int[,] adjMatrix, bool directed)
    {
        int n = adjMatrix.GetLength(0);
        var edges = new List<Edge>();
        for (int i = 0; i < n; i++)
        {
            int jStart = directed ? 0 : i;
            for (int j = jStart; j < n; j++)
            {
                int multiplicity = adjMatrix[i, j];
                for (int k = 0; k < multiplicity; k++)
                    edges.Add(new Edge(i, j));
            }
        }
        return edges;
    }
}
