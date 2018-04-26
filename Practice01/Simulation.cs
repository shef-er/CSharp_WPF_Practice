using System;
using System.Drawing;
using System.IO;

namespace Practice01
{
    public class Simulation
    {
        private const int sceneHeight = 500;
        private const int sceneOffsetX = 30;
        private const int sceneOffsetY = 0;
        
        private Body body;
        private Graphics graphics;
        private double delta_t;
        private double time;
        private bool running;

        public Simulation(Body body, Graphics graphics)
        {
            this.delta_t = 0.01;
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
                time += delta_t;
                running = this.Step();
                
                DrawBody();
            }
            
            var position = this.body.Position;
            
            using (StreamWriter sw = new StreamWriter("output.txt")) 
            {

                sw.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n",
                    time, position[0], position[1]);
                sw.Close();
            }

            return new[] {position[0], position[1], time};
        }

        private bool Step()
        {
            var position = this.body.Move(this.time);
            
            //Console.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n", time, position[0], position[1]);

            return !(position[0] > 0 && position[1] == 0) &&
                   !(position[0] == 0 && position[1] < 0);
        }

        private void DrawScene()
        {
            //graphics.FillRectangle(new SolidBrush(Color.DeepSkyBlue), 0,   100, 10000, 10000);
            //graphics.FillRectangle(new SolidBrush(Color.ForestGreen), 0, sceneHeight, 10000, 10000);
            graphics.Clear(Color.White);
            graphics.FillRectangle(new SolidBrush(Color.Black), 0,   sceneHeight, 100000, 1);
        }

        private void DrawBody()
        {
            var position = this.body.Position;

            graphics.FillEllipse(new SolidBrush(Color.DeepPink),
                sceneOffsetX + (int)(position[0]*1.25), 
                sceneOffsetY + sceneHeight -( (int)(position[1]*1.25) +2), 2, 2);
        }
    }
}
