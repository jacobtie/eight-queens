using System;
using System.Text;

namespace hill_climbing_eight_queens
{
    public sealed class Board : IEquatable<Board>
    {
        // Fields to contain information about the board
        public QueenPosition[] queens;
        private int boardSize;
        public bool GoalState { get; set; }

        // Constructor to create a board with a custom size
        public Board(int size)
        {
            // Set the board size of the object
            boardSize = size;

            // Create a new list of queens
            queens = new QueenPosition[boardSize];

            // For each queen on the board
            for (int i = 0; i < boardSize; i++)
            {
                // Create a new queen position
                queens[i] = new QueenPosition();
            }

            // Set the goal state equal to false
            GoalState = false;
        }

        // Method to return the board size
        public int BoardSize()
        {
            return boardSize;
        }

        // Method to simplify the referencing each queen in the list of queens
        public QueenPosition this[int i]
        {
            get => queens[i];
        }

        // Method to print the board as a string for output
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

        // Method to build a random board with the given size
        public static Board BuildRandomBoard(int size)
        {
            // Create a new board of the given size
            var newBoard = new Board(size);

            // Create a variable to generate a random number
            var rand = new Random();

            // For each queen on the board
            for (int i = 0; i < size; i++)
            {
                // Set the column of the current queen equal to a random number on the given row
                var col = rand.Next(size);
                newBoard.queens[i] = new QueenPosition(i, col);
            }

            // Return the random board
            return newBoard;
        }

        // Method to clone an old board and return a new board
        public static Board CloneFromBoard(Board currentState)
        {
            // Create a new board with the same size
            var newBoard = new Board(currentState.BoardSize());

            // For each queen on the new board
            for (int i = 0; i < newBoard.BoardSize(); i++)
            {
                // Set the position of each queen based on the old board
                newBoard[i].X = currentState[i].X;
                newBoard[i].Y = currentState[i].Y;
            }

            // Return the new board
            return newBoard;
        }

        // Override method for the IEquatable interface
        public bool Equals(Board other)
        {
            // For each queen on the board
            for (int i = 0; i < queens.Length; i++)
            {
                // If each of the current queens are not in the same column
                if (queens[i].Y != other.queens[i].Y)
                {
                    // Return false
                    return false;
                }
            }

            // Return true
            return true;
        }
    }
}
