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
            bool replay;
            char input;

            do
            {
                GetUserInput();

                if (testMode)
                {

                }
                else
                {
                    HillClimbing.RunHillClimbing(doesRestart, movesSideways, boardSize);
                }

                do
                {
                    Console.WriteLine("\nWould you like to run the program again? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
                }
                while(input != 'Y' && input != 'N');

                replay = (input == 'Y');
            }
            while(replay);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void GetUserInput()
        {
            char input;

            Console.WriteLine("\nWelcome to the N-Queens Problem Solver. ");

            do
            {
                Console.WriteLine("\nWould you like to run the tests for the program? (Y/N)");
                input = Console.ReadLine().ToUpper()[0];
            }
            while(input != 'Y' && input != 'N');

            testMode = (input == 'Y');

            if (testMode)
            {
                RunTest();
            }
            else
            {
                do
                {
                    Console.WriteLine("\nWhat would you like the size of the board to be?");
                }
                while(!int.TryParse(Console.ReadLine(), out boardSize));
                
                do
                {
                    Console.WriteLine("\nWould you like the board to restart if a solution could not be found? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
                }
                while(input != 'Y' && input != 'N');

                doesRestart = (input == 'Y');

                do
                {
                    Console.WriteLine("\nWould you like the program to explore equal states? (Y/N)");
                    input = Console.ReadLine().ToUpper()[0];
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
