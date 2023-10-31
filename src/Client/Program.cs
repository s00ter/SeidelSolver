using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var url = ConfigurationManager.AppSettings.Get("url");
                var consoleUI = new ConsoleUI(url);
                await consoleUI.ShowAsync();
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