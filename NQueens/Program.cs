using System;
using System.Diagnostics;

namespace NQueens
{
    public class Program
    {
        const int STEPS_COEF = 2;
        static void Main(string[] args)
        {
            var queensNum = int.Parse(Console.ReadLine());
            var sw = new Stopwatch();
            sw.Start();
            var minConflicts = new MinConflicts(queensNum);
            minConflicts.InitQueens();
            while (!minConflicts.Solve(STEPS_COEF)) { };
            sw.Stop();
            if (queensNum < 20)
            {
                minConflicts.PrintState();
            }

            Console.WriteLine($"Program ran for {sw.ElapsedMilliseconds}ms");
        }
    }
}