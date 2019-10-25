using System;
using System.Linq;
using System.Collections.Generic;

namespace hill_climbing_eight_queens
{
    public static class HillClimbing
    {
        // Method to run hill climbing and return the result
        public static HillClimbingResult RunHillClimbing(bool doesRestart, bool movesSideways, int boardSize, bool logOutput = true)
        {
            // Variable to store the results of the hill climbing
            var result = new HillClimbingResult
            {
                Type = (doesRestart, movesSideways) switch
                {
                    (false, false) => "default",
                    (false, true) => "sideways_moves",
                    (true, _) => "random_restart",
                },
                SidewaysMoves = movesSideways,
                NumRestarts = null,
                NumSteps = 0,
                Succeeded = false,
            };

            // Variables to keep track of the restarts
            var retry = false;
            var restarts = 0;

            // Do while the program fails to find a solution and random restart is enabled
            do
            {
                // Variable to store the original board
                var gameBoard = Board.BuildRandomBoard(boardSize);

                // Variable to store the different boards that were visited during hill climbing
                var boards = ClimbHill(gameBoard, movesSideways);

                // If output should be printed, print the following
                if (logOutput)
                {
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
                }

                // If the last state of the list of boards is the goal state
                if (boards.Last().GoalState)
                {
                    // Set retry to false
                    retry = false;

                    // If output should be printed, print the following
                    if (logOutput)
                    {
                        Logger.WriteLine($"Success! This iteration took {boards.Count-1} moves. ");
                    }

                    // Set the values for the returned results
                    result.Succeeded = true;
                    result.NumSteps = boards.Count - 1;

                    // If the program is set to perform random restart
                    if (doesRestart)
                    {
                        // If output should be printed, print the following
                        if (logOutput)
                        {
                            Logger.Write($"Restarted {restarts} time");
                        }

                        // Update the number of restarts
                        result.NumRestarts = restarts;

                        // If output should be printed, print the following
                        if (logOutput)
                        {
                            if (restarts != 1)
                            {
                                Logger.WriteLine("s.");
                            }
                            else
                            {
                                Logger.WriteLine(".");
                            }
                        }
                    }
                    // Else if output should be printed, print the following
                    else if (logOutput)
                    {
                        Logger.WriteLine();
                    }
                }
                // Else the last board in boards is not the goal state
                else
                {
                    // Set retry equal to true and increment the number of restarts
                    retry = true;
                    restarts++;

                    // If output should be printed, print the following
                    if (logOutput)
                    {
                        Logger.WriteLine($"Failed. This iteration took {boards.Count-1} moves");
                    }

                    // If the program is set to perform random restarts
                    if (doesRestart)
                    {
                        // If output should be printed, print the following
                        if (logOutput)
                        {
                            Logger.WriteLine("Restarting now.");
                        }
                    }
                    // Else the program is not performing random restarts
                    else
                    {
                        // Set the values of the returned result
                        result.Succeeded = false;
                        result.NumSteps = boards.Count - 1;

                        // If output should be printed, print the following
                        if (logOutput)
                        {
                            Logger.WriteLine();
                        }
                    }
                }
            } 
            while (retry && doesRestart);

            // Return the result to be logged
            return result;
        }

        // Method to calculate the heuristic based on the board
        public static int GetHeuristicFromBoard(Board gameBoard)
        {
            // Get the board size and set the heuristic to zero
            var numQueens = gameBoard.queens.Length;
            int heuristic = 0;

            // For each queen on the given board
            for (int i = 0; i < numQueens; i++)
            {
                // For each queen after the current queen
                for (int k = i + 1; k < numQueens; k++)
                {
                    // If both queens are in the same column
                    if (gameBoard[i].Y == gameBoard[k].Y)
                    {
                        // Increment the heuristic
                        heuristic++;
                    }

                    // If both queens are in the same diagonal
                    if (Math.Abs(gameBoard[i].X - gameBoard[k].X) == 
                        Math.Abs(gameBoard[i].Y - gameBoard[k].Y))
                    {
                        // Increment the heuristic
                        heuristic++;
                    }
                }
            }

            // Return the heuristic
            return heuristic;
        }

        // Method to generate the neighbors of the current board
        public static List<Board> GenerateNeighbors(Board currentState)
        {
            // List to store the neighbors of the current board
            var successors = new List<Board>();
            
            // For each queen on the current board
            for (int i = 0; i < currentState.queens.Length; i++)
            {
                // For each cell in the row of the current queen
                for (int j = 0; j < currentState.BoardSize(); j++)
                {
                    // If the queen is not already in the current column
                    if (currentState.queens[i].Y != j)
                    {
                        // Move the queen to the new space and add this state to the neighbors
                        var newBoard = Board.CloneFromBoard(currentState);
                        newBoard.queens[i].Y = j;
                        successors.Add(newBoard);
                    }
                }
            }

            // Return the successors of the current board
            return successors;
        }

        // Method to climb the hill from the starting state until either a solution is found
        // or an impass could not be overcome
        public static List<Board> ClimbHill(Board startingState, bool movesSideways)
        {
            // Variable to store the starting heuristic 
            var startingHeuristic = GetHeuristicFromBoard(startingState);

            // List to store the boards to reach the solution or impass
            var intermediateBoards = new List<Board>{startingState};

            // Variable to store the current state of the boards as moves are made
            var currentState = startingState;

            // Variable to create random numbers to choose from equal states to prevent looping
            Random rand = new Random();

            // If the starting heuristic is zero
            if (startingHeuristic == 0)
            {
                // The goal has been found, so the list can be returned
                startingState.GoalState = true;
                return intermediateBoards;
            }

            // Variable to store the previous state after each iteration
            var prevState = new Board(startingState.BoardSize());

            // Variable to store the number of sideways moves that were made
            var numSideways = 0;

            // While true
            while (true)
            {
                // Variable to store the next state with the lowest heuristic
                Board nextState = null;

                // Variable to store the list of queens on the current board
                var queenPositions = currentState.queens;

                // Variable to store the heuristic of the next state
                var nextHeuristic = int.MaxValue;

                // List to store the successors of the current board
                var successors = GenerateNeighbors(currentState);

                // List to store the boards that have the same heuristic as the current board
                var equalBoards = new List<Board>();

                // Variable to store the heuristic of the current board
                var currHeuristic = GetHeuristicFromBoard(currentState);

                // For each board in successors
                foreach (var successor in successors)
                {
                    // If the current successor is equal to the previous state
                    if (successor.Equals(prevState))
                    {
                        // Continue to the next loop
                        continue;
                    }

                    // Variable to store the heuristic of the current successor
                    var foundHeuristic = GetHeuristicFromBoard(successor);

                    // If the heuristic of the current successor is less than the current 
                    // heuristic of the next state
                    if (foundHeuristic < nextHeuristic)
                    {
                        // Set the next heuristic equal to the heuristic of the current successor
                        nextHeuristic = foundHeuristic;

                        // Set the next state equal to the current successor
                        nextState = successor;
                    }

                    // If the heuristic of the current successor is equal to the heuristic
                    // of the current board
                    if (foundHeuristic == currHeuristic)
                    {
                        // Add the current successor to the list of equal boards
                        equalBoards.Add(successor);
                    }
                }

                // If the heuristic of the next state is zero
                if (nextHeuristic == 0)
                {
                    // Set the next state as the goal state
                    nextState.GoalState = true;

                    // Add the next state to the list of boards and break from the while loop
                    intermediateBoards.Add(nextState);
                    break;
                }

                // If the next heuristic is greater than or equal to the current heuristic
                if (nextHeuristic >= currHeuristic)
                {
                    // If the next heuristic is equal to the current heuristic and 
                    // the program is set to perform sideways moves
                    if (nextHeuristic == currHeuristic && movesSideways)
                    {
                        // Increment the number of sideways moves
                        numSideways++;

                        // If the number of sideways moves is equal to 100
                        if (numSideways == 100)
                        {
                            // Break from the while loop
                            break;
                        }

                        // Set the next state equal to a random board from the list of equal boards
                        nextState = equalBoards[rand.Next(equalBoards.Count)];
                    }
                    else
                    {
                        // Break from the while loop
                        break;
                    }
                }
                // Else the next heuristic is less than the current heuristic
                else
                {
                    // Reset the number of sideways moves
                    numSideways = 0;
                }

                // Update the current state to the next state for the next iteration of the loop
                prevState = currentState;
                currentState = nextState;

                // Add the current state to the list of boards for the solution
                intermediateBoards.Add(currentState);
            }

            // Return the list of boards for the solution
            return intermediateBoards;
        }
    }
}