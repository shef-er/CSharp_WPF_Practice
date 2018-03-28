using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Practice01
{
    class Program : Form
    {
        String[] inputTags = {"angle", "v0", "mass", "x0", "y0"};
        
        private Dictionary<string, double> parameters;

        private bool defaultSimulationDone = false;
        private Control fieldResult;
        
        private Control[] inputArray;

        private Program(Dictionary<string, double> parameters)
        {
            this.parameters = parameters;
            
            this.Text = "Body flight simulation";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Controls
            
            inputArray = new Control[5];
            Control[] labelArray = new Control[5];
            String[] labels = {"Angle", "Velocity", "Mass", "X", "Y"};

            for (var i = 0; i < 5; i++ ) 
            {
                inputArray[i] = new TextBox();
                inputArray[i].Location = new Point(30 + i*100, 30);
                inputArray[i].Size = new Size(60, 20);
                inputArray[i].Text = this.parameters[inputTags[i]].ToString(CultureInfo.InvariantCulture);
                
                labelArray[i] = new Label();
                labelArray[i].Location = new Point(30 + i*100, 10);
                labelArray[i].Size = new Size(60, 20);
                labelArray[i].Text = labels[i];
                
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
           
        }

        private void DrawSimulation()
        {
            Graphics formGraphics = this.CreateGraphics();
            
            Body ball = new Body(parameters["angle"], parameters["v0"],
                parameters["mass"], parameters["x0"], parameters["y0"]);
            Simulation flight = new Simulation(ball, formGraphics);
            var result = flight.Run();
            
            fieldResult.Text = String.Format("Result: ({1}, {2}) after {0} seconds",
                result[2], result[0], result[1]);
        }
        
        private void buttonStart_Click(object sender, EventArgs e)
        {

            for (var i = 0; i < 5; i++)
                this.parameters[inputTags[i]] = Convert.ToDouble(inputArray[i].Text.Trim(), CultureInfo.InvariantCulture);
            
            DrawSimulation();
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