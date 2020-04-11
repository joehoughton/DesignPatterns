using static System.Console;

/// <summary>
/// Inheriting fluent interfaces
/// </summary>
namespace DesignPatterns.FluentBuilderRecursiveGenerics
{
    public class Person
    {
        public string Name;

        public string Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }

        public class Builder : BetterPersonJobBuilder<Builder>
        {
        }

        public static Builder New => new Builder(); // when we try to construct new person, it gives us a new builder
    }

    public class PersonInfoBuilder
    {
        protected Person person = new Person();

        public PersonInfoBuilder Called(string name)
        {
            person.Name = name;
            return this;
        }
    }

    /// <summary>
    /// New requirement comes along to include job builder, that includes info builder
    /// We opt for open-closed principle, so we go for inheritance
    /// Problem: unable to use fluent interface builder.Called("Joe").WorksAs("Developer");
    /// Why? Returning this from PersonJobBuilder returns reference to interface PersonInfoBuilder
    /// </summary>
    public class PersonJobBuilder : PersonInfoBuilder
    {
        public PersonJobBuilder WorksAs(string position)
        {
            person.Position = position;
            return this;
        }
    }
    
    public class BetterPersonJobBuilder<Self>: BetterPersonInfoBuilder<BetterPersonJobBuilder<Self>> where Self : BetterPersonJobBuilder<Self>
    {
        public Self WorksAs(string position)
        {
            person.Position = position;
            return (Self) this;
        }
    }

    public class BetterPersonInfoBuilder<Self> : PersonBuilder where Self : BetterPersonInfoBuilder<Self>
    {
        public Self Called(string name)
        {
            person.Name = name;
            return (Self) this;
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build()
        {
            return person;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new PersonJobBuilder();
            // builder.Called("Joe").WorksAs("Developer"); // WorksAs not available, PersonInfoBuilder returned

            // We cannot initialize BetterPersonJobBuilder, because it is unclear what we would specify as type:
            // new BetterPersonJobBuilder<???>()
            // We make new additional types, the Person now exposes it's own builder
            // All fluent interfaces now available:
            BetterPersonJobBuilder<Person.Builder> me = Person.New.Called("Joe").WorksAs("Software Developer");
            WriteLine(me.Build());

            ReadLine();
        }
    }
}