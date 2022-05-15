using System;
using System.Collections.Generic;

namespace ConsoleApp6
{
    class Line : Figure
    {
        private static int width = Program.width, height = Program.height;
        private int id = 0;
        private double k, b;
        private double x1, y1, x2, y2;
        private double middlePointX;
        private double middlePointY;
        private List<PointOnScreen> pointsOnScreen = new List<PointOnScreen>();

        public Line(double x1, double y1, double x2, double y2)
        {
            screen = new char[width, height];
            this.x2 = Coordinates.normalizeX(x2);
            this.x1 = Coordinates.normalizeX(x1);
            this.y2 = Coordinates.normalizeY(y2);
            this.y1 = Coordinates.normalizeY(y1);
            middlePointX = (x1 + x2) / 2;
            middlePointY = (y1 + y2) / 2;
            updateScreen();
            DrawFigure.figures.Add(new Figure(screen));
            id = DrawFigure.figures.Count - 1;
            DrawFigure.addFigure(screen, pointsOnScreen);
        }

        public void turnLine(int angel)
        {
            double vectorX = x2 - x1, vectorY = y2 - y1;
            Coordinates.Point newVector = Coordinates.turnVector(new Coordinates.Point(vectorX, vectorY), angel);
            double newX = newVector.x;
            double newY = newVector.y;
            x1 = middlePointX - newX / 2;
            y1 = middlePointY - newY / 2;
            x2 = middlePointX + newX / 2;
            y2 = middlePointY + newY / 2;
            updateInfo();
        }
        private void updateScreen()
        {
            pointsOnScreen.Clear();
            Coordinates.Point A = new Coordinates.Point(x1, y1);
            Coordinates.Point B = new Coordinates.Point(x2, y2);
            Coordinates.Direct direct = Coordinates.findDirectByPoints(A, B);
            k = direct.k;
            b = Coordinates.normalizeX(direct.b);
            for (int j = 0; j <= height - 1; j++)
            {
                double y = Coordinates.jToY(j);
                double approximateX = Coordinates.normalizeX((y - b) / k);
                approximateX = Math.Round(approximateX, 4);

                for (int i = 0; i < width; i++)
                {
                    double x = Coordinates.iToX(i);
                    x = Math.Round(x, 4);
                    bool check = (approximateX == x);
                    bool checkLine = x >= Math.Min(x1, x2) && x <= Math.Max(x1, x2);
                    if (direct.k == 0)
                    {
                        check = y == direct.b;
                    }
                    if (direct.k == int.MaxValue)
                    {
                        check = x == direct.b;
                        checkLine = y >= Math.Min(y1, y2) && y <= Math.Max(y1, y2);
                    }
                    if (check && checkLine) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
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
