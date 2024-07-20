using System.Diagnostics;

namespace TinyTypeContainer
{
    internal class DebugLog
    {
        public static void WriteLine(string message)
        {
            if (Container.Debug)
            {
                Debugger.Log(0, "Container", message + "\n");
            }
        }
    }
}
