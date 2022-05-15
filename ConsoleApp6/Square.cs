using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp6
{
    class Square : Figure
    {
        private static int width = Program.width, height = Program.height;

        private int id = 0;
        private double x0, y0, side;
        private int angelInDegrees = 45;
        private Coordinates.Point A, B, C, D;
        private List<PointOnScreen> pointsOnScreen = new List<PointOnScreen>();
        private List<PointOnScreen> allPoints = new List<PointOnScreen>();
        public double X0 { get { return x0; } private set { x0 = Coordinates.normalizeX(value); } }
        public double Y0 { get { return y0; } private set { y0 = Coordinates.normalizeY(value); } }
        public double Side { get { return side; } set { side = Coordinates.normalizeX(value); updateInfo(); } }

        public int AngelInDegrees { get { return angelInDegrees; } set{ angelInDegrees = (int)value % 360; } }
        public Square(double x0, double y0, double side)
        {
            screen = new char[width, height];
            X0 = x0;
            Y0 = y0;
            side = Coordinates.normalizeX(side);
            for (int j = 0; j <= height - 1; j++)
            {
                double y = Coordinates.jToY(j);
                for (int i = 0; i < width; i++)
                {
                    double x = Coordinates.iToX(i);
                    if (Math.Abs(x + y - X0) + Math.Abs(x - y - Y0) <= side) { screen[i, j] = '@'; pointsOnScreen.Add(new PointOnScreen(i, j)); }
                    else screen[i, j] = ' ';
                    allPoints.Add(new PointOnScreen(i, j));
                }
            }
            A = new Coordinates.Point(X0 - side / 2, Y0 + side / 2);
            B = new Coordinates.Point(X0 + side / 2, Y0 + side / 2);
            C = new Coordinates.Point(X0 + side / 2, Y0 - side / 2);
            D = new Coordinates.Point(X0 - side / 2, Y0 - side / 2);
            DrawFigure.addFigure(screen, pointsOnScreen);
            DrawFigure.figures.Add(new Figure(screen));
            id = DrawFigure.figures.Count - 1;
        }

        public void turn(int angelInDegrees)
        {
            screen = DrawFigure.fullArray.Clone() as char[,];

            Coordinates.Point center = new Coordinates.Point(X0, Y0);
            A.substract(center); B.substract(center); C.substract(center); D.substract(center);
            A = Coordinates.turnVector(A, angelInDegrees);
            B = Coordinates.turnVector(B, angelInDegrees);
            C = Coordinates.turnVector(C, angelInDegrees);
            D = Coordinates.turnVector(D, angelInDegrees);
            A.add(center); B.add(center); C.add(center); D.add(center);

            updateInfo();
        }

        public void updateScreen()
        {
            pointsOnScreen = new List<PointOnScreen>(allPoints);
            List<Coordinates.Direct> directs = new List<Coordinates.Direct>();
            directs.Add(Coordinates.findDirectByPoints(A, B));
            directs.Add(Coordinates.findDirectByPoints(B, C));
            directs.Add(Coordinates.findDirectByPoints(C, D));
            directs.Add(Coordinates.findDirectByPoints(D, A));
            for (int z = 0; z < directs.Count; z++)
            {
                Coordinates.Direct cursor = directs[z];
                if (cursor.k == 0)
                {
                    int isUnder = 1;
                    if (Y0 > cursor.b) isUnder = -1;
                    for (int j = 0; j <= height - 1; j++)
                    {
                        double y = Coordinates.jToY(j);
                        for (int i = 0; i < width; i++)
                        {
                            if (y * isUnder >= cursor.b * isUnder) { screen[i, j] = ' ';
                                int index = pointsOnScreen.FindIndex(point => point.i == i && point.j == j);
                                if (index > -1) pointsOnScreen.RemoveAt(index);
                            }
                        }
                    }
                    continue;
                }
                if (cursor.k == int.MaxValue)
                {
                    int isRight1 = 1;
                    if (X0 < cursor.b) isRight1 = -1;
                    for (int j = 0; j <= height - 1; j++)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            double x = Coordinates.iToX(i);
                            x = Math.Round(x, 4);
                            if (x * isRight1 < cursor.b * isRight1)
                            {
                                screen[i, j] = ' ';
                                int index = pointsOnScreen.FindIndex(point => point.i == i && point.j == j);
                                if (index > -1) pointsOnScreen.RemoveAt(index);
                            }
                        }
                    }
                    continue;
                }
                bool isCenterRight = cursor.isPointRight(new Coordinates.Point(X0, Y0));
                int isRight = 1;
                if (!isCenterRight) isRight = -1;
                for (int j = 0; j <= height - 1; j++)
                {
                    double y = Coordinates.jToY(j);
                    double approximateX = Coordinates.normalizeX((y - cursor.b) / cursor.k);
                    approximateX = Math.Round(approximateX, 4);
                    for (int i = 0; i < width; i++)
                    {
                        double x = Coordinates.iToX(i);
                        x = Math.Round(x, 4);
                        if (x * isRight < approximateX * isRight)
                        {
                            screen[i, j] = ' ';
                            int index = pointsOnScreen.FindIndex(point => point.i == i && point.j == j);
                            if (index > -1) pointsOnScreen.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public void updateInfo()
        {
            DrawFigure.substractFigure(screen, id, pointsOnScreen);
            DrawFigure.figures.RemoveAt(id);
            updateScreen();
            DrawFigure.figures.Insert(id, new Figure(screen));
            DrawFigure.addFigure(screen, pointsOnScreen);
        }
    }
}
