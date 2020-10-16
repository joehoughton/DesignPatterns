using System;
using static System.Console;

namespace DesignPatterns.FactoryMethod
{
    /// <summary>
    /// A Factory Pattern or Factory Method Pattern says just define an interface or abstract class for creating an object but let the subclasses decide which class to instantiate
    /// In other words, subclasses are responsible to create the instance of the class
    /// Achieved by returning instance of it's private constructor
    /// A factory method is a static method that creates objects
    /// A factory can take care of object creation
    /// A factory can be external or reside inside the object as an inner class
    /// Factory method allows us to overload and use different and descriptive parameter names
    /// </summary>
    public class Point
    {
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        private double x, y;

        private Point(double x, double y)
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
            WriteLine(Point.NewPolarPoint(12, 4));

            ReadLine();
        }
    }
}
