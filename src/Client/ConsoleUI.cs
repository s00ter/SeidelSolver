using Client.Network;
using Client.Readers;
using Common.Models;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class ConsoleUI
    {
        private readonly SolverClient _client;

        public ConsoleUI(string url)
        {
            _client = new SolverClient(url);
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var slaeData = await ReadSlaeDataAsync();
                Console.WriteLine();
                var x = await _client.SendRequestToSolveAsync(slaeData);

                for (int i = 0; i < x.Length; i++)
                {
                    Console.WriteLine(x[i]);
                }

                Console.WriteLine();
            }
        }

        private async Task<SlaeData> ReadSlaeDataAsync()
        {
            Console.Write("Путь к файлу с матрицей: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            var matrixPath = Console.ReadLine();
            Console.ResetColor();
            var matrixTask = Task.Run(() => TxtReader.ReadMatrix(matrixPath));
            Console.Write("Путь к файлу с вектором свободных членов: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            var vectorPath = Console.ReadLine();
            Console.ResetColor();
            var vectorTask = Task.Run(() => TxtReader.ReadVector(vectorPath));
            var slaeData = new SlaeData(await matrixTask, await vectorTask);
            return slaeData;
        }
    }
}