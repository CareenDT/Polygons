using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Polygons.Shapes
{
    public abstract class Shape
    {
        protected static readonly Brush _br;

        protected int _x;
        protected int _y;
        protected static int _radius;

        protected bool _isMoving;
        protected int _moveDX;
        protected int _moveDY;

        public virtual int X { get { return _x; } set { _x = value; } }
        public virtual int Y { get { return _y; } set { _y = value; } }
        public int Radius { get { return _radius; } set { _radius = value; } }

        public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }
        public int MoveDX { get { return _moveDX; } set { _moveDX = value; } }
        public int MoveDY { get { return _moveDY; } set { _moveDY = value; } }

        public virtual void Draw(Graphics g) { }

        public virtual bool IsInside(int mouseX, int mouseY) => false;

        static Shape()
        {
            _br = new SolidBrush(Color.Black);

            _radius = 50;
        }

        public virtual void Move(int x, int y)
        {
            this._x = x + _moveDX;
            this._y = y + _moveDY;
        }

        protected Point GetPointOnCircle(float angle)
        {
            return new Point((int)(MathF.Cos(angle) * _radius), (int)(MathF.Sin(angle) * _radius));
        }
    }

    public class Square : Shape
    {
        public override int X
        {
            get => _x;
            set { _x = value; RecalculateExtents(); }
        }

        public override int Y
        {
            get => _y;
            set { _y = value; RecalculateExtents(); }
        }

        protected Rectangle extents;

        public override void Move(int x, int y)
        {
            this._x = x + _moveDX;
            this._y = y + _moveDY;
            RecalculateExtents();
        }

        public Square(int x, int y)
        {
            this._x = x;
            this._y = y;

            RecalculateExtents();
        }

        private void RecalculateExtents()
        {
            int corner = (int)(MathF.Cos(0.785398f) * _radius);
            extents.Width = corner * 2;
            extents.Height = corner * 2;

            extents.X = _x - corner;
            extents.Y = _y - corner;
        }

        public override void Draw(Graphics g)
        {

            g.FillRectangle(_br, extents);
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
            this._x = x;
            this._y = y;
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(_br, new Rectangle(_x - _radius, _y - _radius, _radius * 2, _radius * 2));
        }

        public override bool IsInside(int mouseX, int mouseY)
        {
            int relativeX = _x - mouseX;
            int relativeY = _y - mouseY;

            return MathF.Sqrt(relativeX * relativeX + relativeY * relativeY) <= _radius;
        }
    }

    public class Triangle : Shape
    {

        Point[] points = new Point[3];

        public Triangle(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public override void Draw(Graphics g)
        {
            Point sidePoint = GetPointOnCircle(0.523599f);
            points =
            [
                new(_x, _y - _radius),
                new(_x - sidePoint.X, _y + sidePoint.Y),
                new(_x + sidePoint.X, _y + sidePoint.Y)
            ];
            g.FillPolygon(_br, points);
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
