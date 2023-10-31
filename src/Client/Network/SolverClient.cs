using Common.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Network
{
    /// <summary>
    /// Предсталяет клиентскую часть, отправляющую данные на сервер.
    /// </summary>
    public class SolverClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private bool _disposedValue;

        /// <summary>
        /// Инициализирует новый объект типа <see cref="SolverClient"/>.
        /// </summary>
        /// <param name="url">Url-адрес, на который необходимо отправлять http-запросы.</param>
        /// <exception cref="ArgumentNullException">Если url является null.</exception>
        public SolverClient(string url)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _httpClient = new HttpClient
            {
                Timeout = new TimeSpan(0, 0, 0, 0, -1)
            };
        }

        /// <summary>
        /// Асинхронно отправляет http-запрос в формате json на решение СЛАУ.
        /// </summary>
        /// <param name="slaeData">Данные для решения СЛАУ.</param>
        /// <returns>Объект типа <see cref="Task"/>, содержащий вектор ответов.</returns>
        public async Task<float[]> SendRequestToSolveAsync(SlaeData slaeData)
        {
            var json = JsonConvert.SerializeObject(slaeData);

            using (var response = await SendRequestAsync(json, HttpMethod.Post))
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<float[]>(content);
            }
        }

        /// <summary>
        /// Закрывает соединение с http сервером.
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
                    _httpClient.Dispose();
                }

                _disposedValue = true;
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(string json, HttpMethod httpMethod)
        {
            using (var request = new HttpRequestMessage(httpMethod, _url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            })
            {
                return await _httpClient.SendAsync(request);
            }
        }
    }
}