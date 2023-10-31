using System;

namespace Server.Services
{
    public class ConsoleWriter
    {
        private readonly object _lock = new object();

        public void Write(string data)
        {
            lock (_lock)
            {
                Console.WriteLine(data);
            }
        }
    }
}