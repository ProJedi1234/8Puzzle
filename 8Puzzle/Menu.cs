using System;
using System.Collections.Generic;
using System.IO;
using Puzzle.Models;
namespace Puzzle
{
    public class Menu
    {
        public static MenuChoice ShowMenu()
        {
            string menuText = "Select:\n[1] Single Test Puzzle\n[2] Multi - Test Puzzle\n[3] Exit";
            Console.WriteLine(menuText);
            int option = 0;
            while (option < 1 || option > 3)
            {
                Console.Write("Choice: ");
                int.TryParse(Console.ReadLine(), out option);
            }
            if (option == 1)
            {
                var state = SingleTestPuzzle();
                return new MenuChoice { testType = TestType.singleTestPuzzle, gameState = new int[][] { state } };
            }
            else if (option == 2)
            {
                var states = MultiTestPuzzle();
                return new MenuChoice { testType = TestType.multiTestPuzzle, gameState = states };
            }
            else
                return new MenuChoice { testType = TestType.exit, gameState = new int[][] { } };
        }
        private static int[] SingleTestPuzzle()
        {
            string menuText = "Select Input Method:\n[1] Random\n[2] File";
            Console.WriteLine(menuText);
            int option = 0;
            while (option < 1 || option > 3)
            {
                Console.Write("Choice: ");
                int.TryParse(Console.ReadLine(), out option);
            }
            if (option == 1)
                return GenerateRandomPuzzle();
            else
                return readFromFile();
        }
        private static int[][] MultiTestPuzzle()
        {
            var items = new List<int[]>();

            for (int i = 0; i < 25; i++)
            {
                items.Add(GenerateRandomPuzzle(depth: 4));
                items.Add(GenerateRandomPuzzle(depth: 8));
                items.Add(GenerateRandomPuzzle(depth: 12));
                items.Add(GenerateRandomPuzzle(depth: 16));
                //items.Add(GenerateRandomPuzzle(depth: 20));
            }

            return items.ToArray();
        }
        private static int[] GenerateRandomPuzzle(int depth = -1)
        {
            while (depth < 2)
            {
                Console.Write("Depth: ");
                int.TryParse(Console.ReadLine(), out depth);
            }

            var gameStates = new List<List<int>>();
            var lines = File.ReadAllLines("Length" + depth + ".txt");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "" || lines[i][0] == '/')
                    continue;

                var puzzle = new List<int>();
                for (int line = 0; line < 3; line++)
                {
                    var numbers = lines[i].Split(' ');
                    for (int j = 0; j < numbers.Length; j++)
                        puzzle.Add(int.Parse(numbers[j]));
                    i++;
                }
                gameStates.Add(puzzle);
            }

            var random = new Random();
            return gameStates[random.Next(gameStates.Count)].ToArray();
        }
        private static int[] readFromFile()
        {
            string path = "";
            while (path != "" || !File.Exists(path)) { 
                Console.Write("File: ");
                path = Console.ReadLine();
                if (!File.Exists(path))
                    Console.WriteLine("File does not exist");
            }
            var gameState = new List<int>();
            var lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                var numbers = lines[i].Split(' ');
                for (int j = 0; j < numbers.Length; j++)
                    gameState.Add(int.Parse(numbers[j]));
            }

            return gameState.ToArray();
        }
    }
}
