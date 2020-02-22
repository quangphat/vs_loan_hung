using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Utility
{
    public static class CoreApiClient
    {
        public static async Task<HttpClientResult<T>> Post<T>(this HttpClient httpClient, string basePath, string path, object data = null, string param = null)
        {
            
            var response = await httpClient.SendAsync(HttpMethod.Post, basePath, path, data, param);
            if (response != null && response.Content != null)
            {
                try
                {
                    var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(responseData);
                    return HttpClientResult<T>.Create(response.StatusCode, result, response.Headers?.ETag?.Tag, response.StatusCode == HttpStatusCode.NotModified);
                }
                catch(Exception e)
                {
                    return HttpClientResult<T>.Create(response.StatusCode, TypeExtensions.GetDefaultValue<T>(), response.Headers?.ETag?.Tag, response.StatusCode == HttpStatusCode.NotModified);
                }
                
            }
            return HttpClientResult<T>.Create(response.StatusCode, TypeExtensions.GetDefaultValue<T>(), response.Headers?.ETag?.Tag, response.StatusCode == HttpStatusCode.NotModified); 
        }
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpMethod httpMethod, string basePath, string path, object data = null, string param = null)
        {

            var url = $"{basePath}{path}";
            var requestMessage = new HttpRequestMessage(httpMethod, url);
            string json = null;

            HttpContent content = null;

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
            var signature = CreateSignature("VietbankFc");
            Dictionary<string, string> signatures = new Dictionary<string, string>() {
                { "X-VietbankFC-Signature",signature}
            };
            if (signatures != null && signatures.Count > 0)
            {
                foreach (var item in signatures)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }

            requestMessage.Content = content;
            return await httpClient.SendAsync(requestMessage);
        }
        public static async Task<T> Get<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null, bool includeSignature = false, string token = null)
        {
            var response = await httpClient.Call<T>(HttpMethod.Get, basePath, path, param, includeSignature: includeSignature);
            return response.Data;
        }
        //public static async Task<T> Post2<T>(this HttpClient httpClient, string basePath, string path = "/", object param = null, object data = null, bool includeSignature = false, string token = null)
        //{
        //    var response = await httpClient.Call<T>(HttpMethod.Post, basePath, path, param, data, includeSignature);
        //    return response.Data;
        //}
        
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
    public static class TypeExtensions
    {
        public static T GetDefaultValue<T>()
        {
            return (T)GetDefaultValue(typeof(T));
        }

        public static object GetDefaultValue(this Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
