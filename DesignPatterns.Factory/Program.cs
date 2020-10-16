using System;
using static System.Console;

namespace DesignPatterns.Factory
{
    /// <summary>
    /// It can be argued that the construction of an object is a separate responsibility from what the object actually does
    /// Here we move the factory methods from DesignPatterns.FactoryMethod into a separate class
    /// The caveat is that the constructor is now public
    /// Solved by DesignPatterns.InnerFactory
    /// </summary>
    public static class PointFactory
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

    /// <summary>
    /// Factory method allows us to overload and use different and descriptive parameter names
    /// </summary>
    public class Point
    {
        private double x, y;

        public Point(double x, double y) // the caveat is that the constructor is now public
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            WriteLine(PointFactory.NewPolarPoint(12, 4));

            ReadLine();
        }
    }
}
