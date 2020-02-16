using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Utility
{
    public static class CoreApiClient
    {
        public static async Task<T> Get<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null)
        {
            var response = await httpClient.Call<T>(HttpMethod.Get, basePath, path, param);
            return response.Data;
        }
        public static async Task<T> Post<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null, object data = null, int type = 0)
        {
            var response = await httpClient.Call<T>(HttpMethod.Post, basePath, path, param, data, type);
            return response.Data;
        }
        public static async Task<HttpResponseMessage> GetToken(this HttpClient httpClient, HttpMethod method, string basePath, string path = "/", string clientId = null, string clientSecret = null)
        {
            var request = buildHeader(method, basePath, path);
            var key = Utility.Base64Encode($"{clientId}:{clientSecret}");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", key);
            //request.Headers.Add("Authorization", $"Basic ${key}");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
        private static HttpRequestMessage buildHeader(
            HttpMethod method, string basePath, string path = "/", object param = null)
        {
            var url = $"{basePath}{path}";
            var requestMessage = new HttpRequestMessage(method, url);
            return requestMessage;
        }
        private static async Task<HttpClientResult<T>> Call<T>(this HttpClient httpClient,
            HttpMethod method, string basePath, string path = "/", object param = null, object data = null, int type = 0)
        {
            
            HttpContent content = null;
            string json = null;
            if (data != null)
                if (data is string)
                    content = new StringContent((string)data, Encoding.UTF8, "application/json");
                else if (data is IDictionary<string, object>)
                {
                    var formData = new MultipartFormDataContent();

                    foreach (var pair in data as IDictionary<string, object>)
                        if (pair.Value is byte[])
                            formData.Add(new ByteArrayContent(pair.Value as byte[]), pair.Key, pair.Key);
                        else
                            formData.Add(new StringContent(pair.Value.ToString()), pair.Key);

                    content = formData;
                }
                else
                {
                    json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }

            var originalData = string.Empty;
            if (data != null)
                originalData = json;

            if (string.IsNullOrWhiteSpace(originalData))
                originalData = string.Empty;
            var requestMessage = buildHeader(method, basePath, path);

            using (var response = await httpClient.SendAsync(requestMessage))
            {
                if (response.Content != null)
                {
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(responseData);
                    return HttpClientResult<T>.Create(response.StatusCode,result,response.Headers?.ETag?.Tag,response.StatusCode == HttpStatusCode.NotModified);
                }
                else
                    throw new Exception($"request to {path} error {response.StatusCode}");
            }
        }
    }
    public class HttpClientResult<T>
    {
        public static HttpClientResult<T> Create(HttpStatusCode statusCode, T data, string eTag, bool isCache)
        {
            return new HttpClientResult<T>()
            {
                StatusCode = statusCode,
                Data = data,
                ETag = eTag,
                IsCache = isCache
            };
        }

        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string ETag { get; set; }
        public bool IsCache { get; set; }
    }
}
