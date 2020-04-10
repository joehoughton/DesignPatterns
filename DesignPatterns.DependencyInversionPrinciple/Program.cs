using System.Collections.Generic;
using System.Linq;
using static System.Console;

/// <summary>
/// High level parts of the system should not depend on low level parts directly
/// Instead they should depend on abstraction
/// </summary>
namespace DesignPatterns.DependencyInversionPrinciple
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    /// <summary>
    /// Low-level module
    /// </summary>
    public class Relationships
    {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public List<(Person, Relationship, Person)> Relations => relations;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    /// <summary>
    /// Better low-level module
    /// Instead of depending on the low level module, we're going to depend on the abstraction (IRelationshipBrowser)
    /// </summary>
    public class BetterRelationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations.Where(x => x.Item1.Name == name && x.Item2 == Relationship.Parent).Select(r => r.Item3);
        }
    }

    /// <summary>
    /// High-level module
    /// How do we access the low level relations in the high level component?
    /// We could expose the relations with a pubic property named Relations
    /// We are breaking the dependency principle as now, relationships cannot freely change how it stores data
    /// i.e. from a tuple to a dictionary
    /// We can solve this by providing a form of abstraction, using interfaces (IRelationshipBrowser)
    /// </summary>
    public class Research
    {
        public Research(Relationships relationships)
        {
            var relations = relationships.Relations;
            foreach(var r in relations.Where(x =>
                x.Item1.Name == "John" && x.Item2 == Relationship.Parent
            ))
            {
                WriteLine($"John has a child called {r.Item3.Name}");
            }
        }        
    }

    /// <summary>
    /// Instead of depending on the low level module, we're depending on the abstraction
    /// We're not directly accessing the Relations, but accessing them through the interface method FindAllChildrenOf
    /// </summary>
    public class BetterResearch
    {
        public BetterResearch(IRelationshipBrowser browser)
        {
            foreach (var c in browser.FindAllChildrenOf("John"))
            {
                WriteLine($"John has a child called {c.Name}");
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);
            new Research(relationships);

            var betterRelationships = new BetterRelationships();
            betterRelationships.AddParentAndChild(parent, child1);
            betterRelationships.AddParentAndChild(parent, child2);
            new BetterResearch(betterRelationships);

            ReadLine();
        }
    }
}