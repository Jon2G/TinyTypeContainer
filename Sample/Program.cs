using Sample.Types;
using TinyTypeContainer;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Container.Debug = true;
            Console.WriteLine("¿Hello, whats your name?");
            Container.GetorActivate<DummyType>().Name = Console.ReadLine();
            Console.WriteLine($"Hello {Container.GetRequired<DummyType>().Name}");


            var disposable = new DummyDisposableType();
            Container.Register<DummyDisposableType>(disposable);

            disposable.Buffer = Container.GetRequired<DummyType>().Name.ToArray().Select(x => (byte)x).ToArray();
            Console.WriteLine($"Buffer size: {disposable.Buffer.Length}");

            Console.WriteLine("Press any key to finish");
            Console.ReadKey();

            Console.WriteLine($"See you later,{Container.GetRequired<DummyType>().Name}");
            Container.Unregister<DummyType>();
            Container.Unregister<DummyDisposableType>();
        }
    }
}
