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
        private Dictionary<string, double> parameters;

        private Program(Dictionary<string, double> parameters)
        {
            this.Text = "Body flight simulation";
            this.Size = new Size(1600, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.parameters = parameters;
        }
         
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = this.CreateGraphics();

            Body ball = new Body(parameters["angle"], parameters["v0"],
                parameters["mass"], parameters["x0"], parameters["y0"]);

            Simulation flight = new Simulation(ball, formGraphics);
            flight.Run();
    
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