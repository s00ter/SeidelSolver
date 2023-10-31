using Server.Services;
using System;

namespace Server.Network
{
    public class SolverServerSettings
    {
        public SolverServerSettings(ConsoleWriter writer, string url, int minThreadsCount, int maxThreadsCount)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            MinThreadsCount = minThreadsCount;
            MaxThreadsCount = maxThreadsCount;
        }

        public ConsoleWriter Writer { get; }

        public string Url { get; }

        public int MinThreadsCount { get; }

        public int MaxThreadsCount { get; }
    }
}