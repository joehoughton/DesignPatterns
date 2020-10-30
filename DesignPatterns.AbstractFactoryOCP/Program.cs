using System;
using System.Collections.Generic;
using static System.Console;

namespace DesignPatterns.AbstractFactoryOCP
{
    /// <summary>
    /// Enhanced DesignPatterns.AbstractFactory:
    /// Conform to the Open-Closed principle by using reflection
    /// Ideally the factories would be initialized through DI
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
            WriteLine("This coffee is delicious!");
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
            WriteLine($"Grind coffee beans, boil water, pour {amount} ml, add cream and sugar");

            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        private readonly List<Tuple<string, IHotDrinkFactory>> _factories = new List<Tuple<string, IHotDrinkFactory>>();

        public HotDrinkMachine()
        {
            foreach (var type in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(type) && !type.IsInterface)
                {
                    _factories.Add(Tuple.Create(type.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(type)));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            WriteLine("Available drinks:");
            for (var index = 0; index < _factories.Count; index++)
            {
                var tuple = _factories[index];
                WriteLine($"{index}: {tuple.Item1}");
            }

            WriteLine("Select drink: ");
            while (true)
            {
                string input;

                if ((input = ReadLine()) != null
                    && int.TryParse(input, out int i)
                    && i >= 0
                    && i < _factories.Count)
                {
                    WriteLine("Specify amount of water (ml): ");
                    input = ReadLine();

                    if (input != null && int.TryParse(input, out int amount) && amount > 0)
                    {
                        return _factories[i].Item2.Prepare(amount);
                    }
                }

                WriteLine("Incorrect input, try again: ");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
            drink.Consume();

            ReadLine();
        }
    }
}
