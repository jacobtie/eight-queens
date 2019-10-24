using System;
using System.Linq;
using System.Collections.Generic;

namespace hill_climbing_eight_queens
{
    public static class HillClimbing
    {
        public static void RunHillClimbing(bool doesRestart, bool movesSideways, int boardSize)
        {
            var retry = false;
            var restarts = 0;

            do
            {
                var gameBoard = Board.BuildRandomBoard(boardSize);

                var boards = ClimbHill(gameBoard, movesSideways);

                Logger.WriteLine("\nStarting Board Configuration: ");
                Logger.WriteLine($"Heuristic: {HillClimbing.GetHeuristicFromBoard(boards[0])}");
                Logger.Write(boards[0].GetBoardAsString());

                for (int i = 0; i < (boardSize * 2) + 5 || i < 21; i++)
                {
                    Logger.Write("-");
                }

                Logger.WriteLine("\n");

                for (int i = 1; i < boards.Count; i++)
                {
                    int currH = HillClimbing.GetHeuristicFromBoard(boards[i]);

                    Logger.WriteLine($"Board Configuration {i+1}");
                    Logger.WriteLine($"Heuristic: {currH}");

                    if (currH == HillClimbing.GetHeuristicFromBoard(boards[i-1]))
                    {
                        Logger.WriteLine("Moved Sideways. ");
                    }

                    Logger.Write(boards[i].GetBoardAsString());

                    for (int k = 0; k < (boardSize * 2) + 5 || k < 21; k++)
                    {
                        Logger.Write("-");
                    }

                    Logger.WriteLine("\n");
                }

                if (boards.Last().GoalState)
                {
                    retry = false;

                    Logger.WriteLine($"Success! This iteration took {boards.Count-1} moves. ");

                    if (doesRestart)
                    {
                        Logger.Write($"Restarted {restarts} time");

                        if (restarts != 1)
                        {
                            Logger.WriteLine("s.");
                        }
                        else
                        {
                            Logger.WriteLine(".");
                        }
                    }
                    else
                    {
                        Logger.WriteLine();
                    }
                }
                else
                {
                    retry = true;
                    restarts++;
                    Logger.WriteLine($"Failed. This iteration took {boards.Count-1} moves");

                    if (doesRestart)
                    {
                        Logger.WriteLine("Restarting now.");
                    }
                    else
                    {
                        Logger.WriteLine();
                    }
                }
            } 
            while (retry && doesRestart);
        }

        public static int GetHeuristicFromBoard(Board gameBoard)
        {
            var boardSize = gameBoard.BoardSize();
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

                    if (Math.Abs(gameBoard[i].X - gameBoard[k].X) == 
                        Math.Abs(gameBoard[i].Y - gameBoard[k].Y))
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
                for (int j = 0; j < currentState.BoardSize(); j++)
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

        public static List<Board> ClimbHill(Board startingState, bool movesSideways)
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

            var prevState = new Board(startingState.BoardSize());

            while (true)
            {
                Board nextState = null;
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
                        nextState = successor;
                    }

                    if (foundHeuristic == currHeuristic)
                    {
                        equalBoards.Add(successor);
                    }
                }

                // Check if heuristic is 0
                if (nextHeuristic == 0)
                {
                    nextState.GoalState = true;
                    intermediateBoards.Add(nextState);
                    break;
                }

                // Make sure the heuristic actually is lower
                if (nextHeuristic >= currHeuristic)
                {
                    if (nextHeuristic == currHeuristic && movesSideways)
                    {
                        nextState = equalBoards[rand.Next(equalBoards.Count)];
                    }
                    else
                    {
                        break;
                    }
                }

                // Loop
                prevState = currentState;
                currentState = nextState;
                intermediateBoards.Add(currentState);
            }

            return intermediateBoards;
        }
    }
}