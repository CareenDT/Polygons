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
            square1 = new Square(new Point(200, 50), new Size(400,100));
            circle1 = new Circle(new Point(200, 200), new Size(200,200));
            triangle1 = new Triangle(new Point(100, 50), new Size(200, 100));
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            square1.Draw(e.Graphics);
            circle1.Draw(e.Graphics);
            triangle1.Draw(e.Graphics);
        }
    }
}
