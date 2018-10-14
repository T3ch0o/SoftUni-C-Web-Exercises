namespace _01.Even_Numbers_Thread
{
    using System;
    using System.Linq;
    using System.Threading;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            int[] numbers = ReadLine().Split().Select(int.Parse).ToArray();

            Thread evens = new Thread(() => PrintEvenNumbers(numbers[0], numbers[1]));
            evens.Start();
            evens.Join();
            WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    WriteLine(i);
                }
            }
        }
    }
}