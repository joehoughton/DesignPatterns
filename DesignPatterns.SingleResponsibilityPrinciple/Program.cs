using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Every module or class should have responsibility over a single part of the functionality provided by the software
/// That responsibility should be entirely encapsulated by the class, module or function
/// </summary>
namespace DesignPatterns.SingleResponsibilityPrinciple
{
    /// <summary>
    /// Concerned with keeping a bunch of entries
    /// </summary>
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");

            return count; // memento
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        // Adding Save, Load methods here would violate the single responsibility principle
    }

    /// <summary>
    /// Concerned with persisting objects to files
    /// </summary>
    public class Persistence
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, journal.ToString());
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var journal = new Journal();
            journal.AddEntry("I cried today");
            journal.AddEntry("I ate a bug");
            Console.WriteLine(journal);

            var persitence = new Persistence();
            var filename = @"c:\temp\journal.txt";
            persitence.SaveToFile(journal, filename, true);

            Process.Start(filename);
            Console.ReadLine();
        }
    }
}
