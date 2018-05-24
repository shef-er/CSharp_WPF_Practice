using System;
using System.Drawing;
using System.IO;

namespace Practice01
{
    public class Simulation
    {
        private const int OffsetAxisX = 30;
        private const int OffsetAxisY = 500;
        
        private Body body;
        private Graphics graphics;
        private double delta_t;
        private double time;
        private bool running;

        public Simulation(Body body, Graphics graphics)
        {
            this.delta_t = 0.05;
            this.body = body;
            this.time = 0;
            this.running = true;
            this.graphics = graphics;
        }

        public double[] Run()
        {
            DrawScene();
            DrawBody();
            
            while (this.running)
            {
                this.time += delta_t;
                this.running = this.Step();
                
                DrawBody();
            }

            WriteResultToFile("output.txt");
            
            var position = this.body.Position;
            return new[] {position.X, position.Y, time};
        }

        private bool Step()
        {
            var position = this.body.Position;
            //this.CheckCollision(position);
            
            position = this.body.Move(time);

            Console.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n",
                time, position.X, position.Y);

            return !(position.X > 0 && position.Y == 0) &&
                   !(position.X == 0 && position.Y < 0);
        }

        private void CheckCollision(Vector position)
        {
            if (position.X >= 800 || position.X <= 0)
            {
                this.body.InvertVelocityX();
            }
        }

        private void DrawScene()
        {
            graphics.Clear(Color.White);
            graphics.FillRectangle(new SolidBrush(Color.Black), 0, OffsetAxisY, 100000, 1);
        }

        private void DrawBody()
        {
            var position = this.body.Position;

            graphics.FillEllipse(new SolidBrush(Color.DeepPink),
                OffsetAxisX + (int)(position.X * 1.25), 
                OffsetAxisY - ((int)(position.Y * 1.25) + 2), 2, 2);
        }
        
        /* ---- */

        private void WriteResultToFile(String filename)
        {
            var position = this.body.Position;
            using (StreamWriter sw = new StreamWriter(filename)) 
            {
                sw.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n",
                    time, position.X, position.Y);
                sw.Close();
            }
        }
    }
}
