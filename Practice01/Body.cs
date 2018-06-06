using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Practice01
{
    public class Body
    {
        private const double G = 9.80665;
        
        private double angle, mass;
        private Vector r, v, w;
        
        public Body(double angle, double v0,
            double mass = 1, double x0 = 0, double y0 = 0)
        {
            this.angle = angle;
            this.mass = mass;
            this.w = new Vector(0, 0);
            this.v = new Vector(v0 * Math.Cos(angle), v0 * Math.Sin(angle));
            this.r = new Vector(x0, y0);
            
            Console.WriteLine(
                "\nNew Body:\n  Angle: {0}\n  Velocity: {1}\n  Mass: {2}\n  Position: ({3}, {4})\n",
                angle, v0, mass, x0, y0
            );
        }

        public Vector Move(double dt)
        {
            
            //w.X += 0; 
            v.X += 0;
            r.X += v.X * dt;

            //w.Y = - mass * G / 2;
            v.Y += - mass * G / 2 * dt;
            r.Y += v.Y * dt;
      
            Console.WriteLine("Velocity: ({0}, {1})\nPosition: ({2}, {3});", v.X, v.Y, r.X, r.Y);
            
            return this.v;
        }

        public Vector Position => r;
        
        
        public void InvertVelocityX()
        {
            this.v.X = -v.X;
        }

        public void InvertVelocityY()
        {
            this.v.Y = -v.Y;
        }
    }
}
