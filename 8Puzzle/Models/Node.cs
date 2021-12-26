using System;

namespace Puzzle.Models
{
    public class Node
    {
        Node parent;
        int[] gameState;
        int cost;
        int depth;

        public Node(ref Node parent, int[] gameState, int cost, int depth)
        {
            this.parent = parent;
            this.gameState = gameState;
            this.cost = cost;
            this.depth = depth;
        }
    }
}
