using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Utility
{
    public static class CoreApiClient
    {

        public static async Task<HttpResponseMessage> Post(this HttpClient httpClient, string basePath, string path = "/", object param = null, object data = null, int type = 0)
        {
            return await httpClient.Call(HttpMethod.Post, basePath, path, param, data, type);
        }
        private static async Task<HttpResponseMessage> Call(this HttpClient httpClient,
            HttpMethod method, string basePath, string path = "/", object param = null, object data = null, int type = 0)
        {
            //if (param != null)
            //    path = path.AddQuery(param);
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
            if (type == 0)
            {
                requestMessage.Headers.Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Authorization", "Bearer 3c8f7cf4-8228-4c2f-8b09-75d42bad3c8e");
            }
            if (type == 1)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer write:loan_request");
            }
            requestMessage.Content = content;

            var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        //public static string AddQuery(this string path, object obj)
        //{
        //    if (path == null || obj == null)
        //        return path;

        //    return QueryHelpers.AddQueryString(path, obj.ToKeyPairs().ToDictionary(m => m.Key, m => m.Value.ToString()));
        //}

    }
}
