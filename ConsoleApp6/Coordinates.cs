using System;

namespace ConsoleApp6
{
    class Coordinates
    {
        private static int width = Program.width, height = Program.height;
        private static double aspect = Program.aspect, pixelAspect = Program.pixelAspect;
        public static double normalizeX(double z)
        {
            return (double)((int)(z * (height * aspect * pixelAspect / 2))) / (height * aspect * pixelAspect / 2);
        }

        public static double normalizeY(double z)
        {
            return (double)((int)(z * (height / 2))) / (height / 2);
        }

        public static double iToX(int i)
        {
            double x = ((double)i / width) * 2 - 1;
            x *= aspect * pixelAspect;
            return x;
        }

        public static double jToY(int j)
        {
            double y = ((double)(height - 1 - j) / height) * 2 - 1;
            return y;
        }

        public static Point turnVector(Point point, int angelInDegrees)
        {
            double angelInRads = Math.PI * angelInDegrees / 180;
            double cos = Math.Cos(angelInRads), sin = Math.Sin(angelInRads);
            double newX = point.x * cos - point.y * sin;
            double newY = point.x * sin + point.y * cos;
            return new Point(newX, newY);
        }

        public static Direct findDirectByPoints(Point A, Point B)
        {
            double Ax = Math.Round(A.x, 4);
            double Bx = Math.Round(B.x, 4);
            double Ay = Math.Round(A.y, 4);
            double By = Math.Round(B.y, 4);
            if (Ay - By == 0) return new Direct(0, Ay);
            if (Ax - Bx == 0) return new Direct(int.MaxValue, Ax);
            double k = (Ay - By) / (Ax - Bx);
            double b = Coordinates.normalizeX(Ay - k * Ax);
            return new Direct(k, b);
        }

        public class Point
        {
            public double x { get; private set; }
            public double y { get; private set; }
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            public bool isEqual(Point B)
            {
                if (Math.Round(B.x, 4) == Math.Round(x, 4) && Math.Round(B.y, 4) == Math.Round(y, 4)) return true;
                return false;
            }

            public void add(Point point)
            {
                x += point.x;
                y += point.y;
            }

            public void substract(Point point)
            {
                x -= point.x;
                y -= point.y;
            }
        }

        public class Direct
        {
            public double k { get; }
            public double b { get; }

            public Direct(double k, double b)
            {
                this.k = k;
                this.b = b;
            }

            public bool isPointRight(Point point)
            {
                double D = point.x - point.y/k + b/k;
                if (D > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
