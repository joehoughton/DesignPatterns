using System;

/// <summary>
/// Objects in a program should be replaceable with instances of their subtypes without altering the correctness of that program
/// </summary>
namespace DesignPatterns.LiskovSubstitutionPrinciple
{
    public class Rectangle
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }
  
    /// <summary>
    /// Violates principle, cannot store square with a reference to rectangle as height not set
    /// </summary>
    public class Square : Rectangle
    {
        public new int Width
        {
            set
            {
                base.Width = base.Height = value;
            }
        }

        public new int Height
        {
            set
            {
                base.Height = base.Width = value;
            }
        }
    }

    public class BetterRectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public BetterRectangle()
        {
        }

        public BetterRectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class BetterSquare : BetterRectangle
    {
        public /*new*/ override int Width
        {
            set
            {
                base.Width = base.Height = value;
            }
        }

        public /*new*/ override int Height
        {
            set
            {
                base.Height = base.Width = value;
            }
        }
    }

    public class Program
    {
        static public int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            Rectangle rectangle = new Rectangle(2, 3);
            Console.WriteLine($"{rectangle} has area {Area(rectangle)}"); // 6

            Square square = new Square();
            square.Width = 4;
            Console.WriteLine($"{square} has area {Area(square)}"); // 16

            // It is legal to store reference to a square as a rectangle
            // This causes an issue, the area is now 0 as we are setting the width and not the height
            // Liskov principle says you should always be able to upcase to your base type and the operation should be okay
            // e.g. the square should behave as a square when you have a reference to a rectangle
            // We can fix this violation by making the Width and Height properties virtual
            // Even though we're holding a rectangle reference to a square, when the width is accessed
            // the virtual function table will be checked and the setter will be called on the square
            Rectangle square2 = new Square();
            square2.Width = 4;
            Console.WriteLine($"{square2} has area {Area(square2)} (violation)"); // 0

            BetterRectangle square3 = new BetterSquare();
            square3.Width = 4;
            Console.WriteLine($"{square3} has area {square3.Width * square3.Height} (fixed)"); // 16

            Console.ReadLine();
        }
    }
}
