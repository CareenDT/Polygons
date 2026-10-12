using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Polygons.Shapes
{
    public abstract class Shape
    {
        protected static readonly Pen pen;

        protected int x;
        protected int y;
        protected static int radius;

        protected Rectangle extents;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Radius { get { return radius; } set { radius = value; } }

        public virtual void Draw(Graphics g) { }

        public virtual bool IsInside(int mouseX, int mouseY) { return false; }

        static Shape()
        {
            pen = new Pen(new SolidBrush(Color.Gray));

            radius = 150;
        }

        protected Point GetPointOnCircle(float angle, int radius)
        {
            return new Point((int)(MathF.Cos(angle) * radius), (int)(MathF.Sin(angle) * radius));
        }
    }

    public class Square : Shape
    {
        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;

            Point point1 = GetPointOnCircle(0.785398f, radius);
            Point point2 = GetPointOnCircle(0.785398f, radius);

            extents = new(x - point1.X / 2, y - point1.Y / 2, point1.X, point2.Y);
        }

        public override void Draw(Graphics g)
        {

            g.DrawRectangle(pen, extents);
        }

        public override bool IsInside(int mouseX, int mouseY)
        {
            return mouseX >= extents.Left && mouseX <= extents.Right
                && mouseY >= extents.Bottom && mouseY >= extents.Top;
        }
    }

    public class Circle : Shape
    {
        public Circle(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(pen, new Rectangle(x - Radius / 2, y - Radius / 2, Radius, Radius));
        }
    }

    public class Triangle : Shape
    {
        public Triangle(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            Point sidePoint = GetPointOnCircle(0.523599f, radius / 2);
            Point[] points =
            [
                new(x, y - radius / 2),
                new(x - sidePoint.X, y + sidePoint.Y),
                new(x + sidePoint.X, y + sidePoint.Y)
            ];
            g.DrawPolygon(pen, points);
        }
    }
}
