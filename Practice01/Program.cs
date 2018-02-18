using System;
using System.Collections.Generic;

namespace CSharp_WPF_Practice
{
    struct Position
    {
        private double x, y;

        public Position(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }

    
    class Body
    {
        const double acceleration = 9.80665;
        
        private double mass;
        private double v0, v1;
        private Position position;
        
        public Body(double mass = 1, double x = 0, double y = 0)
        {
            this.mass = mass;
            this.position = new Position(x, y);
            Console.WriteLine($"\nMass: {mass}\nVelocity: {velocity}");
            
        }
        
        public void Throw(double angle, double start_velocity)
        {
            this.v0 = start_velocity;

        }
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {   
            Body ball = new Body();
            
            
        }
    }
}
