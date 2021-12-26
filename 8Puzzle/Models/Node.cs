using System;

namespace Puzzle.Models
{
    public class Node
    {
        public readonly Node parent;
        public readonly int[] gameState;
        public readonly int cost;
        public readonly int depth;

        public int totalCost => cost + depth;

        public Node(Node parent, int[] gameState, int cost, int depth)
        {
            this.parent = parent;
            this.gameState = gameState;
            this.cost = cost;
            this.depth = depth;
        }
    }
}
