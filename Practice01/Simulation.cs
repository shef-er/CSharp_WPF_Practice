using System;
using System.IO;
using System.Windows.Forms;

namespace Practice01
{
    public class Simulation
    {
        Timer timer = new Timer();
        
        private const int OffsetAxisX = 30;
        private const int OffsetAxisY = 500;
        
        private Body body;

        private Drawer drawer;
        private double fps;
        private double delta_t;
        private int interval;
        private double time;
        private bool running;

        public Simulation(Body body, Drawer drawer)
        {
            this.fps = 100;
            this.delta_t = 1/fps;
            this.interval = (int)(1000/fps);
            
            this.body = body;
            this.time = 0;
            this.running = true;
            
            this.drawer = drawer;
            
            this.timer.Interval = this.interval;
            this.timer.Tick += new EventHandler(Timer_Tick);
        } 

        private void Timer_Tick(object Sender, EventArgs e)     
        {
            if (!running)
            {
                Stop();
                return;
            }

            drawer.DrawStep(body.Position);

            running = Step();
        }  

        public void Start()
        {
            timer.Start();
            Console.WriteLine("\nTIMER: STARTED\n");
        }

        public double[] Stop()
        {
            timer.Stop();
            Console.WriteLine("\nTIMER: STOPED\n");
            
            //WriteResultToFile("output.txt");
            
            return new[] {body.Position.X, body.Position.Y, time};
        }

        public void Reset(Body newBody)
        {
            body = null;
            body = newBody;
        }

        private bool Step()
        {
            var position = this.body.Position;
            //this.CheckCollision(position);
            
            time += delta_t;
            position = body.Move(time);

            Console.WriteLine("\nTime: {0}\nPosition: ({1}, {2})",
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
        
        /* -- Write -- */

        private void WriteResultToFile(String filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("\nTime: {0}\nPosition: ({1}, {2})\n",
                    time, body.Position.X, body.Position.Y);
                sw.Close();
            }
        }
    }
}
