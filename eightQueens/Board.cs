using System;
using System.Text;

namespace hill_climbing_eight_queens
{
    public sealed class Board
    {
        public QueenPosition[] queens;
        private const int BOARD_SIZE = 8;
        public bool GoalState { get; set; }

        public Board()
        {
            queens = new QueenPosition[BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                queens[i] = new QueenPosition();
            }

            GoalState = false;
        }

        public static int BoardSize()
        {
            return BOARD_SIZE;
        }

        public QueenPosition this[int i]
        {
            get => queens[i];
        }

        public string GetBoardAsString()
        {
            StringBuilder sb = new StringBuilder();
            Boolean isQueen;

            sb.AppendLine();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    isQueen = false;

                    for (int k = 0; k < BOARD_SIZE; k++)
                    {
                        if (queens[k].X == i && queens[k].Y == j)
                        {
                            isQueen = true;
                            break;
                        }
                    }

                    sb.Append(isQueen ? " 1" : " 0");
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
                var col = rand.Next(8);
                newBoard.queens[i] = new QueenPosition(i, col);
            }

            return newBoard;
        }

        public static Board CloneFromBoard(Board currentState)
        {
            var newBoard = new Board();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                newBoard[i].X = currentState[i].X;
                newBoard[i].Y = currentState[i].Y;
            }

            return newBoard;
        }
    }
}
