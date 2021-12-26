using System;
using System.Collections.Generic;
using Puzzle.Models;

namespace Puzzle
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var startingState = new int[] { 0, 3, 5, 6, 2, 4, 7, 8, 1 };

            Game game = new Game(startingState);
            var depth = 0;
            Dictionary<int[], bool> explored = new Dictionary<int[], bool>();
            Frontier frontier = new Frontier();
            Node nextNode = null;
            Node final = null;

            var searchCost = 0;

            while (!(final != null && final.totalCost < frontier.First().totalCost))
            {
                searchCost++;
                var nextMoves = game.GetPossibleMoves();
                foreach (var move in nextMoves)
                {
                    frontier.Add(new Node(nextNode, move, H1(move), depth + 1));
                }

                nextNode = frontier.Pop();

                while (explored.ContainsKey(nextNode.gameState))
                    nextNode = frontier.Pop();

                explored[nextNode.gameState] = true;
                depth = nextNode.depth;
                game.NextState(nextNode.gameState);

                if (game.isFinalState)
                {
                    final = nextNode;
                }
            }

            PrintNodes(final, startingState);
            Console.WriteLine("Search Cost: {0}", searchCost);
        }
        private static int H1(int[] gameState)
        {
            int value = 0;

            for (int i = 0; i < gameState.Length; i++)
                value += gameState[i] == i ? 0 : 1;

            return value;
        }
        private static void PrintNodes(Node finalNode, int[] startingState)
        {
            Node node = finalNode;
            Stack<Node> nodes = new Stack<Node>();

            while (node != null)
            {
                nodes.Push(node);
                node = node.parent;
            }

            Game.PrintGameState(startingState);
            Console.WriteLine();

            var nodeCount = nodes.Count;
            for (int i = 1; i <= nodeCount; i++)
            {
                Console.WriteLine("\nStep {0}:", i);
                Game.PrintGameState(nodes.Pop().gameState);
            }
        }
    }
}
