using System;
using static System.Console;

namespace DesignPatterns.InnerFactory
{
    /// <summary>
    /// Factory method allows us to overload and use different and descriptive parameter names
    /// </summary>
    public class Point
    {
        private double x, y;

        private Point(double x, double y) // the caveat is that the constructor is now public
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// In DesignPatterns.Factory, we had a problem, were the constructor was left public to be accessible to PointFactory
        /// We've moved the factory inline
        /// </summary>
        public static class Factory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }

        public static Point Origin = new Point(0, 0); // alternative

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            WriteLine(Point.Factory.NewCartesianPoint(12, 4));
            WriteLine(Point.Factory.NewPolarPoint(12, 4));
            WriteLine(Point.Origin);

            ReadLine();
        }
    }
}
