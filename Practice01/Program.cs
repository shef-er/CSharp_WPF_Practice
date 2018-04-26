using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Practice01
{
    class Program : Form
    {
        private String[] inputTags = {"angle", "v0", "mass", "x0", "y0"};
        private String[] inputLabels = { "Angle", "Velocity", "Mass", "X", "Y" };
        private Dictionary<string, double> parameters;
        private Control fieldResult;
        private Control[] inputArray;
        private Control[] labelArray;
        private Graphics formGraphics;
        private Point[] arrow = new Point[2];

        private Program(Dictionary<string, double> parameters)
        {
            this.parameters = parameters;
            
            this.Text = "Body flight simulation";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            formGraphics = this.CreateGraphics();

            // Controls
            inputArray = new Control[5];
            labelArray = new Control[5];

            for (var i = 0; i < 5; i++ ) 
            {
                inputArray[i] = new TextBox();
                inputArray[i].Location = new Point(30 + i*100, 30);
                inputArray[i].Size = new Size(60, 20);
                inputArray[i].Text = this.parameters[inputTags[i]].ToString(CultureInfo.InvariantCulture);
                
                labelArray[i] = new Label();
                labelArray[i].Location = new Point(30 + i*100, 10);
                labelArray[i].Size = new Size(60, 20);
                labelArray[i].Text = inputLabels[i];
                
                this.Controls.Add(inputArray[i]);
                this.Controls.Add(labelArray[i]);
                
            }
            
            Button buttonStart = new Button();
            buttonStart.Location = new Point(30, 70);
            buttonStart.Size = new Size(60, 25);
            buttonStart.Text = "Start";
            buttonStart.Click += buttonStart_Click;
            
            fieldResult = new TextBox();
            fieldResult.Location = new Point(130, 70);
            fieldResult.Size = new Size(360, 20);
            
            this.Controls.Add(buttonStart);
            this.Controls.Add(fieldResult);

            this.MouseDown += form_OnMouseDown;
            this.MouseUp += form_OnMouseUp;

        }

        private void DrawSimulation()
        {
            Body ball = new Body(parameters["angle"], parameters["v0"],
                parameters["mass"], parameters["x0"], parameters["y0"]);
            Simulation flight = new Simulation(ball, formGraphics);
            var result = flight.Run();
            
            fieldResult.Text = String.Format("Result: ({1}, {2}) after {0} seconds",
                result[2], result[0], result[1]);
        }

        private void ChangeParameterByName(String name, String value)
        {
            this.parameters[name] = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < 5; i++)
                this.ChangeParameterByName(inputTags[i], inputArray[i].Text.Trim());
            
            DrawSimulation();
        }

        protected void form_OnMouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseDown: ({0}, {1})", e.X, e.Y);
            arrow[0] = new Point(e.X, e.Y);

        }

        protected void form_OnMouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseUp: ({0}, {1})", e.X, e.Y);
            arrow[1] = new Point(e.X, e.Y);

            double arrowX = arrow[1].X - arrow[0].X;
            double arrowY = arrow[1].Y - arrow[0].Y;
            double arrowLength = Math.Sqrt(Math.Pow(arrowX, 2) + Math.Pow(arrowY, 2));
            double arrowAngle = Math.Acos(arrowX / arrowLength);
            ChangeParameterByName("angle", arrowAngle);

            DrawArrow();

        }

        private void DrawArrow()
        {
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 255), 4);
            pen.StartCap = LineCap.ArrowAnchor;
            pen.EndCap = LineCap.RoundAnchor;
            formGraphics.DrawLine(pen, arrow[1], arrow[0]);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawSimulation();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            Dictionary<string, double> parameters = new Dictionary<string, double>();
            parameters.Add("angle", 0);
            parameters.Add("v0", 0);
            parameters.Add("mass", 1);
            parameters.Add("x0", 0);
            parameters.Add("y0", 0);

            try
            {
                using (var sr = new StreamReader("input.txt"))
                {
                    String line;
                    while ( (line = sr.ReadLine()) != null )
                    {
                        var items = line.Split(':');
                        if (items[1] != null)
                        {
                           var key = items[0].Trim();
                           var value = Convert.ToDouble(items[1].Trim(), CultureInfo.InvariantCulture);
                           parameters[key] = value;
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            
            Application.Run(new Program(parameters));
        }
    }
}
