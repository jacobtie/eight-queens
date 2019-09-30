using System;
using System.Text;

namespace hill_climbing_eight_queens
{
    public sealed class Board
    {
        private Node[,] gameBoard;
        private const int BOARD_SIZE = 8;
        public bool GoalState { get; set; }

        public Board()
        {
            gameBoard = new Node[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    gameBoard[i,j] = new Node();
                }
            }
            GoalState = false;
        }

        public static int BoardSize()
        {
            return BOARD_SIZE;
        }

        public Node this[int i, int j]
        {
            get => gameBoard[i,j];
        }

        public string GetBoardAsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    sb.Append(gameBoard[j,i].HasQueen ? "   1" : "   0");
                }
                sb.AppendLine();
            }

            sb.AppendLine();

            return sb.ToString();
        }

        public static Board BuildRandomBoard()
        {
            var newBoard = new Board();
            var rand = new Random();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                var retry = false;
                do
                {
                    var xPos = rand.Next(8);
                    var yPos = rand.Next(8);

                    if (newBoard[xPos, yPos].HasQueen)
                    {
                        retry = true;
                    }
                    else
                    {
                        newBoard[xPos, yPos].HasQueen = true;
                        retry = false;
                    }
                } while (retry);
            }

            return newBoard;
        }

        public static Board CloneFromBoard(Board currentState)
        {
            var newBoard = new Board();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    newBoard[i,j].HasQueen = currentState[i,j].HasQueen;
                }
            }

            return newBoard;
        }
    }
}
