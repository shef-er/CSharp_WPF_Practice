using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Practice01
{
	class Program : Form
	{
		private CultureInfo invC = CultureInfo.InvariantCulture;
		
		private Dictionary<string, double> parameters;
		private Dictionary<string, string> labels;
		
		private Graphics canvasGraphics;
		
		private Point canvasZeroOffset;
		private Drawer canvasDrawer;
		private Simulation simulation;
		
		private Program(string filename)
		{
			this.Text = "Body flight simulation";
			this.Size = new Size(1280, 720);
			this.StartPosition = FormStartPosition.CenterScreen;
			
			labels = new Dictionary<string, string>();
			labels.Add("angle", "Angle:");
			labels.Add("v0", "Velocity:");
			labels.Add("mass", "Mass:");
			labels.Add("x0", "Start X:");
			labels.Add("y0", "Start Y:");
			
			parameters = new Dictionary<string, double>();
			parameters.Add("angle", 0);
			parameters.Add("v0", 0);
			parameters.Add("mass", 1);
			parameters.Add("x0", 0);
			parameters.Add("y0", 0);

			this.ReadParametersFromFile(filename);
			
			// Parameter fields
			var index = 0;
			foreach(var item in parameters)
			{
				Control input = new TextBox();
				input.Location = new Point(30 + index*100, 30);
				input.Size = new Size(60, 20);
				input.Name = item.Key;
				input.Text = item.Value.ToString(invC);
				
				Control label = new Label();
				label.Location = new Point(30 + index*100, 10);
				label.Size = new Size(60, 20);
				label.Text = labels[item.Key];
				
				this.Controls.Add(input);
				this.Controls.Add(label);
				index++;
			}

			// Simulation start button
			Button buttonStart = new Button();
			buttonStart.Location = new Point(30, 70);
			buttonStart.Size = new Size(60, 25);
			buttonStart.Text = "Start";
			buttonStart.Click += buttonStart_Click;

			// Simulation stop button
			Button buttonStop = new Button();
			buttonStop.Location = new Point(130, 70);
			buttonStop.Size = new Size(60, 25);
			buttonStop.Text = "Stop";
			buttonStop.Click += buttonStop_Click;

			// Results after stop
			Control textBoxResult;
			textBoxResult = new TextBox();
			textBoxResult.Location = new Point(230, 70);
			textBoxResult.Size = new Size(360, 20);
			textBoxResult.Name = "Result";
			
			// Canvas
			PictureBox canvas = new PictureBox();
			canvas.Location = new Point(30, 110);
			canvas.Size = new Size(1000, 550);
			canvas.Name = "Canvas";
			canvas.MouseUp += canvas_OnMouseUp;

			// Adding controls to window
			this.Controls.Add(buttonStart);
			this.Controls.Add(buttonStop);
			this.Controls.Add(textBoxResult);
			this.Controls.Add(canvas);
			
			// Create graphics stuff
			this.canvasGraphics = canvas.CreateGraphics();
			this.canvasZeroOffset = new Point(30, 500);
			this.canvasDrawer = new Drawer(canvasGraphics, canvasZeroOffset);
			
			//Create simulation
			Body ball = new Body(parameters["angle"], parameters["v0"],
				parameters["mass"], parameters["x0"], parameters["y0"]);
			
			this.simulation = new Simulation(ball, canvasDrawer);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//simulation.Start();
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			UpdateAllParameters();
			StartSimulation();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			StopSimulation();
		}

		private void canvas_OnMouseUp(object sender, MouseEventArgs e)
		{	
			SetAngle(new Point(e.X, e.Y), canvasZeroOffset);
		}

		private void StartSimulation()
		{
			Body ball = new Body(parameters["angle"], parameters["v0"],
				parameters["mass"], parameters["x0"], parameters["y0"]);
			
			simulation.Reset(ball);
			simulation.Start();
		}

		private void StopSimulation()
		{
			var result = simulation.Stop();
			var input = this.Controls.Find("Result", false)[0];
			input.Text = String.Format("Result: ({1}, {2}) after {0} seconds",
				result[2], result[0], result[1]);
		}

		private void SetAngle(Point arrowEnd, Point arrowStart)
		{
			canvasDrawer.DrawArrow(arrowEnd);
			
			double arrowX = arrowEnd.X - arrowStart.X;
			double arrowY = arrowEnd.Y - arrowStart.Y;
			//double arrowLength = Math.Sqrt(Math.Pow(arrowX, 2) + Math.Pow(arrowY, 2));
		
			double angle = Math.Abs(Math.Atan(arrowY / arrowX));
			UpdateParameterByName("angle", angle); 
		}

		/* -- Parameters -- */
		
		private void UpdateAllParameters()
		{
			foreach(var item in parameters.ToList())
			{
				var input = this.Controls.Find(item.Key, false)[0];
				this.UpdateParameterByName(item.Key, 
					Convert.ToDouble(input.Text.Trim(), invC));
			}
		}

		private void UpdateParameterByName(String name, double value)
		{
			this.parameters[name] = value;
			var input = this.Controls.Find(name, false)[0];
			input.Text = value.ToString(invC);
		}

		private void ReadParametersFromFile(string filename)
		{
			try
			{
				using (var sr = new StreamReader(filename))
				{
					String line;
					while ( (line = sr.ReadLine()) != null )
					{
						var items = line.Split(':');
						if (items[1] != null)
						{
							var key = items[0].Trim();
							var value = Convert.ToDouble(items[1].Trim(), invC);
							this.parameters[key] = value;
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
		}

		[STAThread]
		private static void Main(string[] args)
		{
			Application.Run(new Program("input.txt"));
		}
	}
}
