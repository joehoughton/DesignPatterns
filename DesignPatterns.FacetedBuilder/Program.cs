using static System.Console;

/// <summary>
/// Sometimes one builder is not enough, here we demonstrate multiple builders to build a single object
/// Solution: Facated
/// </summary>
namespace DesignPatterns.FacetedBuilder
{
    public class Person
    {
        // Address
        public string StreetAddress, Postcode, City;

        // Employment
        public string CompanyName, Position;

        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    /// <summary>
    /// Facade - doesn't build by itself but keeps a reference to the person that's being built up
    /// Allows access to sub builders
    /// </summary>
    public class PersonBuilder // facade 
    {
        // The object we're going to build
        protected Person person = new Person(); // this is a reference!

        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);
        public PersonJobBuilder Works => new PersonJobBuilder(person);

        /// <summary>
        /// Implicit conversion operator allows access to Person through PersonBuilder object by using Person type
        /// </summary>
        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        /// <summary>
        /// Pass into builder the object we're building up
        /// Store it in the field we inherit from PersonBuilder
        /// </summary>
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int annualIncome)
        {
            person.AnnualIncome = annualIncome;
            return this;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person) // might not work with a value type!
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();
            Person person = pb // accessing Person through implicit conversion
                .Works
                    .At("Solutions")
                    .AsA("Software Developer")
                    .Earning(100)
                .Lives
                    .At("Long Road")
                    .WithPostcode("G4D64F4")
                    .In("UK");

            WriteLine(person);
            ReadLine();
        }
    }
}