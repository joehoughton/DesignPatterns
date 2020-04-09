using System;
using System.Collections.Generic;

/// <summary>
/// Open to extension, closed for modification
/// Solution: Specification Pattern
/// </summary>
namespace DesignPatterns.OpenClosedPrinciple
{
    public enum Colour
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large
    }

    public class Product
    {
        public string Name;
        public Colour Colour;
        public Size Size;

        public Product(string name, Colour colour, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Colour = colour;
            Size = size;
        }
    }

    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var product in products)
                if (product.Size == size)
                    yield return product;
        }

        public IEnumerable<Product> FilterByColour(IEnumerable<Product> products, Colour colour)
        {
            foreach (var product in products)
                if (product.Colour == colour)
                    yield return product;
        }

        // Now the client wants to do both...
        // This breaks the open-closed principle
        // The ProductFilter class should be open to extension, but closed for modification
        // Solution: Specification Pattern
        [Obsolete]
        public IEnumerable<Product> FilterBySizeAndColour(IEnumerable<Product> products, Size size, Colour colour)
        {
            foreach (var product in products)
                if (product.Size == size && product.Colour == colour)
                    yield return product;
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColourSpecification : ISpecification<Product>
    {
        Colour colour;

        public ColourSpecification(Colour colour)
        {
            this.colour = colour;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Colour == colour;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T> // combinator
    {
        ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class BetterProductFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
            {
                if (spec.IsSatisfied(i))
                    yield return i;
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Colour.Green, Size.Small);
            var tree = new Product("Tree", Colour.Green, Size.Large);
            var house = new Product("House", Colour.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var productFilter = new ProductFilter();
            Console.WriteLine("Green products (old filter):");
            foreach(var product in productFilter.FilterByColour(products, Colour.Green))
            {
                Console.WriteLine($" - {product.Name} is green");
            }

            var betterProductFilter = new BetterProductFilter();
            Console.WriteLine("Green products (new filter):");
            foreach (var product in betterProductFilter.Filter(products, new ColourSpecification(Colour.Green)))
            {
                Console.WriteLine($" - {product.Name} is green");
            }

            Console.WriteLine("Green products (new filter):");
            foreach (var product in betterProductFilter.Filter(products, new SizeSpecification(Size.Large)))
            {
                Console.WriteLine($" - {product.Name} is large");
            }

            Console.WriteLine("Large blue products (new filter):");
            foreach (var product in betterProductFilter.Filter(products, new AndSpecification<Product>(new SizeSpecification(Size.Large), new ColourSpecification(Colour.Blue))))
            {
                Console.WriteLine($" - {product.Name} is big and blue");
            }

            Console.ReadLine();
        }
    }
}