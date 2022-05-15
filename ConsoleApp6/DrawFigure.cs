using System;
using System.Collections.Generic;

namespace ConsoleApp6
{
    static class DrawFigure
    {
        private static int width = Program.width, height = Program.height;
        private static char[,] axesX = new char[width, height], axesY = new char[width, height];
        private static bool firstAxesX = true, firstAxesY = true;
        public static List<Figure> figures = new List<Figure>();
        public static char[,] emptyArray = new char[width, height], fullArray = new char[width, height];
        private static List<PointOnScreen> pointsToDelete = new List<PointOnScreen>();
        private static List<PointOnScreen> pointsToDeleteNow = new List<PointOnScreen>();

        private static List<PointOnScreen> pointsOnOx = new List<PointOnScreen>(), pointsOnOy = new List<PointOnScreen>();

        public static void enableAxesX(bool command)
        {
            if (firstAxesX)
            {
                for (int j = 0; j <= height - 1; j++)
                {
                    double y = Coordinates.jToY(j);
                    for (int i = 0; i < width; i++)
                    {
                        if (y == 0) { axesX[i, j] = '@'; pointsOnOx.Add(new PointOnScreen(i, j)); }
                    }
                }
                firstAxesX = false;
            }
            figures.RemoveAt(0);
            if (command)
            {
                figures.Insert(0, new Figure(axesX));
                addFigure(axesX, pointsOnOx);
            }else
            {
                substractFigure(axesX, 0, pointsOnOx);
                figures.Insert(0, new Figure(emptyArray));
                addFigure(emptyArray, new List<PointOnScreen>());
            }
        }

        public static void enableAxesY(bool command)
        {
            if (firstAxesY)
            {
                for (int j = 0; j <= height - 1; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        double x = Coordinates.iToX(i);
                        if (x == 0) { axesY[i, j] = '@'; pointsOnOy.Add(new PointOnScreen(i, j)); }
                    }
                }
                firstAxesY = false;
            }
            figures.RemoveAt(1);
            if (command)
            {
                figures.Insert(1, new Figure(axesY));
                addFigure(axesX, pointsOnOy);
            }
            else
            {
                substractFigure(axesY, 1, pointsOnOy);
                figures.Insert(1, new Figure(emptyArray));
                addFigure(emptyArray, new List<PointOnScreen>());
            }
        }

        public static void refresh()
        {
            pointsToDeleteNow = new List<PointOnScreen>(pointsToDelete);
            pointsToDelete.Clear();
            Console.SetCursorPosition(0, 0);
            for (int j = 0; j <= height - 1; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (Program.screen[i, j] == '@')
                    {
                        pointsToDelete.Add(new PointOnScreen(i, j));
                        Console.SetCursorPosition(i, j);
                        Console.Write('@');
                    }
                }
            }
            for (int i = 0; i < pointsToDeleteNow.Count; i++)
            {
                PointOnScreen cursor = pointsToDeleteNow[i];
                if (!pointsToDelete.Exists(point => point.i == cursor.i && point.j == cursor.j))
                {
                    Console.SetCursorPosition(cursor.i, cursor.j);
                    Console.Write(' ');
                }
            }
        }

        private static bool checkForPixelCollision(int i1, int j1, int figureId)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                if (figures[i].screen[i1, j1] == '@' && i != figureId)
                {
                    return true;
                }
            }
            return false;
        }

        public static void addFigure(char[,] figureScreen, List<PointOnScreen> pointOnScreen)
        {
            for (int i = 0; i < pointOnScreen.Count; i++)
            {
                Program.screen[pointOnScreen[i].i, pointOnScreen[i].j] = '@';
            }
        }

        public static void substractFigure(char[,] figureScreen, int figureId, List<PointOnScreen> pointOnScreen)
        {
            for (int i = 0; i < pointOnScreen.Count; i++)
            {
                if (!checkForPixelCollision(pointOnScreen[i].i, pointOnScreen[i].j, figureId))
                {
                    Program.screen[pointOnScreen[i].i, pointOnScreen[i].j] = ' ';
                }
            }
                    
        }
        
    }
}
