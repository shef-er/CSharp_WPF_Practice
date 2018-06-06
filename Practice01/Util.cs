using System;
using System.IO;
using System.Windows.Forms;

namespace Practice01
{
    public class Util 
    {
        public static void WriteResultToFile(String filename, double time, Vector position)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n",
                    time, position.X, position.Y);
                sw.Close();
            }
        }
    }
    
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

        public double Abs => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
    }
}