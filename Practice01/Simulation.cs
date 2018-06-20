using System;
using System.Windows.Forms;

namespace Practice01
{
    public class Simulation
    {
        Timer timer = new Timer();
        
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
            this.interval = (int)(1000/(fps*10));
            
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
            
            Util.WriteResultToFile("output.txt", time, body.Position);
            
            return new[] {body.Position.X, body.Position.Y, time};
        }

        public void Reset(Body newBody)
        {
            body = null;
            body = newBody;
        }

        private bool Step()
        {
            var positionBuffer = this.body.Position;
            this.CheckCollision(positionBuffer);

            time += delta_t;
            
            Console.WriteLine("\nTime: {0};", time);
            var velocity = body.Move(delta_t);

            return !(velocity.X == 0) && !(velocity.Y == 0);
        }

        
        private void CheckCollision(Vector position)
        {
            if (position.X >= 740 || position.X < 0)
            {
                this.body.InvertVelocityX();
            }
            
            if (position.Y >= 500 || position.Y < 0)
            {
                this.body.InvertVelocityY();
            }
        }
        
    }
}
