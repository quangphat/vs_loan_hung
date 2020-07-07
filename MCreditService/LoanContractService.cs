using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using HttpClientService;
using System.Net.Http.Headers;
using VS_LOAN.Core.Business;
using MCreditService.Interfaces;

namespace MCreditService
{
    public class LoanContractService: ILoanContractService
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
        protected readonly MCreditBusiness _bizMcredit;
        protected int _userId;
        protected string _userToken;
        public LoanContractService()
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            _userToken = string.Empty;
            _bizMcredit = new MCreditBusiness();
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
        public async Task<AuthenResponse> Step2(int userId)
        {
            await GetUserToken(userId);
            return null;
        }
        private async Task GetUserToken(int userId)
        {
            if (!string.IsNullOrWhiteSpace(_userToken))
                return;
            _userId = userId;
            var tokenFromDb = await _bizMcredit.GetUserTokenByIdAsync(_userId);
            if (tokenFromDb != null)
            {
                _userToken = tokenFromDb.Token;
                return;
            }
            else
            {
                var tokenFromMCApi = await Authen();
                if (tokenFromMCApi == null)
                    return;
                _userToken = tokenFromMCApi.Obj.Token;
                return;
            }
        }
    }
}
