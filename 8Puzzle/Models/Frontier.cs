using System.Collections.Generic;

namespace Puzzle.Models
{
    public class Frontier
    {
        List<Node> nodes;

        public int Count => nodes.Count;

        public Frontier()
        {
            nodes = new List<Node>();
        }

        public void Add(Node node)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].totalCost > node.totalCost)
                {
                    nodes.Insert(i, node);
                    return;
                }
            }
            nodes.Add(node);
        }
        public Node Pop()
        {
            var node = nodes[0];
            nodes.RemoveAt(index: 0);
            return node;
        }
        public Node First()
        {
            return nodes[0];
        }
    }
}
