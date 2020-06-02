using System;

namespace Eventus.ConsoleUI
{
    public static class ConsoleHelper
    {
        public static int EnterNumber()
        {
            while (true)
            {
                Console.WriteLine("Enter number:");
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    return number;
                }
            }
        }

        public static double EnterDoubleNumber()
        {
            while (true)
            {
                Console.WriteLine("Enter double number:");
                if (double.TryParse(Console.ReadLine(), out double number))
                {
                    return number;
                }
            }
        }
    }
}