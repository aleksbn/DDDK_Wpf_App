using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class GenericDAL
    {
        private static readonly string BaseUrl = "https://localhost:7056/api/";
        private static async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, object data, string token)
        {
            using(HttpClient client = new HttpClient())
            {
                if(!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                HttpRequestMessage request = new HttpRequestMessage(method, BaseUrl + url);

                if(data != null)
                {
                    string json = JsonSerializer.Serialize(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                return await client.SendAsync(request);
            }
        }
        public static async Task<string> SendPostRequestAsync(string url, object data, string token)
        {
            HttpResponseMessage response = await SendRequestAsync(HttpMethod.Post, url, data, token);
            return await HandleResponseAsync(response);
        }
        public static async Task<string> SendPutRequestAsync(string url, object data, string token)
        {
            HttpResponseMessage response = await SendRequestAsync(HttpMethod.Put, url, data, token);
            return await HandleResponseAsync(response);
        }
        public static async Task<HttpResponseMessage> SendGetRequestAsync(string url, string token)
        {
            HttpResponseMessage response = await SendRequestAsync(HttpMethod.Get, url, null, token);
            return response;
        }
        public static async Task<string> SendDeleteRequestAsync(string url, string token)
        {
            HttpResponseMessage response = await SendRequestAsync(HttpMethod.Delete, url, null, token);
            return await HandleResponseAsync(response);
        }
        private static async Task<string> HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return "Done";
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
