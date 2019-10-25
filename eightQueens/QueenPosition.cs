namespace hill_climbing_eight_queens
{
    public class QueenPosition
    {
        // Fields to store the position of the queen
        public int X { get; set; }
        public int Y { get; set; }

        // Default constructor
        public QueenPosition(){}

        // Overloaded constructor to set the row and column of the queen
        public QueenPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}