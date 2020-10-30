using System;
using static System.Console;
using System.Collections.Generic;

namespace DesignPatterns.AbstractFactory
{
    /// <summary>
    /// Factory method pattern is a creational pattern that uses factory methods to deal with the problem of creating objects
    /// without having to specify the exact class of the object that will be created
    /// 
    /// Abstract factory is used to give out abstract objects as opposed to concrete objects
    /// Demonstrated by our IHotDrinkFactory
    /// </summary>
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This is nice, but I'd prefer it with milk");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffee is delicious");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Put in a tea bag, boil water, pour {amount} ml, add lemon");

            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar");

            return new Coffee();
        }
    }

    /// <summary>
    /// Breaks Open-Closed principle, we have to go into the hot drink machine to add another AvailableDrink, e.g. hot chocolate
    /// </summary>
    public class HotDrinkMachine
    {
        public enum AvailableDrink
        {
            Coffee, Tea
        }

        private readonly Dictionary<AvailableDrink, IHotDrinkFactory> _factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory = (IHotDrinkFactory) Activator.CreateInstance(Type.GetType("DesignPatterns.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));

                _factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
            return _factories[drink].Prepare(amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Coffee coffee = (Coffee)new CoffeeFactory().Prepare(3); // no - we can make a coffee without going to the factory!

            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 1);
            drink.Consume();

            ReadLine();
        }
    }
}
