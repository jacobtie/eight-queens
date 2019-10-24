using System;
using System.Linq;

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
            char input;

            Logger.WriteLine("\nWelcome to the N-Queens Problem Solver. ");

            do
            {
                Logger.WriteLine("\nWould you like to run the tests for the program? (Y/N)");
                input = Console.ReadLine().ToUpper()[0];
                Logger.WriteLine(input, false);
            }
            while(input != 'Y' && input != 'N');

            testMode = (input == 'Y');

            if (!testMode)
            {
                do
                {
                    Logger.WriteLine("\nWhat would you like the size of the board to be?");
                }
                while(!int.TryParse(Console.ReadLine(), out boardSize));
                Logger.WriteLine(boardSize, false);

                do
                {
                    Logger.WriteLine("\nWould you like the board to restart if a solution could not be found? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');

                doesRestart = (input == 'Y');

                do
                {
                    Logger.WriteLine("\nWould you like the program to explore equal states? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
                    Logger.WriteLine(input, false);
                }
                while(input != 'Y' && input != 'N');

                movesSideways = (input == 'Y');
            }
        }

        static void RunTest()
        {
            // Needs the big implement
        }
    }
}
