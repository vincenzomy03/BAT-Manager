using System;

namespace BatManager.Tests
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("       BAT-Manager - Unit Tests Runner            ");
            Console.WriteLine("==================================================");
            Console.WriteLine();

            try
            {
                Console.WriteLine("[1/2] Testing BatParser...");
                BatParserTests.RunAllTests();
                Console.WriteLine();

                Console.WriteLine("[2/3] Testing ScriptRepository...");
                ScriptRepositoryTests.RunAllTests();
                Console.WriteLine();

                Console.WriteLine("[3/3] Testing UpdateChecker...");
                UpdateCheckerTests.RunAllTests();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">> SUCCESS: All unit tests passed successfully! <<");
                Console.ResetColor();
                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(">> ERROR DURING TESTS: <<");
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
                return 1;
            }
        }
    }
}
