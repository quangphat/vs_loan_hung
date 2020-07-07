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
        protected static string _baseUrl = "http://api.taichinhtoancau.vn";
        protected static string _authenApi = "api/act/authen.html";
        protected static string _userName = "vietbankapi";
        protected static string _password = "api@123";
        protected static string _authenToken = "$2y$10$ne/8QwsCG10c.5cVSUW6NO7L3..lUEFItM4ccV0usJ3cAbqEjLywG";
        protected static string _xdnCode = "TWpBeU1HUjFibWR1Wlc4eU1ESXc=";
        protected static string _contentType = "application/json";
        protected readonly HttpClient _httpClient;
        protected readonly HttpRequestMessage _requestMessage;
        public LoanContractService()
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
        }
        public async Task<AuthenResponse> Authen()
        {
            var result = await _httpClient.PostAsync<AuthenResponse>(_requestMessage, _baseUrl, _authenApi, _contentType, null, new AuthenRequestModel
            {
                UserName = _userName,
                UserPass = _password,
                Token = _authenToken
            });
            return result.Data;
        }
    }
}
