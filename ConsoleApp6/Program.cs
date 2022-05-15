using System;
using System.Threading;

namespace ConsoleApp6
{
    class Program
    {
        public static int width = 120;
        public static int height = 30;
        public static double aspect = (double)width / height;
        public static double pixelAspect = (double)8 / 16;
        public static char[,] screen = new char[width, height];

        private static void setup()
        {
            Console.CursorVisible = false;
            Console.Title = "Console 2D graphics";
            Console.SetWindowSize(width, height);
            Console.BufferWidth = width;
            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    screen[i, j] = ' ';
                    DrawFigure.emptyArray[i, j] = ' ';
                    DrawFigure.fullArray[i, j] = '@';
                }
            }
            DrawFigure.figures.Add(new Figure(DrawFigure.emptyArray));
            DrawFigure.figures.Add(new Figure(DrawFigure.emptyArray));
        }


        static void Main(string[] args)
        {
            setup();
            DrawFigure.enableAxesX(true);
            DrawFigure.enableAxesY(true);
            while (true)
            {
                DrawFigure.refresh();
                Thread.Sleep(50);
            }
            
        }
    }
}
