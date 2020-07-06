using HttpService;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

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
            var requestMessage = new HttpRequestMessage();
            requestMessage.Headers.Add("Content-Type", _contentType);
            requestMessage.Headers.Add("xdncode", _xdnCode);
            var result = await _httpClient.PostAsync<AuthenResponse>(requestMessage, _baseUrl, _authenApi, null, new AuthenRequestModel
            {
                UserName = _userName,
                UserPass = _password,
                Token = _authenToken
            });
            return result.Data;
        }
    }
}
