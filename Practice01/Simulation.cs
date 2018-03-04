using System;

namespace CSharp_WPF_Practice
{
    public class Simulation
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
                Console.WriteLine($"Time: {time}\nPosition: ({body.Length}, 0)\n");
                return false;
            }
            
            Console.WriteLine($"Time: {time}\nPosition: ({position[0]}, {position[1]})\n");
            return true;
        }
        
        public ulong GetCurrentTime()
        {
            TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (ulong) time.TotalSeconds;
        }
    }

    public class Body
    {
        private const double gravity = 9.80665;
        
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

            this.length = velocity * velocity * Math.Sin(2 * angle) / gravity;
            
            Console.WriteLine($"\nNew Body:\nAngle: {angle}\nv0: {velocity}\nm: {mass}"); 
        }

        public double[] Move(double t)
        {
            x = x0 + velocity * t * Math.Cos(angle);
            y = y0 + velocity * t * Math.Sin(angle) - mass * gravity * t * t / 2;
            
            return this.Position;
        }
        
        public double Length => length;

        public double[] Position => new[] { x, y };
    }
}