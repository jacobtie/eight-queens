using System;
using System.Collections.Generic;

namespace hill_climbing_eight_queens
{
    public static class HillClimbing
    {
        public static int GetHeuristicFromBoard(Board gameBoard)
        {
            var boardSize = Board.BoardSize();
            int heuristic = 0;

            for (int i = 0; i < boardSize; i++)
            {
                // Check for queens left and right / up and down using same iterator
                for (int k = i + 1; k < boardSize; k++)
                {
                    if (gameBoard[i].Y == gameBoard[k].Y)
                    {
                        heuristic++;
                    }

                    if (Math.Abs(gameBoard[i].X - gameBoard[k].X) == Math.Abs(gameBoard[i].Y - gameBoard[k].Y))
                    {
                        heuristic++;
                    }
                }
            }

            return heuristic;
        }

        public static List<Board> GenerateNeighbors(Board currentState)
        {
            var successors = new List<Board>();
            
            for (int i = 0; i < currentState.queens.Length; i++)
            {
                for (int j = 0; j < Board.BoardSize(); j++)
                {
                    if (currentState.queens[i].Y != j)
                    {
                        var newBoard = Board.CloneFromBoard(currentState);
                        newBoard.queens[i].Y = j;
                        successors.Add(newBoard);
                    }
                }
            }

            return successors;
        }

        public static List<Board> ClimbHill(Board startingState)
        {
            var startingHeuristic = GetHeuristicFromBoard(startingState);
            var intermediateBoards = new List<Board>{startingState};
            var currentState = startingState;
            Random rand = new Random();

            if (startingHeuristic == 0)
            {
                startingState.GoalState = true;
                return intermediateBoards;
            }

            var prevState = new Board();

            while (true)
            {
                Board neighbor = null;
                var queenPositions = currentState.queens;
                var nextHeuristic = int.MaxValue;
                var successors = GenerateNeighbors(currentState);
                var equalBoards = new List<Board>();
                var currHeuristic = GetHeuristicFromBoard(currentState);

                foreach (var successor in successors)
                {
                    if (successor.Equals(prevState))
                    {
                        continue;
                    }

                    var foundHeuristic = GetHeuristicFromBoard(successor);

                    if (foundHeuristic < nextHeuristic)
                    {
                        nextHeuristic = foundHeuristic;
                        neighbor = successor;
                    }

                    if (foundHeuristic == currHeuristic)
                    {
                        equalBoards.Add(successor);
                    }
                }

                // Check if heuristic is 0
                if (nextHeuristic == 0)
                {
                    neighbor.GoalState = true;
                    intermediateBoards.Add(neighbor);
                    break;
                }

                // Make sure the heuristic actually is lower
                if (nextHeuristic > currHeuristic)
                {
                    return intermediateBoards;
                }
                else if (nextHeuristic == currHeuristic)
                {
                    neighbor = equalBoards[rand.Next(equalBoards.Count)];
                    Console.WriteLine("Moved Sideways! ");
                }

                // Loop
                prevState = currentState;
                currentState = neighbor;
                intermediateBoards.Add(currentState);
            }

            return intermediateBoards;
        }
    }
}