using System;
namespace Puzzle
{
    public class Game
    {
        int[] gameState;
        public int[] currentState => gameState;
        public bool isFinalState {
            get
            {
                for (int i = 0; i < gameState.Length; i++)
                    if (i != gameState[i])
                        return false;
                return true;
            }
        }

        public Game(int[] gameState)
        {
            this.gameState = gameState;
        }
        public void NextState(int[] gameState) {
            this.gameState = gameState;
        }
        public int[][] GetPossibleMoves()
        {
            //find 0
            int location = Find0();

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

            var availableMoves = moveIndex[location];

            var possibleMoves = new int[availableMoves.Length][];

            for (int i = 0; i < availableMoves.Length; i++)
            {
                var move = availableMoves[i];
                possibleMoves[i] = SwapWithEmpty(location, move);
            }

            return possibleMoves;
        }
        private int[] SwapWithEmpty(int emptyTile, int index)
        {
            int[] newGameState = new int[9];

            for (int i = 0; i < gameState.Length; i++)
                newGameState[i] = gameState[i];

            int tmp = newGameState[index];
            newGameState[index] = newGameState[emptyTile];
            newGameState[emptyTile] = tmp;

            return newGameState;
        }
        private int Find0()
        {
            for (int i = 0; i < gameState.Length; i++)
            {
                if (gameState[i] == 0)
                    return i;
            }

            return -1;
        }

        public static void PrintGameState(int[] gameState)
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
