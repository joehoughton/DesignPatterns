using System;
using System.Collections.Generic;
using static System.Console;

/// <summary>
/// Functional approaches conforms to open-closed principle
/// Achieved by using extensions
/// </summary>
namespace DesignPatterns.FunctionalBuilder
{
    public class Person
    {
        public string Name, Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    public class PersonBuilder
    {
        public readonly List<Action<Person>> Actions = new List<Action<Person>>();

        public PersonBuilder Called(string name)
        {
            Actions.Add(p => { p.Name = name; });
            return this;
        }

        public Person Build()
        {
            var p = new Person();

            Actions.ForEach(a => a(p));

            return p;
        }
    }

    /// <summary>
    /// We do not add to the PersonBuilder, assume it has been shipped to the client
    /// Instead we add extensions
    /// </summary>
    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAs(this PersonBuilder builder, string position)
        {
            builder.Actions.Add(p => { p.Position = position; });
            return builder;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Person me = new PersonBuilder().Called("Joe").WorksAs("Software Developer").Build();
            WriteLine(me);

            ReadLine();
        }
    }
}