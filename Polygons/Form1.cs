using Polygons.Shapes;
using System.Diagnostics;

namespace Polygons
{
    public partial class Form1 : Form
    {
        private readonly List<Shape> activeShapes;
        private readonly List<Shape> draggedShapes;

        public Form1()
        {
            activeShapes = [
                new Square(200, 200),
                new Circle(200, 400),
                new Triangle(400, 200)
            ];

            draggedShapes = [];

            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape shape in activeShapes)
            {
                shape.Draw(e.Graphics);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Shape shape in draggedShapes)
            {
                shape.X = e.X + shape.MoveDX;
                shape.Y = e.Y + shape.MoveDY;
            }

            if (draggedShapes.Count > 0)
            {
                Refresh();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Shape shape in activeShapes)
            {
                if (e.Button == MouseButtons.Left && shape.IsInside(e.X, e.Y))
                {
                    shape.MoveDX = shape.X - e.X;
                    shape.MoveDY = shape.Y - e.Y;

                    draggedShapes.Add(shape);
                }
            }

            if (draggedShapes.Count == 0)
            {
                activeShapes.Add(new Square(e.X, e.Y));
                Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draggedShapes.Clear();
            }
        }
    }
}
