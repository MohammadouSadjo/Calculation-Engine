using System;
using Calculation_Engine.Calculs;
using System.Threading;

namespace Calculation_Engine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculClass calculClass = new CalculClass();
            Console.WriteLine("Calculation Engine Running...");

            while (true)
            {
                calculClass.Calculation();
                Thread.Sleep(10000);
            }

        }
    }
}
