using System;

namespace CSharp_WPF_Practice
{
    class Program
    {
        private static void Main(string[] args)
        {   
            Body ball = new Body(Math.PI/4, 10);
            Simulation flight = new Simulation(ball);
            flight.Run();
        }
    }
}
