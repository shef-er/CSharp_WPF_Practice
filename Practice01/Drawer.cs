using System.Drawing;
using System.Drawing.Drawing2D;

namespace Practice01
{
    public class Drawer
    {
        private Graphics graphics;
        private Point zeroOffset;
        private Vector positionBuffer;

        public Drawer(Graphics graphics, Point zeroOffset)
        {
            this.graphics = graphics;
            this.zeroOffset = zeroOffset;
        }

        public void DrawStep(Vector position)
        {
            this.positionBuffer = position;
            
            graphics.Clear(Color.White);
            
            graphics.FillRectangle(new SolidBrush(Color.Black), zeroOffset.X, 0, 1, zeroOffset.Y);
            graphics.FillRectangle(new SolidBrush(Color.Black), 771, 0, 1, zeroOffset.Y);
            
            graphics.FillRectangle(new SolidBrush(Color.Black), 0, zeroOffset.Y, 100000, 1);
            
            graphics.FillEllipse(new SolidBrush(Color.DeepPink),
                zeroOffset.X + (int)(position.X), 
                zeroOffset.Y - ((int)(position.Y) + 5), 5, 5);
        }
        
        public void DrawArrow(Point arrowEnd)
        {
            DrawStep(positionBuffer);
            
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 255), 3);
            pen.StartCap = LineCap.ArrowAnchor;
            pen.EndCap = LineCap.RoundAnchor;

            Point arrowStart = new Point(zeroOffset.X, zeroOffset.Y);
			
            graphics.DrawLine(pen, arrowEnd, arrowStart);
        }
    }
}