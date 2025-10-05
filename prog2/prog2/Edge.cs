namespace prog2
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public bool Directed { get; set; }

        public Edge(int from, int to, bool directed)
        {
            From = from;
            To = to;
            Directed = directed;
        }
    }
}
