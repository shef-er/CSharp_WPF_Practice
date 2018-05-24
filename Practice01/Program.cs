using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		
		private Control fieldResult;
		private Point[] arrow = new Point[2];
		
		private Graphics formGraphics;
		
		private Program(string filename)
		{
			this.Text = "Body flight simulation";
			this.Size = new Size(800, 600);
			this.StartPosition = FormStartPosition.CenterScreen;
			
			parameters = new Dictionary<string, double>();
			parameters.Add("angle", 0);
			parameters.Add("v0", 0);
			parameters.Add("mass", 1);
			parameters.Add("x0", 0);
			parameters.Add("y0", 0);

			this.ReadParametersFromFile(filename);
			
			labels = new Dictionary<string, string>();
			labels.Add("angle", "Angle:");
			labels.Add("v0", "Velocity:");
			labels.Add("mass", "Mass:");
			labels.Add("x0", "Start X:");
			labels.Add("y0", "Start Y:");
			
			// Controls
			int index = 0;
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
			
			// Create graphics object
			formGraphics = this.CreateGraphics();
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			foreach(var item in parameters.ToList())
			{	
				var input = this.Controls.Find(item.Key, false)[0];
				this.UpdateParameterByName(item.Key, Convert.ToDouble(input.Text.Trim(), invC));
			}
			
			DrawSimulation();
		}

		protected void form_OnMouseDown(object sender, MouseEventArgs e)
		{
			Console.WriteLine("MouseDown: ({0}, {1})", e.X, e.Y);
            //arrow[0] = new Point(e.X, e.Y);
            arrow[0] = new Point(30, 500);
        }

		protected void form_OnMouseUp(object sender, MouseEventArgs e)
		{
			Console.WriteLine("MouseUp: ({0}, {1})", e.X, e.Y);
			arrow[1] = new Point(e.X, e.Y);

			// TODO: pass and count all of this mess separatley
			double arrowX = arrow[1].X - arrow[0].X;
			double arrowY = arrow[1].Y - arrow[0].Y;
			double arrowLength = Math.Sqrt(Math.Pow(arrowX, 2) + Math.Pow(arrowY, 2));
			double arrowAngle = Math.Acos(arrowX / arrowLength);
			
			UpdateParameterByName("angle", arrowAngle);

			DrawArrow();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			DrawSimulation();
		}

		private void UpdateParameterByName(String name, double value)
		{
			this.parameters[name] = value;
			var input = this.Controls.Find(name, false)[0];
			input.Text = value.ToString(invC);
		}

		private void DrawArrow()
		{
			Pen pen = new Pen(Color.FromArgb(255, 0, 0, 255), 3);
			pen.StartCap = LineCap.ArrowAnchor;
			pen.EndCap = LineCap.RoundAnchor;
			formGraphics.DrawLine(pen, arrow[1], arrow[0]);
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
