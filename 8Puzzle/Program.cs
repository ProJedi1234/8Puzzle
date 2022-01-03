using System;
using System.Collections.Generic;
using Puzzle.Models;

namespace Puzzle
{
    class MainClass
    {
        public enum HFunction { MisplacedTiles, ManhattanDistance };

        public static void Main(string[] args)
        {
            var option = Menu.ShowMenu();

            var startingState = new int[] { 0, 3, 5, 6, 2, 4, 7, 8, 1 };

            if (option.testType == TestType.singleTestPuzzle)
            {
                var watch1 = new System.Diagnostics.Stopwatch();

                watch1.Start();
                doSearch(option.gameState[0], HFunction.MisplacedTiles, true);
                watch1.Stop();

                var watch2 = new System.Diagnostics.Stopwatch();
                watch2.Start();
                doSearch(option.gameState[0], HFunction.ManhattanDistance, false);
                watch2.Stop();

                Console.WriteLine($"H1 Execution Time: {watch1.Elapsed} ms");
                Console.WriteLine($"H2 Execution Time: {watch2.Elapsed} ms");
            } else if (option.testType == TestType.multiTestPuzzle)
            {
                var results1 = new Dictionary<int, List<double>>();
                var results2 = new Dictionary<int, List<double>>();

                for (int i = 0; i < option.gameState.Length; i++)
                {
                    var watch1 = new System.Diagnostics.Stopwatch();
                    
                    watch1.Start();
                    doSearch(option.gameState[i], HFunction.MisplacedTiles, false);
                    watch1.Stop();

                    var watch2 = new System.Diagnostics.Stopwatch();

                    watch2.Start();
                    var finalNode = doSearch(option.gameState[i], HFunction.ManhattanDistance, false);
                    watch2.Stop();

                    Console.WriteLine($"H1 Execution Time: {watch1.Elapsed} ms");
                    Console.WriteLine($"H2 Execution Time: {watch2.Elapsed} ms");

                    var nodeCount = CountSolutionSteps(finalNode);
                    Console.WriteLine("Size: {0}", nodeCount);

                    if (!results1.ContainsKey(nodeCount))
                    {
                        results1[nodeCount] = new List<double>();
                        results2[nodeCount] = new List<double>();
                    }

                    results1[nodeCount].Add(watch1.Elapsed.TotalMilliseconds);
                    results2[nodeCount].Add(watch2.Elapsed.TotalMilliseconds);
                }

                //load into CSV
                var csvFile = "";
                foreach (var key in results1.Keys)
                {
                    double total1 = 0;
                    foreach (var item in results1[key])
                        total1 += item;
                    var average1 = total1 / results1[key].Count;

                    double total2 = 0;
                    foreach (var item in results2[key])
                        total2 += item;
                    var average2 = total2 / results2[key].Count;

                    csvFile += (key + "," + average1 + "," + average2 + "\n");
                }

                System.IO.File.WriteAllText("MultiRun.csv", csvFile);
            } else
            {
                Environment.Exit(0);
            }
        }
        private static Node doSearch(int[] startingState, HFunction heuristic, bool printResults)
        {
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
                    if (heuristic == HFunction.MisplacedTiles)
                        frontier.Add(new Node(nextNode, move, H1(move), depth + 1));
                     else
                        frontier.Add(new Node(nextNode, move, H2(move), depth + 1));
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

            if (printResults)
            {
                PrintNodes(final, startingState);
                Console.WriteLine();
            }
            Console.WriteLine("{0}: {1}", heuristic == HFunction.MisplacedTiles ? "H1" : "H2", searchCost);

            return final;
        }
        private static int H1(int[] gameState)
        {
            int value = 0;

            for (int i = 0; i < gameState.Length; i++)
                value += gameState[i] == i ? 0 : 1;

            return value;
        }
        private static int H2(int[] gameState)
        {
            int totalDistance = 0;
            for (int i = 0; i < gameState.Length; i++)
            {
                var v = gameState[i];

                Point goal = new Point() { x = i % 3, y = i / 3 };
                Point node = new Point() { x = v % 3, y = v / 3 };

                var dx = Math.Abs(goal.x - node.x);
                var dy = Math.Abs(goal.y - node.y);

                totalDistance += dx + dy;
            }

            return totalDistance;
        }
        private static int CountSolutionSteps(Node finalNode)
        {
            int steps = 0;
            Node node = finalNode;
            while (node != null)
            {
                steps++;
                node = node.parent;
            }

            return steps;
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
            Console.WriteLine("Starting:");
            Game.PrintGameState(startingState);

            var nodeCount = nodes.Count;
            for (int i = 1; i <= nodeCount; i++)
            {
                Console.WriteLine("\nStep {0}:", i);
                Game.PrintGameState(nodes.Pop().gameState);
            }
        }
    }
}
