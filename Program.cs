using System;
using System.Linq;

namespace hill_climbing_eight_queens
{
    class Program
    {
        static void Main(string[] args)
        {
            var retry = false;
            var restarts = 0;

            do
            {
                var gameBoard = Board.BuildRandomBoard();

                var boards = HillClimbing.ClimbHill(gameBoard);

                foreach (var board in boards)
                {
                    Console.Write(board.GetBoardAsString());
                    Console.WriteLine($"Heuristic: {HillClimbing.GetHeuristicFromBoard(board)}");
                }

                if (boards.Last().GoalState)
                {
                    retry = false;
                    Console.Write($"Success! Restarted {restarts} time");
                    if (restarts != 1)
                    {
                        Console.WriteLine("s.");
                    }
                    else
                    {
                        Console.WriteLine(".");
                    }
                }
                else
                {
                    retry = true;
                    restarts++;
                    Console.WriteLine($"Failed. Restarting");
                }
            } while (retry);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
