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
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.HosoCourrier;

namespace MCreditService
{
    public class LoanContractService : ILoanContractService
    {
        protected static string _baseUrl = "http://api.taichinhtoancau.vn";
        protected static string _authenApi = "api/act/authen.html";
        protected static string _checkCATApi = "api/act/checkcat.html";
        protected static string _userName = "vietbankapi";
        protected static string _password = "api@123";
        protected static string _authenToken = "$2y$10$ne/8QwsCG10c.5cVSUW6NO7L3..lUEFItM4ccV0usJ3cAbqEjLywG";
        protected static string _xdnCode = "TWpBeU1HUjFibWR1Wlc4eU1ESXc=";
        protected static string _contentType = "application/json";
        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IMCeditBusiness _bizMcredit;
        protected int _userId;
        protected string _userToken;
        public LoanContractService(IMCeditBusiness mCeditBusiness)
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            _userToken = string.Empty;
            _bizMcredit = mCeditBusiness;
        }
        public async Task<AuthenResponse> Authen()
        {
            var result = await _httpClient.PostAsync<AuthenResponse, AuthenResponse>(_requestMessage, _baseUrl, _authenApi, _contentType, null, new AuthenRequestModel
            {
                UserName = _userName,
                UserPass = _password,
                token = _authenToken
            });
            return result.Data;
        }
        public async Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber)
        {
            var model = new CheckCatRequestModel { taxNumber = taxNumber, UserId = userId };
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            var result = await _httpClient.PostAsync<CheckCatResponseModel, CheckCatRequestModel>(_requestMessage, _baseUrl, _checkCATApi, _contentType, null, model, async (data) => await GetUserToken<CheckCatRequestModel>(model));
            return result.Data;
        }
        private async Task<T> GetUserToken<T>( T data) where T : MCreditRequestModelBase
        {
            _userId = data.UserId;
            if (!string.IsNullOrWhiteSpace(_userToken))
            {
                data.token = _userToken;
                return data;
            }
            var tokenFromDb = await _bizMcredit.GetUserTokenByIdAsync(_userId);
            //tokenFromDb = null;
            if (tokenFromDb != null)
            {
                _userToken = tokenFromDb.Token;

            }
            else
            {
                var tokenFromMCApi = await Authen();
                if (tokenFromMCApi == null)
                    return null;
                _userToken = tokenFromMCApi.Obj.Token;
                _bizMcredit.InsertUserToken(new MCreditUserToken { UserId = _userId, Token = _userToken });
            }
            data.token = _userToken;
            return data;
        }
    }
}
