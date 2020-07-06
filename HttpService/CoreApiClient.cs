using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace HttpService
{
    public static class CoreApiClient
    {
        public static async Task<ExternalApiResponseModel<T>> GetAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", object param = null)
        {
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, param);
            return response;
        }
        public static async Task<ExternalApiResponseModel<T>> PostAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", object param = null, object data = null)
        {
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, param, data);
            return response;
        }
        private static async Task<ExternalApiResponseModel<T>> Call<T>(this HttpClient httpClient,
             HttpRequestMessage requestMessage, string baseUrl, string path = "/", object param = null, object data = null)
        {
            if (param != null)
                path = path.AddQuery(param);
            if (path[0] != '/')
                path = $"/{path}";
            var url = $"{baseUrl}{path}";

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
            var signature = string.Empty;
            var originalData = string.Empty;
            if (requestMessage.Method == HttpMethod.Get)
            {
                var list = new List<string>();

                if (url.Contains("?"))
                    foreach (var q in url.Split('?')[1].Split('&'))
                        if (q.Contains("="))
                            list.Add(q.Split('=')[1]);

                originalData = string.Join(string.Empty, list);
            }
            else if (data != null)
                originalData = json;

            if (string.IsNullOrWhiteSpace(originalData))
                originalData = string.Empty;


            requestMessage.Content = content;
            try
            {
                using (var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false))
                {
                    if (response == null)
                    {
                        return new ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, "Không có dữ liệu");
                    }
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(responseData);
                    return new ExternalApiResponseModel<T>(result, response.StatusCode, null);
                }
            }
            catch (Exception e)
            {
                return new ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, string.IsNullOrWhiteSpace(e.Message) ? e.InnerException.Message : e.Message);
            }
        }
        public static string AddQuery(this string path, object obj)
        {
            if (path == null || obj == null)
                return path;

            return QueryHelpers.AddQueryString(path, obj.ToKeyPairs().ToDictionary(m => m.Key, m => m.Value.ToString()));
        }
        public static IEnumerable<KeyValuePair<string, object>> ToKeyPairs(this object obj)
        {
            if (obj == null)
                yield break;

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                    yield return new KeyValuePair<string, object>(property.Name, value);
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
}
