using System;
using Puzzle.Models;

namespace Puzzle
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var startingState = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            MakeMove(startingState);
        }
        private static void MakeMove(int[] gameState)
        {
            //find 0
            int location = Find0(gameState);

            int[][] moveIndex = new int[9][];
            moveIndex[0] = new int[] { 1, 3 };
            moveIndex[1] = new int[] { 0, 2, 4 };
            moveIndex[2] = new int[] { 1, 5 };
            moveIndex[3] = new int[] { 4, 0, 6 };
            moveIndex[4] = new int[] { 3, 5, 1, 7 };
            moveIndex[5] = new int[] { 4, 2, 8 };
            moveIndex[6] = new int[] { 7, 3 };
            moveIndex[7] = new int[] { 6, 8, 4 };
            moveIndex[8] = new int[] { 7, 5 };

            PrintGameState(gameState);
            Console.WriteLine();

            var availableMoves = moveIndex[location];
            foreach (var move in availableMoves)
            {
                int[] newState = SwapWithEmpty(gameState, location, move);
                PrintGameState(newState);
                Console.WriteLine();
            }
        }
        private static int[] SwapWithEmpty(int[] gameState, int emptyTile, int index)
        {
            int[] newGameState = new int[9];

            for (int i = 0; i < gameState.Length; i++)
                newGameState[i] = gameState[i];

            int tmp = newGameState[index];
            newGameState[index] = newGameState[emptyTile];
            newGameState[emptyTile] = tmp;

            return newGameState;
        }
        private static int Find0(int[] gameState)
        {
            for (int i = 0; i < gameState.Length; i++)
            {
                if (gameState[i] == 0)
                    return i;
            }

            return -1;
        }
        private static int H1(int[] gameState)
        {
            int value = 0;

            for (int i = 0; i < gameState.Length; i++)
            {
                value += gameState[i] == i ? 0 : 1;
            }

            return value;
        }
        private static void PrintGameState(int[] gameState)
        {
            for (int i = 0; i < gameState.Length; i++)
            {
                if (i != 0 && i % 3 == 0)
                    Console.WriteLine();
                Console.Write("{0} ", gameState[i]);
            }
            Console.WriteLine();
        }
    }
}
