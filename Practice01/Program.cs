using System;

namespace CSharp_WPF_Practice
{

    class Simulation
    {
        private double delta_t;
        private Body body;
        private bool running;
        private double time;

        public Simulation(Body body, double delta_t = 0.01)
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
            
            if (position[1] <= 0)
            {
                Console.WriteLine("Time: {0}\nPosition: ({1}, 0)\n", time, body.Length);
                return false;
            }

            Console.WriteLine("Time: {0}\nPosition: ({1}, {2})\n", time, position[0], position[1]);
            return true;
        }
    }

    class Body
    {
        private const double acceleration = 9.80665;
        
        private double angle;
        private double velocity;
        private double mass;
        private double x0, y0, x, y, length;
        
        public Body(double angle, double velocity,
                    double mass = 1, double x0 = 0, double y0 = 0)
        {
            this.angle = angle;
            this.velocity = velocity;
            this.mass = mass;
            this.x0 = x0;
            this.y0 = y0;

            this.length = velocity * velocity * Math.Sin(2 * angle) / acceleration;

            Console.WriteLine("\nNew Body:\nAngle: {0}\nv0: {1}\nm: {2}", angle, velocity, mass); 
        }

        public double[] Move(double t)
        {
            x = x0 + velocity * t * Math.Cos(angle);
            y = y0 + velocity * t * Math.Sin(angle) - mass * acceleration * t * t / 2;
            
            return this.Position;
        }
        
        public double Length {    
            get { return length; }
        }

        public double[] Position {    
            get { return new[] { x, y }; }
        }
    }
    
    class Program
    {
        private static void Main(string[] args)
        {   
            Body ball = new Body(Math.PI/4, 10);
            Simulation flight = new Simulation(ball);
            flight.Run();

            while (true) {  }
            
        }
    }
}
