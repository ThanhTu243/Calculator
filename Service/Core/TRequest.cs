using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Service.Core
{
    public class TRequest
    {
        private readonly HttpClient _httpClient;
        public TRequest()
        {
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            _httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri("https://google.com")
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                return response;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public async Task<string> GetStringAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    return responseText;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HttpResponseMessage> PostAsync<K>(string url, K content)
        {
            try
            {
                StringContent stringContent;
                stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, stringContent);
                return response;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
