using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xmr_tutorials.Rpc
{
    public class RpcClient : IDisposable
    {
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializerSettings jsonSettings =
         new JsonSerializerSettings()
         {
             NullValueHandling = NullValueHandling.Ignore
         };

        public Task<RpcResponse<TResult>> CallAsync<TResult>(
            string url,
            string method)
        {
            return CallAsync<TResult>(url, new RpcRequestPayload(method));
        }

        public async Task<RpcResponse<TResult>> CallAsync<TResult>(
            string url,
            RpcRequestPayload payload)
        {
            var json = JsonConvert.SerializeObject(payload, jsonSettings);

            // Create a new HttpContent with the right encoding and charset
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType.CharSet = string.Empty;

            var result = await client.PostAsync(url, content);
            result.EnsureSuccessStatusCode();
            // Read it as string
            // Sometimes the daemon responds with text/plain but with JSON
            var stringResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert
                .DeserializeObject<RpcResponse<TResult>>(stringResult, jsonSettings);

            if (response.Error != null)
            {
                throw new RpcResponseException(response.Error);
            }
            return response;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}