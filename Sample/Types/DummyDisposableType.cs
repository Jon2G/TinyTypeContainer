namespace Sample.Types
{
    internal class DummyDisposableType : IDisposable
    {
        public byte[] Buffer { get; set; }
        public DummyDisposableType()
        {
            Buffer = new byte[1024];
        }
        public void Dispose()
        {
            Buffer = null;
            Console.WriteLine("DummyDisposableType has been disposed");
        }
    }
}
