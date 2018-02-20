using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;

namespace CSharp_WPF_Practice
{

    class Simulation
    {
        private double delta_t;
        private Body body;
        private bool running;
        private double time;

        public Simulation(Body body, double delta_t = 0.001)
        {
            this.delta_t = delta_t;
            this.body = body;
            this.time = 0;
            this.running = true;
        }

        public void Run()
        {
            while (this.running)
            {
                time += delta_t;
                running = this.Step();
            }
        }

        private bool Step()
        {
            var position = body.Move(this.time);
            Console.WriteLine($"Time: {time}\nPosition: ({position[0]}, {position[1]})");
            
            return !(position[1] <= 0);
        }
    }

    class Body
    {
        private const double acceleration = 9.80665;
        
        private double angle;
        private double velocity;
        private double mass;
        private double x, y;
        
        public Body(double angle, double velocity,
                    double mass = 1, double x = 0, double y = 0)
        {
            this.angle = angle;
            this.velocity = velocity;
            this.mass = mass;
            this.x = x;
            this.y = y;
            
            Console.WriteLine($"\nNew Body:\nAngle: {angle}\nv0: {velocity}\nm: {mass}"); 
        }

        public double[] Move(double t)
        {
            double x, y;
            x = this.x + velocity * t * Math.Cos(angle);
            y = this.y + velocity * t * Math.Sin(angle) - mass * acceleration * t * t / 2;
            
            return new[] {x, y};
        }

        public double[] Position => new[] { x, y };
    }
    
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
