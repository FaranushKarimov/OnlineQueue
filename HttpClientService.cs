using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuee
{
    public sealed class HttpClientService
    {
        private static readonly HttpClient _Client = new HttpClient();

        public static HttpClientService Instance { get; } = new HttpClientService();
        public void SetUrl(string url)
        {
        }
        private HttpClientService()
        {
            _Client.BaseAddress = new Uri("http://newq.safoev.beget.tech/"); /*new Uri("https://term.alif.mobi/apiterm/"); */
            _Client.Timeout = TimeSpan.FromSeconds(30);
            ServicePointManager.DnsRefreshTimeout = 120 * 60 * 1000;
            ServicePointManager.DefaultConnectionLimit = 20;
        }

        public void Headers(Models.Setting setting)
        {
            // _Client.BaseAddress = new Uri(setting.URL);
            _Client.DefaultRequestHeaders.Clear();
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(setting.Token))
                _Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {setting.Token}");
        }

        public class ApiException : Exception
        {
            public int StatusCode { get; set; }
            public string Content { get; set; }
        }

        public async Task<T> Post<T>(string path, object obj)
        {
            HttpContent c = CreateHttpContent(obj);
            HttpResponseMessage response = await _Client.PostAsync(path, c);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<T>(stream);

            var content = await StreamToStringAsync(stream);
            throw new ApiException { StatusCode = (int)response.StatusCode, Content = content };
        }
        public async Task<T> PostMultypart<T>(string path, StringContent obj)
        {
            HttpResponseMessage response = await _Client.PostAsync(path, obj);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var res = DeserializeJsonFromStream<T>(stream);
                return res;
            }

            var content = await StreamToStringAsync(stream);
            throw new ApiException { StatusCode = (int)response.StatusCode, Content = content };
        }
        public async Task<T> Get<T>(string path)
        {
            HttpResponseMessage response = await _Client.GetAsync(path);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<T>(stream);

            var content = await StreamToStringAsync(stream);
            throw new ApiException { StatusCode = (int)response.StatusCode, Content = content };
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;
            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                {
                    content = await sr.ReadToEndAsync();
                }
            }
            return content;
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;
            using (var sr = new StreamReader(stream))
            {
                var jtr = new JsonTextReader(sr);
                var jr = new JsonSerializer { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };
                var searchResult = jr.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;
            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            }
            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None };
                var js = new JsonSerializer
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        public void Close()
        {
            _Client.Dispose();
        }
    }
}
