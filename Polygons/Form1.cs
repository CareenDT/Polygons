using Polygons.Shapes;

namespace Polygons
{
    public partial class Form1 : Form
    {
        Square square1;
        Circle circle1;
        Triangle triangle1;
        public Form1()
        {
            square1 = new Square(200, 200);
            circle1 = new Circle(200, 400);
            triangle1 = new Triangle(400, 200);
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            square1.Draw(e.Graphics);
            circle1.Draw(e.Graphics);
            triangle1.Draw(e.Graphics);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Console.Write(square1.IsInside(e.X, e.Y));
        }
    }
}
