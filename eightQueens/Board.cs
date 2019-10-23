using System;
using System.Text;

namespace hill_climbing_eight_queens
{
    public sealed class Board : IEquatable<Board>
    {
        public QueenPosition[] queens;
        private int boardSize;
        public bool GoalState { get; set; }

        public Board(int size)
        {
            boardSize = size;
            queens = new QueenPosition[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                queens[i] = new QueenPosition();
            }

            GoalState = false;
        }

        public int BoardSize()
        {
            return boardSize;
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
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    isQueen = false;

                    for (int k = 0; k < boardSize; k++)
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

        public static Board BuildRandomBoard(int size)
        {
            var newBoard = new Board(size);
            var rand = new Random();

            for (int i = 0; i < size; i++)
            {
                var col = rand.Next(size);
                newBoard.queens[i] = new QueenPosition(i, col);
            }

            return newBoard;
        }

        public static Board CloneFromBoard(Board currentState)
        {
            var newBoard = new Board(currentState.BoardSize());

            for (int i = 0; i < newBoard.BoardSize(); i++)
            {
                newBoard[i].X = currentState[i].X;
                newBoard[i].Y = currentState[i].Y;
            }

            return newBoard;
        }

        // override object.Equals
        public bool Equals(Board other)
        {
            for (int i = 0; i < queens.Length; i++)
            {
                if (queens[i].Y != other.queens[i].Y)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
