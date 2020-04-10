using System;

namespace DesignPatterns.InterfaceSegregationPrinciple
{
    /// <summary>
    /// No client should be forced to depend on methods it does not use
    /// Solution: decorator pattern
    /// </summary>
    public class Program
    {
        public class Document
        {
        }

        public interface IMachine
        {
            void Print(Document d);
            void Scan(Document d);
            void Fax(Document d);
        }

        public class MultiFunctionPrinter : IMachine
        {
            public void Print(Document d)
            {
                // implementation
            }

            public void Fax(Document d)
            {
                // implementation
            }

            public void Scan(Document d)
            {
                // implementation
            }
        }

        /// <summary>
        /// What do we do with the Fax and Scan methods that are not supported?
        /// Throw exception?
        /// Output that it is not supported?
        /// Document it only supplies the implementation of print?
        /// Solution: smaller atomic interfaces
        /// </summary>
        public class OldFashionedPrinter : IMachine
        {
            public void Print(Document d)
            {
                // implementation
            }

            public void Fax(Document d)
            {
                throw new NotImplementedException();
            }

            public void Scan(Document d)
            {
                throw new NotImplementedException();
            }
        }

        // option 1
        public interface IPrinter
        {
            void Print(Document d);
        }

        public interface IScanner
        {
            void Scan(Document d);
        }

        public class PhotoCopier : IPrinter, IScanner
        {
            public void Print(Document d)
            {
                // implementation
            }

            public void Scan(Document d)
            {
                // implementation
            }
        }

        // option 2
        public interface IMultiFunctionDevice : IScanner, IPrinter // ...
        {
        }

        /// <summary>
        /// We can implement the methods, or use delgation
        /// Solution: decorator pattern
        /// </summary>
        public class MultiFunctionMachine : IMultiFunctionDevice
        {
            // compose this out of several modules
            private IPrinter printer;
            private IScanner scanner;

            public MultiFunctionMachine(IPrinter printer, IScanner scanner)
            {
                this.printer = printer ?? throw new ArgumentNullException(paramName: nameof(printer));
                this.scanner = scanner ?? throw new ArgumentNullException(paramName: nameof(scanner));
            }

            public void Print(Document d)
            {
                printer.Print(d);
            }

            public void Scan(Document d)
            {
                scanner.Scan(d);
            }
        }

        static void Main(string[] args)
        {
        }
    }
}