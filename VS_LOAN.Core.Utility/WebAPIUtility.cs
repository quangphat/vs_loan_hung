using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    public class WebAPIUtility
    {
        public static async Task<string> Post(string url, string data)
        {
            string rs = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
                    if (response.IsSuccessStatusCode)
                    {
                        rs = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }

        public static async Task<string> Get(string url)
        {
            string rs = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        rs = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }

        public static async Task<string> Post<T>(string url, T dataObject)
        {
            string rs = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync<T>(url, dataObject);
                    if (response.IsSuccessStatusCode)
                    {
                        rs = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }
        //public static  Task<string> PostNoWait<T>(string url, T dataObject)
        //{
        //    string rs = "";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            HttpResponseMessage response =  client.PostAsJsonAsync<T>(url, dataObject);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                rs =  response.Content.ReadAsStringAsync();
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return rs;
        //}
    }
}
