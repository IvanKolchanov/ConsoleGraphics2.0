using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp6
{
    class Direct : Figure
    {
        private static int width = Program.width, height = Program.height;

        private int id = 0;
        private double k, b;
        private List<PointOnScreen> pointsOnScreen = new List<PointOnScreen>();
        public double K { get { return k; } set { k = value; updateInfo(); } }
        public double B { get { return b; } set { b = value; updateInfo(); } }

        public Direct(double k, double b)
        {
            screen = new char[width, height];
            this.k = k;
            this.b = b;
            updateScreen();
            DrawFigure.figures.Add(new Figure(screen));
            id = DrawFigure.figures.Count - 1;
            DrawFigure.addFigure(screen, pointsOnScreen);
        }

        public Direct(Coordinates.Point A, Coordinates.Point B)
        {
            screen = new char[width, height];
            Coordinates.Direct direct = Coordinates.findDirectByPoints(A, B);
            this.k = direct.k;
            this.b = direct.b;
            updateScreen();
            DrawFigure.figures.Add(new Figure(screen));
            id = DrawFigure.figures.Count - 1;
            DrawFigure.addFigure(screen, null);
        }

        public void turn(int angelInDegrees)
        {
            double currentAngel = Math.Atan(k);
            currentAngel *= 180 / Math.PI;
            currentAngel += angelInDegrees;
            currentAngel *= Math.PI / 180;
            K = Math.Tan(currentAngel);
        }

        private void updateScreen()
        {
            pointsOnScreen.Clear();
            if (k == 0)
            {
                b = Coordinates.normalizeY(b);
                k = 0;
                for (int j = 0; j <= height - 1; j++)
                {
                    double y = Coordinates.jToY(j);
                    for (int i = 0; i < width; i++)
                    {
                        if (y == this.b) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
                        else screen[i, j] = ' ';
                    }
                }
            }
            else if (k == int.MaxValue)
            {
                k = int.MaxValue;
                b = Coordinates.normalizeX(b);
                for (int j = 0; j <= height - 1; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        double x = Coordinates.iToX(i);
                        if (x == b) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
                        else screen[i, j] = ' ';
                    }
                }
            }
            else
            {
                b = Coordinates.normalizeX(b);
                for (int j = 0; j <= height - 1; j++)
                {
                    double y = Coordinates.jToY(j);
                    double approximateX = Coordinates.normalizeX((y - this.b) / k);
                    approximateX = Math.Round(approximateX, 4);
                    for (int i = 0; i < width; i++)
                    {
                        double x = Coordinates.iToX(i);
                        x = Math.Round(x, 4);
                        if (approximateX == x) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
                        else screen[i, j] = ' ';
                    }
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
