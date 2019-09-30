using System;
using System.Collections.Generic;

namespace hill_climbing_eight_queens
{
    public static class HillClimbing
    {
        public static int GetHeuristicFromBoard(Board gameBoard)
        {
            var visited = new List<Node>();
            var boardSize = Board.BoardSize();
            int heuristic = 0;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    var current = gameBoard[i,j];
                    if (current.HasQueen)
                    {
                        visited.Add(current);
                        // Check for queens left and right / up and down using same iterator
                        for (int k = 0; k < boardSize; k++)
                        {
                            if (gameBoard[i,k].HasQueen && !visited.Contains(gameBoard[i,k]))
                            {
                                heuristic++;
                            }

                            if (gameBoard[k,j].HasQueen && !visited.Contains(gameBoard[k,j]))
                            {
                                heuristic++;
                            }
                        }

                        // Check for queens upper left
                        int upperLeftX = i - 1;
                        int upperLeftY = j - 1;
                        while (upperLeftX >= 0 && upperLeftY >= 0)
                        {
                            if (gameBoard[upperLeftX, upperLeftY].HasQueen && !visited.Contains(gameBoard[upperLeftX, upperLeftY]))
                            {
                                heuristic++;
                            }
                            upperLeftX--;
                            upperLeftY--;
                        }

                        // Check for queens upper right
                        int upperRightX = i - 1;
                        int upperRightY = j + 1;
                        while (upperRightX >= 0 && upperRightY < boardSize)
                        {
                            if (gameBoard[upperRightX, upperRightY].HasQueen && !visited.Contains(gameBoard[upperRightX, upperRightY]))
                            {
                                heuristic++;
                            }
                            upperRightX--;
                            upperRightY++;
                        }

                        // Check for queens bottom left
                        int bottomLeftX = i + 1;
                        int bottomLeftY = j - 1;
                        while (bottomLeftX < boardSize && bottomLeftY >= 0)
                        {
                            if (gameBoard[bottomLeftX, bottomLeftY].HasQueen && !visited.Contains(gameBoard[bottomLeftX, bottomLeftY]))
                            {
                                heuristic++;
                            }
                            bottomLeftX++;
                            bottomLeftY--;
                        }

                        // Check for queens bottom right
                        int bottomRightX = i + 1;
                        int bottomRightY = j + 1;
                        while (bottomRightX < boardSize && bottomRightY < boardSize)
                        {
                            if (gameBoard[bottomRightX, bottomRightY].HasQueen && !visited.Contains(gameBoard[bottomRightX, bottomRightY]))
                            {
                                heuristic++;
                            }
                            bottomRightX++;
                            bottomRightY++;
                        }
                    }
                }
            }

            return heuristic;
        }

        public static List<Board> GenerateBoardsFromQueen(Board currentState, int currentQueenX, int currentQueenY)
        {
            Board baseBoard = Board.CloneFromBoard(currentState);
            // Remove the queen from the base board
            baseBoard[currentQueenX, currentQueenY].HasQueen = false;
            var successors = new List<Board>();
            
            for (int i = 0; i < Board.BoardSize(); i++)
            {
                for (int j = 0; j < Board.BoardSize(); j++)
                {
                    if (!currentState[i,j].HasQueen)
                    {
                        var newBoard = Board.CloneFromBoard(baseBoard);
                        newBoard[i,j].HasQueen = true;
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
            var minHeuristic = startingHeuristic;

            if (startingHeuristic == 0)
            {
                startingState.GoalState = true;
                return intermediateBoards;
            }


            while (true)
            {
                Board neighbor = null;
                var queenPositions = new List<QueenPosition>();
                // This is inefficient, but I am in a hurry
                for (int i = 0; i < Board.BoardSize(); i++)
                {
                    for (int j = 0; j < Board.BoardSize(); j++)
                    {
                        if (currentState[i,j].HasQueen)
                        {
                            queenPositions.Add(new QueenPosition(i, j));
                        }
                    }
                }

                // Find the neighbor with the lowest heuristic
                var nextHeuristic = int.MaxValue;
                foreach (var queen in queenPositions)
                {
                    var successors = GenerateBoardsFromQueen(currentState, queen.X, queen.Y);
                    foreach (var successor in successors)
                    {
                        var foundHeuristic = GetHeuristicFromBoard(successor);
                        if (foundHeuristic < nextHeuristic)
                        {
                            nextHeuristic = foundHeuristic;
                            neighbor = successor;
                        }
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
                if (nextHeuristic >= minHeuristic)
                {
                    return intermediateBoards;
                }

                // Loop
                currentState = neighbor;
                intermediateBoards.Add(currentState);
                minHeuristic = nextHeuristic;
            }

            return intermediateBoards;
        }
    }
}