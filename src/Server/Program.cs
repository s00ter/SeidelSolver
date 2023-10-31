using Server.Network;
using Server.Services;
using System;
using System.Configuration;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var url = ConfigurationManager.AppSettings.Get("url");
                var minThreadsCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("minThreadsCount"));
                var maxThreadsCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("maxThreadsCount"));
                var settings = new SolverServerSettings(new ConsoleWriter(), url, minThreadsCount, maxThreadsCount);
                var server = new SolverServer(settings);
                server.Start();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            Console.ReadKey();
        }
    }
}