using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;

namespace HttpClientService
{
    public static class CoreApiClient
    {
        public static async Task<ExternalApiResponseModel<T>> GetAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", string headerContentType = null, object param = null)
        {
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, headerContentType, param);
            return response;
        }
        public static async Task<ExternalApiResponseModel<T>> PostAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", string headerContentType = null, object param = null, object data = null)
        {
            requestMessage.Method = HttpMethod.Post;
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, headerContentType, param, data);
            return response;
        }
        private static async Task<ExternalApiResponseModel<T>> Call<T>(this HttpClient httpClient,
             HttpRequestMessage requestMessage, string baseUrl, string path = "/", string headerContentType = null, object param = null, object data = null)
        {
            //if (param != null)
            //    path = path.AddQuery(param);
            if (path[0] != '/')
                path = $"/{path}";
            var url = $"{baseUrl}{path}";
            requestMessage.RequestUri = new Uri(url);
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
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    //json = "{\"str\":\"ddddddddddd\",\"status\":\"0\",\"page\":1,\"type\":\"draft\",\"token\":\"7ac1e4d882b98a0086080706e354cf4c\"} ";
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

            if (headerContentType != "multipart/form-data")
                requestMessage.Content = content;
            if (!string.IsNullOrWhiteSpace(headerContentType))
            {
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(headerContentType);
            }
            var newRequest = await requestMessage.CloneAsync();
            try
            {
                using (var response = await httpClient.SendAsync(newRequest).ConfigureAwait(false))
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
        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync().ConfigureAwait(false),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        public static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();
            await content.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }
        //public static string AddQuery(this string path, object obj)
        //{
        //    if (path == null || obj == null)
        //        return path;

        //    return QueryHelpers.AddQueryString(path, obj.ToKeyPairs().ToDictionary(m => m.Key, m => m.Value.ToString()));
        //}
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
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static async Task<ExternalApiResponseModel<T>> MultipartFormPost<T>(string postUrl, string userAgent, Dictionary<string, object> postParameters, string headerkey, string headervalue, string token)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            
            using (var webResponse = await PostForm<T>(postUrl, userAgent, contentType, formData, headerkey, headervalue, token))
            {
                if(webResponse == null)
                {
                    return new ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, "No data");
                }
                if(webResponse.StatusCode == HttpStatusCode.Moved || webResponse.StatusCode == HttpStatusCode.Found)
                {
                    return new ExternalApiResponseModel<T>(default(T),webResponse.StatusCode, webResponse.ResponseUri.AbsoluteUri);
                }

                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var response = await responseReader.ReadToEndAsync();
                    var result = JsonConvert.DeserializeObject<T>(response);
                    return new ExternalApiResponseModel<T>(result, webResponse.StatusCode, null);
                }
                   
            }
        }
        private static async Task<HttpWebResponse> PostForm<T>(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue, string token)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                return null;
            }

            // Set up the request properties.  
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            request.Headers.Add(headerkey, headervalue);
            using (Stream requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            var response = (await request.GetResponseAsync()) as HttpWebResponse;
            return response;
        }
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {

                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter) // to check if parameter if of file type   
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream  
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.  
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline  
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]  
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

    }
    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }

}

