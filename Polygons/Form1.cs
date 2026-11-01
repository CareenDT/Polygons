using Polygons.Shapes;
using System.Diagnostics;

namespace Polygons
{
    public partial class Form1 : Form
    {
        private readonly Pen _hullPen;
        private readonly List<Shape> _activeShapes;

        private int _selectedPlacementShape;

        public Form1()
        {
            _activeShapes = [
                new Square(200, 200),
                new Circle(200, 400),
                new Triangle(400, 200)
            ];

            _hullPen = new Pen(Color.DarkViolet);

            InitializeComponent();
        }

        private void DrawHullByCross(Graphics g)
        {
            for (int i = 0; i < _activeShapes.Count; i++)
            {
                for (int j = i + 1; j < _activeShapes.Count; j++)
                {
                    var a = _activeShapes[i];
                    var b = _activeShapes[j];

                    bool left = false, right = false;

                    for (int k = 0; k < _activeShapes.Count; k++)
                    {
                        if (k == i || k == j) continue;

                        var p = _activeShapes[k];
                        float cross = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);

                        if (cross > 0) left = true;
                        else if (cross < 0) right = true;

                        if (left && right) break;
                    }

                    if (!(left && right))
                        g.DrawLine(_hullPen, a.X, a.Y, b.X, b.Y);
                }
            }
        }

        private void DrawHullByDefinition(Graphics g)
        {
            int n = _activeShapes.Count;
            if (n < 2) return;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var a = _activeShapes[i];
                    var b = _activeShapes[j];

                    if (a.X == b.X)
                    {
                        bool allLeft = true;
                        bool allRight = true;

                        foreach (var p in _activeShapes)
                        {
                            if (p == a || p == b || p == a || p == b) continue;

                            if (p.X < a.X) allLeft = false;
                            else if (p.X > a.X) allRight = false;
                        }

                        if (allLeft || allRight)
                            g.DrawLine(_hullPen, a.X, a.Y, b.X, b.Y);

                        continue;
                    }

                    long D = (long)a.X - b.X;

                    bool allAbove = true;
                    bool allBelow = true;

                    foreach (var p in _activeShapes)
                    {
                        if (p == a || p == b) continue;

                        long expr = (a.Y - b.Y) * ((long)p.X - a.X)
                                  + (a.Y - p.Y) * D;
                        long s = (D > 0) ? expr : -expr;

                        if (s > 0) allBelow = false;
                        else if (s < 0) allAbove = false;
                    }

                    if (allAbove || allBelow)
                        g.DrawLine(_hullPen, a.X, a.Y, b.X, b.Y);
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawHullByDefinition(e.Graphics);
            foreach (Shape shape in _activeShapes)
            {
                shape.Draw(e.Graphics);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            bool isSomethingDragged = false;
            foreach (Shape shape in _activeShapes)
            {
                if (shape.IsMoving)
                {
                    shape.Move(e.X, e.Y);
                    isSomethingDragged = true;
                }
            }

            if (isSomethingDragged) Refresh();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            bool isSomethingDragged = false;

            foreach (Shape shape in _activeShapes)
            {
                if (!shape.IsInside(e.X, e.Y)) continue;

                if (e.Button == MouseButtons.Right)
                {
                    _activeShapes.Remove(shape);
                    Refresh();

                    return;
                }

                shape.MoveDX = shape.X - e.X;
                shape.MoveDY = shape.Y - e.Y;
                shape.IsMoving = true;

                isSomethingDragged = true;

            }

            if (!isSomethingDragged)
            {
                switch (_selectedPlacementShape)
                {
                    case 1:
                        _activeShapes.Add(new Circle(e.X, e.Y));

                        Refresh();

                        return;
                    case 2:
                        _activeShapes.Add(new Square(e.X, e.Y));

                        Refresh();

                        return;
                    case 3:
                        _activeShapes.Add(new Triangle(e.X, e.Y));

                        Refresh();

                        return;
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (Shape shape in _activeShapes)
                {
                    shape.IsMoving = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedPlacementShape = 3;
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedPlacementShape = 2;
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedPlacementShape = 1;
        }
    }
}
