using System.Drawing;

namespace prog2
{
    public class Vertex
    {
        public string Name { get; set; }
        public Point Position { get; set; }
        public bool Visited { get; set; }

        public Vertex(string name, int x, int y)
        {
            Name = name;
            Position = new Point(x, y);
            Visited = false;
        }
    }
}
