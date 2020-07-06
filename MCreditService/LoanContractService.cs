using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using HttpClientService;
using System.Net.Http.Headers;

namespace MCreditService
{
    public class LoanContractService
    {
        public static string _baseUrl = "http://api.taichinhtoancau.vn";
        public static string _authenApi = "api/act/authen.html";
        public static string _userName = "vietbankapi";
        public static string _password = "api@123";
        public static string _authenToken = "$2y$10$ne/8QwsCG10c.5cVSUW6NO7L3..lUEFItM4ccV0usJ3cAbqEjLywG";
        public static string _xdnCode = "TWpBeU1HUjFibWR1Wlc4eU1ESXc=";
        public static string _contentType = "application/json";
        public static HttpClient _httpClient = new HttpClient();
        public static async Task<AuthenResponse> Authen()
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post,$"{_baseUrl}/{_authenApi}");
                //requestMessage.
                requestMessage.Headers.Add("xdncode", "TWpBeU1HUjFibWR1Wlc4eU1ESXc=");
                //requestMessage.Content.Headers.ContentType  = new MediaTypeHeaderValue(_contentType);
                // requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await _httpClient.PostAsync<AuthenResponse>(requestMessage, _baseUrl, _authenApi, null, new AuthenRequestModel
                {
                    UserName = _userName,
                    UserPass = _password,
                    Token = _authenToken
                });
                return result.Data;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
