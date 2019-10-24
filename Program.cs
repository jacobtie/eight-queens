using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace hill_climbing_eight_queens
{
    class Program
    {
        private static bool doesRestart;
        private static bool movesSideways;
        private static bool testMode;
        private static int boardSize;

        static void Main(string[] args)
        {
            char input;

            do
            {
                GetUserInput();

                if (testMode)
                {
                    RunTest();
                }
                else
                {
                    HillClimbing.RunHillClimbing(doesRestart, movesSideways, boardSize);
                }

                do
                {
                    Logger.WriteLine("\nWould you like to run the program again? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');
            }
            while(input == 'Y');

            Logger.OutputToFile();
            Console.WriteLine("\nPress enter to exit...");
            Console.ReadLine();
        }

        static void GetUserInput()
        {
            string allInput;
            char input = '?';

            Logger.WriteLine("\nWelcome to the N-Queens Problem Solver. ");

            do
            {
                Logger.WriteLine("\nWould you like to run the tests for the program? (Y/N)");
                allInput = Console.ReadLine();

                if (allInput.Length > 0)
                {
                    input = allInput.ToUpper()[0];
                    Logger.WriteLine(input, false);
                }
            }
            while(input != 'Y' && input != 'N');

            testMode = (input == 'Y');
            input = '?';

            if (!testMode)
            {
                do
                {
                    Logger.WriteLine("\nWhat would you like the size of the board to be? (Must be positive)");
                    int.TryParse(Console.ReadLine(), out boardSize);
                }
                while(boardSize <= 0);

                Logger.WriteLine(boardSize, false);

                do
                {
                    Logger.WriteLine("\nWould you like the board to restart if a solution could not be found? (Y/N)");
                    allInput = Console.ReadLine();

                    if (allInput.Length > 0)
                    {
                        input = allInput.ToUpper()[0];
                    }

                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');

                doesRestart = (input == 'Y');
                input = '?';

                do
                {
                    Logger.WriteLine("\nWould you like the program to explore equal states? (Y/N)");
                    allInput = Console.ReadLine();

                    if (allInput.Length > 0)
                    {
                        input = allInput.ToUpper()[0];
                    }

                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');

                movesSideways = (input == 'Y');
            }
        }

        static void RunTest()
        {
            Logger.WriteLine("Running tests....this might take a while...");

            var results = new List<HillClimbingResult>();

            Logger.WriteLine("Recording default Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(false, false, 8, false));
            }

            Logger.WriteLine("Recording sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(false, true, 8, false));
            }

            Logger.WriteLine("Recording random restart without sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(true, false, 8, false));
            }

            Logger.WriteLine("Recording random restart with sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(true, true, 8, false));
            }

            using (var writer = new StreamWriter($"output/results_{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond}.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords<HillClimbingResult>(results);
            }

            Logger.WriteLine("All test results recorded in output directory");
        }
    }
}
