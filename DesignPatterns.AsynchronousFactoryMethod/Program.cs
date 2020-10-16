using System.Threading.Tasks;

namespace DesignPatterns.AsynchronousFactoryMethod
{
    /// <summary>
    /// We cannot use async in constructors
    /// We could us an async method but the user might forget to call it, thus not initializing our class
    /// We solve this using an asynchronous factory method
    /// </summary>
    public class Foo
    {
        private Foo()
        {
        }

        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);

            return this;
        }

        public static Task<Foo> CreateAsync()
        {
            return new Foo().InitAsync();
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Foo x = await Foo.CreateAsync();
        }
    }
}
