using System;
using System.Drawing;
using System.IO;

namespace Practice01
{
    public class Body
    {
        private const double gravity = 9.80665;
        
        private double angle;
        private double v0, vx, vy;
        private double mass;
        private double x0, y0, x, y, length;
        
        public Body(double angle, double v0,
            double mass = 1, double x0 = 0, double y0 = 0)
        {
            this.angle = angle;
            this.v0 = v0;
            this.vx = v0 * Math.Cos(angle);
            this.vy = v0 * Math.Sin(angle);
            this.mass = mass;
            this.x0 = x0;
            this.y0 = y0;

            this.length = v0 * v0 * Math.Sin(2 * angle) / gravity;
            
            Console.WriteLine(
                "\nNew Body:\n  Angle: {0}\n  Velocity: {1}\n  Mass: {2}\n  Position: ({3}, {4})\n",
                angle, v0, mass, x0, y0
            );
        }

        public void InvertVelocityX()
        {
            this.vx = -vx;
        }

        public void InvertVelocityY()
        {
            this.vy = -vy;
        }

        public double[] Move(double t)
        {

            x = x0 + vx * t;
            y = y0 + vy * t - mass * gravity * t * t / 2;
            
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
