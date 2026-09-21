using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Polygons.Shapes
{
    public abstract class Shape
    {
        protected static readonly Pen pen;

        protected Point position;
        protected Size size;

        public Point Position { get { return position; } set { position = value; } }
        public Size Size { get { return size; } set { size = value; } }

        public virtual void Draw(Graphics g) { }

        static Shape()
        {
            pen = new Pen(new SolidBrush(Color.Gray));
        }
    }

    public class Square : Shape
    {
        public Square(Point position, Size size)
        {
            this.position = position;
            this.size = size;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(pen, new Rectangle(position.X - size.Width / 2, position.Y - size.Height / 2, size.Width, size.Height));
        }
    }

    public class Circle : Shape
    {
        public Circle(Point position, Size size)
        {
            this.position = position;
            this.size = size;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(pen, new Rectangle(position.X - size.Width / 2, position.Y - size.Height / 2, size.Width, size.Height));
        }
    }

    public class Triangle : Shape
    {
        public Triangle(Point position, Size size)
        {
            this.position = position;
            this.size = size;
        }

        public override void Draw(Graphics g)
        {
            Point[] points =
            [
                new(position.X - size.Width / 2, position.Y + size.Height / 2),
                new(position.X + size.Width / 2, position.Y + size.Height / 2),
                new(position.X, position.Y - size.Height / 2),
            ];
            g.DrawPolygon(pen, points);
        }
    }
}
