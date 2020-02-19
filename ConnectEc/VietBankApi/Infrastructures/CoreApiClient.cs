using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;

namespace VietBankApi.Infrastructures
{
    public static class CoreApiClient
    {

        public static async Task<IActionResult> Get(this HttpClient httpClient, string basePath,
            HttpRequest request, string path = "/", object param = null, CurrentProcess process = null)
        {
            return await httpClient.Call(basePath, request, HttpMethod.Get, path, param, null, process);
        }

        public static async Task<IActionResult> Delete(this HttpClient httpClient, string basePath,
            HttpRequest request, string path = "/", object data = null, CurrentProcess process = null)
        {
            return await httpClient.Call(basePath, request, HttpMethod.Delete, path, null, data, process);
        }

        public static async Task<IActionResult> Post(this HttpClient httpClient, HttpRequest request,ILogBusiness logBusiness, string basePath,
             string path = "/", object param = null, object data = null, CurrentProcess process = null, string token = null)
        {
            return await httpClient.Call(basePath, request, HttpMethod.Post, path, param, data, process, token,logBusiness);
        }

        public static async Task<IActionResult> Put(this HttpClient httpClient, string basePath,
            HttpRequest request, string path = "/", object param = null, object data = null, CurrentProcess process = null)
        {
            return await httpClient.Call(basePath, request, HttpMethod.Put, path, param, data, process);
        }
        public static async Task<HttpClientResult<T>> SendRequestAsync<T>(
            this HttpClient httpClient, HttpRequest request, HttpMethod httpMethod, string basePath,
             string path = "/", object param = null, object data = null, CurrentProcess process = null)
        {
            var response = await httpClient.CallGetResponse(basePath, request, httpMethod, path, param, data, process);

            if (response != null)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Item2);

                if (result is ResponseJsonModel)
                {
                    var obj = result as ResponseJsonModel;

                    if (obj?.error?.code != null)
                        obj.error.message = "Lỗi";
                }

                return HttpClientResult<T>.Create(response.Item1, result, response.Item3, response.Item4);
            }
            else
                return HttpClientResult<T>.Create(response.Item1, TypeExtensions.GetDefaultValue<T>(), null, false);
        }
        public static async Task<T> GetToken<T>(this HttpClient httpClient, string basePath, string path = "/", string clientId = null, string clientSecret = null)
        {
            var url = $"{basePath}{path}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var key = Utils.Base64Encode($"{clientId}:{clientSecret}");
            var signature = CreateSignature("VietbankFc");
            requestMessage.Headers.Add("X-VietbankFC-Signature", signature);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", key);
            using (var response = await httpClient.SendAsync(requestMessage))
            {
                if (response.Content != null)
                {
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var data = JsonConvert.DeserializeObject<T>(responseData);
                    return data;
                }
                else
                    throw new Exception($"request to {path} error {response.StatusCode}");
            }
        }
        public static async Task<object> Step2(this HttpClient httpClient, ILogBusiness logBusiness, string basePath, string path = "/", string token = null)
        {
            try
            {
                var url = $"{basePath}{path}";
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Headers.Add("Authorization", string.Concat("Bearer ", token));
                //httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat("Bearer ", token));
                using (var response = await httpClient.SendAsync(requestMessage))
                {
                    if (response.Content != null)
                    {
                        var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                            ? response.Headers.Location.AbsoluteUri
                            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var data = JsonConvert.DeserializeObject<object>(responseData);
                        await logBusiness.LogInfo("step2 result 2 data", responseData);
                        return data;
                    }
                    else
                    {
                        await logBusiness.LogInfo("step2 result 2", "response = null");
                    }
                }
                return null;
            }
           catch(Exception e)
            {
                await logBusiness.LogInfo("Step2<T> step2 error", e.Message);
                return null;
            }

        }
        private static async Task<IActionResult> Call(this HttpClient httpClient, string basePath, HttpRequest request,
            HttpMethod method, string path = "/", object param = null, object data = null, CurrentProcess process = null, string token = null, ILogBusiness logBusiness = null)
        {
            try
            {
                if (param != null)
                    path = path.AddQuery(param);
                var url = $"{basePath}{path}";
                var requestMessage = new HttpRequestMessage(method, url);
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
                requestMessage.Content = content;


                //requestMessage.Headers.Add("Authorization", "Bearer " + token);
                //httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat("Bearer ", token));
                var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                if(logBusiness!=null)
                {
                    await logBusiness.LogInfo("Call api", "error here");
                }
                return new HttpContentActionResult(response.Content);
            }
            catch(Exception e)
            {
                if (logBusiness != null)
                {
                    await logBusiness.LogInfo("Call api error", "exception");
                }
                var stringContent = new StringContent("Excepption " + e.Message);
                return new HttpContentActionResult(stringContent);
            }
        }

        private static string CreateSignature(string input)
        {
            return Utils.HmacSha256(input, "everbodyknowthatthecaptainlied");
        }
        private static async Task<Tuple<HttpStatusCode, string, string, bool>> CallGetResponse(this HttpClient httpClient, string basePath, HttpRequest request,
            HttpMethod method, string path = "/", object param = null, object data = null, CurrentProcess process = null)
        {
            if (param != null)
                path = path.AddQuery(param);
            var url = $"{basePath}{path}";
            var requestMessage = new HttpRequestMessage(method, url);
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

            requestMessage.Headers.Add("Authorization", "Bearer " + process.Token);
            requestMessage.Content = content;
            using (var response = await httpClient.SendAsync(requestMessage))
            {
                if (response.Content != null)
                {
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return new Tuple<HttpStatusCode, string, string, bool>(
                        response.StatusCode,
                        responseData,
                        response.Headers?.ETag?.Tag,
                        response.StatusCode == HttpStatusCode.NotModified);
                }
                else
                    throw new Exception($"request to {url} error {response.StatusCode}");
            }
        }

    }
    public class HttpContentActionResult : IActionResult
    {
        private readonly HttpContent content;
        public HttpContentActionResult(HttpContent content)
        {
            this.content = content;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            using (var stream = await content.ReadAsStreamAsync())
            {
                context.HttpContext.Response.ContentType = content.Headers.ContentType.ToString();

                await stream.CopyToAsync(context.HttpContext.Response.Body);
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
    public class ResponseJsonModel
    {
        public ErrorJsonModel error { get; set; }
    }
    public class ResponseJsonModel<T> : ResponseJsonModel where T : class
    {
        public T data { get; set; }

        public void InitDefaultData(T defaultData)
        {
            error = null;
            data = defaultData;
        }
    }
    public class ErrorJsonModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<object> trace_keys { get; set; }
    }
    public class ResponseActionJsonModel : ResponseJsonModel
    {
        public bool? success { get; set; }
    }
}
