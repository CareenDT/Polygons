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

        //protected bool isMoving;
        protected int moveDX;
        protected int moveDY;

        public virtual int X { get { return x; } set { x = value; } }
        public virtual int Y { get { return y; } set { y = value; } }
        public int Radius { get { return radius; } set { radius = value; } }

        //public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
        public int MoveDX { get { return moveDX; } set { moveDX = value; } }
        public int MoveDY { get { return moveDY; } set { moveDY = value; } }

        public virtual void Draw(Graphics g) { }

        public virtual bool IsInside(int mouseX, int mouseY) => false;

        static Shape()
        {
            pen = new Pen(new SolidBrush(Color.Gray));

            radius = 150;
        }

        protected Point GetPointOnCircle(float angle)
        {
            return new Point((int)(MathF.Cos(angle) * radius), (int)(MathF.Sin(angle) * radius));
        }
    }

    public class Square : Shape
    {
        public override int X
        {
            get => x;
            set { x = value; RecalculateExtents(); }
        }

        public override int Y
        {
            get => y;
            set { y = value; RecalculateExtents(); }
        }

        protected Rectangle extents;

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;

            RecalculateExtents();
        }

        private void RecalculateExtents()
        {
            int corner = (int)(MathF.Cos(0.785398f) * radius);
            extents = new Rectangle(x - corner, y - corner, corner * 2, corner * 2);
        }

        public override void Draw(Graphics g)
        {

            g.DrawRectangle(pen, extents);
        }

        public override bool IsInside(int mouseX, int mouseY)
        {
            return mouseX >= extents.Left && mouseX <= extents.Right
                && mouseY <= extents.Bottom && mouseY >= extents.Top;
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
            g.DrawEllipse(pen, new Rectangle(x - radius, y - radius, radius * 2, radius * 2));
        }

        public override bool IsInside(int mouseX, int mouseY)
        {
            int relativeX = x - mouseX;
            int relativeY = y - mouseY;

            return MathF.Sqrt(relativeX * relativeX + relativeY * relativeY) <= radius;
        }
    }

    public class Triangle : Shape
    {

        Point[] points = new Point[3];

        public Triangle(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            Point sidePoint = GetPointOnCircle(0.523599f);
            points =
            [
                new(x, y - radius),
                new(x - sidePoint.X, y + sidePoint.Y),
                new(x + sidePoint.X, y + sidePoint.Y)
            ];
            g.DrawPolygon(pen, points);
        }

        public override bool IsInside(int mouseX, int mouseY)
        {
            Point mousePoint = new(mouseX, mouseY);

            int wholeArea = GetTriangleArea(points[0], points[1], points[2]);

            int sum = GetTriangleArea(mousePoint, points[0], points[1]) +
                      GetTriangleArea(mousePoint, points[1], points[2]) +
                      GetTriangleArea(mousePoint, points[2], points[0]);

            return sum == wholeArea;
        }

        private static int GetTriangleArea(Point p1, Point p2, Point p3)
        {
            return Math.Abs(
                p1.X * (p2.Y - p3.Y) +
                p2.X * (p3.Y - p1.Y) +
                p3.X * (p1.Y - p2.Y)
            );
        }
    }
}
