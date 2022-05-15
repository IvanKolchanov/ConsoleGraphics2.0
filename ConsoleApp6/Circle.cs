using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp6
{
    class Circle : Figure
    {
        private static int width = Program.width, height = Program.height;

        private int id = 0;
        private double x0, y0, radius;
        private List<PointOnScreen> pointsOnScreen = new List<PointOnScreen>();
        public double X0 { get { return x0; } private set { x0 = Coordinates.normalizeX(value); } }
        public double Y0 { get { return y0; } private set { y0 = Coordinates.normalizeY(value); } }
        public double Radius { get { return radius; } set { radius = Coordinates.normalizeX(value); updateInfo(); } }
        public Circle(double x0, double y0, double radius)
        {
            screen = new char[width, height];
            X0 = x0;
            Y0 = y0;
            this.radius = Coordinates.normalizeX(radius);
            updateScreen();
            DrawFigure.addFigure(screen, pointsOnScreen);
            DrawFigure.figures.Add(new Figure(screen));
            id = DrawFigure.figures.Count - 1;
        }

        public void moveTo(double newX0, double newY0)
        {
            X0 = newX0;
            Y0 = newY0;
            updateInfo();
        }

        private void updateScreen()
        {
            pointsOnScreen.Clear();
            for (int j = 0; j <= height - 1; j++)
            {
                double y = Coordinates.jToY(j);
                for (int i = 0; i < width; i++)
                {
                    double x = Coordinates.iToX(i);
                    if (Math.Pow(x - x0, 2) + Math.Pow(y - y0, 2) <= radius) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
                    else screen[i, j] = ' ';
                }
            }
        }

        private void updateInfo()
        {
            DrawFigure.substractFigure(screen, id, pointsOnScreen);
            DrawFigure.figures.RemoveAt(id);
            updateScreen();
            DrawFigure.figures.Insert(id, new Figure(screen));
            DrawFigure.addFigure(screen, pointsOnScreen);
        }
    }
}
