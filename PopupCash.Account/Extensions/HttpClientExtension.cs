using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using PopupCash.Account.Models.Authenthications.Exceptions;

namespace PopupCash.Account.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddDefaultHeaders(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PcPomissionSDK");
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<T> ReadContentFormJsonAsync<T>(this HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (httpResponse.StatusCode == HttpStatusCode.Forbidden ||
                httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthenticationException(content);
            }

            if (JsonConvert.DeserializeObject<T>(content) is T response)
                return response;

            throw new Exception($"Get {typeof(T).Name} response failed.");
        }

        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string uri)
        {
            using HttpResponseMessage httpResponse = await httpClient.GetAsync(uri).ConfigureAwait(false);

            return await httpResponse.ReadContentFormJsonAsync<T>();
        }

        public static async Task<T> PostAsync<T>(this HttpClient httpClient, string uri, object? requestValue)
        {
            string requestBody = JsonConvert.SerializeObject(requestValue);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(uri, requestBody.TotJsonTypeStringConten()).ConfigureAwait(false);

            return await httpResponse.ReadContentFormJsonAsync<T>();
        }


        public static async Task<T> PostAsync<T>(this HttpClient httpClient, string uri, StringContent requestBody)
        {
            using HttpResponseMessage httpResponse = await httpClient.PostAsync(uri, requestBody).ConfigureAwait(false);

            return await httpResponse.ReadContentFormJsonAsync<T>();
        }
    }
}
