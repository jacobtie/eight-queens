using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace hill_climbing_eight_queens
{
    class Program
    {
        // Creation of fields to decide how the hill climbing will function
        private static bool doesRestart;
        private static bool movesSideways;
        private static bool testMode;
        private static int boardSize;

        // Main method to get user input and choose to run the test or the custom program
        static void Main(string[] args)
        {
            // Variables to store user input
            string allInput;
            char input = '?';

            // Do while the user stated that they wanted to rerun the program
            do
            {
                // Method call to get user input
                GetUserInput();

                // If the program is in test mode, run the test
                if (testMode)
                {
                    RunTest();
                }
                // Else run the custom program
                else
                {
                    // Begin hill climbing based on the inputted boolean variables
                    HillClimbing.RunHillClimbing(doesRestart, movesSideways, boardSize);
                }

                // Do while the user did not input Y or N
                do
                {
                    // Get user input to run the program again
                    Logger.WriteLine("\nWould you like to run the program again? (Y/N)");
                    allInput = Console.ReadLine();

                    // If the user inputted a string with at least one character
                    if (allInput.Length > 0)
                    {
                        // Set the input equal to the first character
                        input = allInput.ToUpper()[0];
                        Logger.WriteLine(input, false);
                    }
                }
                while(input != 'Y' && input != 'N');
            }
            while(input == 'Y');

            // Output the data to the CSV file
            Logger.OutputToFile();

            Console.WriteLine("\nPress enter to exit...");
            Console.ReadLine();
        }

        // Method to get user input and set boolean fields
        static void GetUserInput()
        {
            // Variables used to get user input
            string allInput;
            char input = '?';
            boardSize = 0;

            Logger.WriteLine("\nWelcome to the N-Queens Problem Solver. ");

            // Do while the user did not input Y or N
            do
            {
                // Get user input to run the test for the program
                Logger.WriteLine("\nWould you like to run the tests for the program? (Y/N)");
                allInput = Console.ReadLine();

                // If the user inputted a string with at least one character
                if (allInput.Length > 0)
                {
                    // Set the input equal to the first character
                    input = allInput.ToUpper()[0];
                    Logger.WriteLine(input, false);
                }
            }
            while(input != 'Y' && input != 'N');

            // Set the test mode
            testMode = (input == 'Y');

            // Reset the input
            input = '?';

            // If the program is not in test mode
            if (!testMode)
            {
                // Do while the board size is less than or equal to zero
                do
                {
                    // Attempt to set the board size equal to the user input
                    Logger.WriteLine("\nWhat would you like the size of the board to be? (Must be positive)");
                    int.TryParse(Console.ReadLine(), out boardSize);
                }
                while(boardSize <= 0);

                Logger.WriteLine(boardSize, false);

                // Do while the user did not input Y or N
                do
                {
                    // Get user input to run random restart
                    Logger.WriteLine("\nWould you like the board to restart if a solution could not be found? (Y/N)");
                    allInput = Console.ReadLine();

                    // If the user inputted a string with at least one character
                    if (allInput.Length > 0)
                    {
                        // Set the input equal to the first character
                        input = allInput.ToUpper()[0];
                        Logger.WriteLine(input, false);
                    }
                }
                while(input != 'Y' && input != 'N');

                // Set the program mode for restarting
                doesRestart = (input == 'Y');

                // Reset the input 
                input = '?';

                // Do while the user did not input Y or N
                do
                {
                    // Get user input to run with sideways moves along plateaus
                    Logger.WriteLine("\nWould you like the program to explore equal states? (Y/N)");
                    allInput = Console.ReadLine();

                    // If the user inputted a string with at least one character
                    if (allInput.Length > 0)
                    {
                        // Set the input equal to the first character
                        input = allInput.ToUpper()[0];
                    }

                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');

                // Set program mode for moving sideways along plateaus
                movesSideways = (input == 'Y');
            }
        }

        // Method to run the test and log the results to an CSV file
        static void RunTest()
        {
            Logger.WriteLine("Running tests....this might take a while...");

            // List of the results for the hill climbing runs
            var results = new List<HillClimbingResult>();

            // Run hill climbing without restarting or sideways moves 500 times
            Logger.WriteLine("Recording default Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(false, false, 8, false));
            }

            // Run hill climbing with sideways moves 500 times
            Logger.WriteLine("Recording sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(false, true, 8, false));
            }

            // Run hill climbing with random restart 500 times
            Logger.WriteLine("Recording random restart without sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(true, false, 8, false));
            }

            // Run hill climbing with random restart and sideways moves 500 times
            Logger.WriteLine("Recording random restart with sideways moves Hill Climbing with 8 Queens statistics...");
            for (int i = 0; i < 500; i++)
            {
                results.Add(HillClimbing.RunHillClimbing(true, true, 8, false));
            }

            // Write the results to a CSV file
            using (var writer = new StreamWriter($"output/results_{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond}.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords<HillClimbingResult>(results);
            }

            Logger.WriteLine("All test results recorded in output directory");
        }
    }
}
