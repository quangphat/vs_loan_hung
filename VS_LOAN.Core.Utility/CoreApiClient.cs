using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Utility
{
    public static class CoreApiClient
    {
        public static async Task<T> Get<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null, bool includeSignature = false, string token = null)
        {
            var response = await httpClient.Call<T>(HttpMethod.Get, basePath, path, param, includeSignature: includeSignature);
            return response.Data;
        }
        public static async Task<T> Post<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null, object data = null, bool includeSignature = false, string token = null)
        {
            var response = await httpClient.Call<T>(HttpMethod.Post, basePath, path, param, data, includeSignature);
            return response.Data;
        }
        public static async Task<T> GetToken<T>(this HttpClient httpClient, string basePath, string path = "/", string clientId = null, string clientSecret = null)
        {
            var request = buildHeader(HttpMethod.Post, basePath, path);
            var key = Utility.Base64Encode($"{clientId}:{clientSecret}");
            var signature = CreateSignature("VietbankFc");
            request.Headers.Add("X-VietbankFC-Signature", signature);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", key);
            using (var response = await httpClient.SendAsync(request))
            {
                if (response.Content != null)
                {
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var data = JsonConvert.DeserializeObject<T>(responseData);
                    //var result = HttpClientResult<T>.Create(response.StatusCode, data, response.Headers?.ETag?.Tag, response.StatusCode == HttpStatusCode.NotModified);
                    return data;
                }
                else
                    throw new Exception($"request to {path} error {response.StatusCode}");
            }
        }
        private static HttpRequestMessage buildHeader(
            HttpMethod method, string basePath, string path = "/", object param = null)
        {
            var url = $"{basePath}{path}";
            var requestMessage = new HttpRequestMessage(method, url);
            return requestMessage;
        }
        private static async Task<HttpClientResult<T>> Call<T>(this HttpClient httpClient,
            HttpMethod method, string basePath, string path = "/", object param = null, object data = null, bool includeSignature = false, string token = null)
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

                    content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                }

            var originalData = string.Empty;
            if (data != null)
                originalData = json;

            if (string.IsNullOrWhiteSpace(originalData))
                originalData = string.Empty;
            var requestMessage = buildHeader(method, basePath, path);
            //if(method == HttpMethod.Post && data!=null)
            //{
            //    requestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            //    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //}
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (includeSignature)
            {
                var signature = CreateSignature("VietbankFc");
                requestMessage.Headers.Add("X-VietbankFC-Signature", signature);
            }
            if (!string.IsNullOrWhiteSpace(token))
            {
                requestMessage.Headers.Add("Authorization", "Bearer " + token);
            }

            using (var response = await httpClient.SendAsync(requestMessage))
            {
                if (response.Content != null)
                {
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(responseData);
                    return HttpClientResult<T>.Create(response.StatusCode, result, response.Headers?.ETag?.Tag, response.StatusCode == HttpStatusCode.NotModified);
                }
                else
                    throw new Exception($"request to {path} error {response.StatusCode}");
            }
        }
        private static string CreateSignature(string input)
        {
            return Utility.HmacSha256(input, "everbodyknowthatthecaptainlied");
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
