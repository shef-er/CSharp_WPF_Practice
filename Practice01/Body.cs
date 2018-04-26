using System;
using System.Drawing;
using System.IO;

namespace Practice01
{
    public class Body
    {
        private const double gravity = 9.80665;
        
        private double angle;
        private double v0;
        private double mass;
        private double x0, y0, x, y, length;
        
        public Body(double angle, double v0,
            double mass = 1, double x0 = 0, double y0 = 0)
        {
            this.angle = angle;
            this.v0 = v0;
            this.mass = mass;
            this.x0 = x0;
            this.y0 = y0;

            this.length = v0 * v0 * Math.Sin(2 * angle) / gravity;
            
            Console.WriteLine(
                "\nNew Body:\n  Angle: {0}\n  Velocity: {1}\n  Mass: {2}\n  Position: ({3}, {4})\n",
                angle, v0, mass, x0, y0
            );
        }

        public double[] Move(double t)
        {
            x = x0 + v0 * t * Math.Cos(angle);
            y = y0 + v0 * t * Math.Sin(angle) - mass * gravity * t * t / 2;
            
            if (x > 0 && y <= 0)
            {
                x = this.length;
                y = 0;
            }
            
            return this.Position;
        }
        
        public double[] Position {    
            get { return new[] { x, y }; }
        }
    }
}