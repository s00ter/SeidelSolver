using Common.Models;
using Newtonsoft.Json;
using Server.Math;
using Server.Services;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Server.Network
{
    /// <summary>
    /// Представляет серверную часть, многопоточно решающую СЛАУ клиентов.
    /// </summary>
    public class SolverServer : IDisposable
    {
        private readonly HttpListener _httpListener;
        private readonly ConsoleWriter _writer;
        private bool _disposedValue;

        /// <summary>
        /// Инициализирует новый объект типа <see cref="SolverServer"/>.
        /// </summary>
        /// <param name="settings">Настройки сервера.</param>
        /// <exception cref="ArgumentNullException">Если настройки сервера являются null.</exception>
        public SolverServer(SolverServerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(settings.Url);
            _writer = settings.Writer;
            SetUpThreadPool(settings.MinThreadsCount, settings.MaxThreadsCount);
        }

        /// <summary>
        /// Решает СЛАУ согласно запросов клиентов, после чего отправляет http-ответ в формате json каждому клиенту.
        /// </summary>
        public void Start()
        {
            _httpListener.Start();
            _writer.Write("Ожидание подключения клиентов...\n");

            while (true)
            {
                var context = _httpListener.GetContext();
                Console.WriteLine(context.Request.Url.ToString());
                ThreadPool.QueueUserWorkItem(SolveInThreadPool, context);
            }
        }

        /// <summary>
        /// Закрывает соединение с http клиентом.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _httpListener.Stop();
                    _httpListener.Close();
                }

                _disposedValue = true;
            }
        }

        private void SolveInThreadPool(object state)
        {
            var context = (HttpListenerContext)state;
            var request = context.Request;

            try
            {
                var slaeData = JsonConvert.DeserializeObject<SlaeData>(GetRequestData(request));
                _writer.Write($"Решение СЛАУ клиента {request.UserHostName}...");
                var x = SeidelSolver.Solve(slaeData.Matrix, slaeData.Vector);
                _writer.Write($"СЛАУ решено для клиента {request.UserHostName}. Идёт отправка данных...");
                var xJson = JsonConvert.SerializeObject(x);

                using (var response = context.Response)
                {
                    SendResponse(response, xJson);
                }
            }
            catch (Exception ex)
            {
                _writer.Write($"Произошла ошибка: {ex.Message}");
            }
        }

        private static void SetUpThreadPool(int minThreadsCount, int maxThreadsCount)
        {
            ThreadPool.SetMinThreads(minThreadsCount, minThreadsCount);
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
        }

        private static string GetRequestData(HttpListenerRequest request)
        {
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                return reader.ReadToEnd();
            }
        }

        private static void SendResponse(HttpListenerResponse response, string json)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = buffer.Length;

            using (var writer = response.OutputStream)
            {
                writer.Write(buffer, 0, buffer.Length);
                writer.Flush();
            }
        }
    }
}
