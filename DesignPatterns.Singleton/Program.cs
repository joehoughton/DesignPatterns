using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq.Extensions;
using NUnit.Framework;

namespace DesignPatterns.Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }
    
    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount; // 0
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            instanceCount++;
            Console.WriteLine("Initializing database");

            capitals = File.ReadAllLines(Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt"))
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        public static SingletonDatabase Instance => instance.Value;
    }

    /// <summary>
    /// Hardcoded dependency to SingletonRecordFinder is removed in DesignPatterns.SingletonInDependencyInjection
    /// </summary>
    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += SingletonDatabase.Instance.GetPopulation(name);
            return result;
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.Instance;
            var city = "Tokyo";

            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
            Console.ReadLine();
        }
    }

    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;

            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalRecordPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] {"Seoul", "Mexico City"};
            int tp = rf.GetTotalPopulation(names);

            Assert.That(tp, Is.EqualTo(10));
            Assert.That(tp, Is.EqualTo(17400000 + 17500000));
        }
    }
}
