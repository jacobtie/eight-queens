namespace hill_climbing_eight_queens
{
    public sealed class Node
    {
        public bool HasQueen { get; set; }

        public Node()
        {
            HasQueen = false;
        }
    }
}
