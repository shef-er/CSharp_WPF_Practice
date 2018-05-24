using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Practice01
{
    public struct Vector
    {  
        private double x;
        private double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get => x;
            set => x = value;
        }

        public double Y
        {
            get => y;
            set => y = value;
        }
    }
    
    /* ---- */
    
    public class Body
    {
        private const double G = 9.80665;
        
        private double angle, v0, mass;
        private Vector r, r0, v;
        private List<Vector> path;
        
        public Body(double angle, double v0,
            double mass = 1, double x0 = 0, double y0 = 0)
        {
            this.angle = angle;
            this.v0 = v0;
            this.mass = mass;
            this.v = new Vector(v0 * Math.Cos(angle), v0 * Math.Sin(angle));
            this.r = new Vector(0, 0);
            this.r0 = new Vector(x0, y0);
            this.path = new List<Vector>();
            
            Console.WriteLine(
                "\nNew Body:\n  Angle: {0}\n  Velocity: {1}\n  Mass: {2}\n  Position: ({3}, {4})\n",
                angle, v0, mass, x0, y0
            );
        }

        public Vector Position => r;
        
        public List<Vector> Path => path;

        public void InvertVelocityX()
        {
            this.v.X = -v.X;
        }

        public void InvertVelocityY()
        {
            this.v.Y = -v.Y;
        }

        public Vector Move(double t)
        {
            this.path.Add(this.Position);
            
            this.r.X = r0.X + v.X * t;
            this.r.Y = r0.Y + v.Y * t - mass * G * t * t / 2;
            
            if (r.X > 0 && r.Y <= 0)
                this.r = new Vector( Math.Sin(2 * angle) * v0 * v0 / G, 0);
            
            return this.Position;
        }
    }
}
