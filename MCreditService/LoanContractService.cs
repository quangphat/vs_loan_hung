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
            var result = await _httpClient.PostAsync<AuthenResponse>(_requestMessage, _baseUrl, _authenApi, _contentType, null, new AuthenRequestModel
            {
                UserName = _userName,
                UserPass = _password,
                Token = _authenToken
            },  async (p) => {
                await GetUserToken(_userId);
            });
            return result.Data;
        }
        public async Task<AuthenResponse> Step2(int userId)
        {
            await GetUserToken(userId);
            return null;
        }
        public async Task<string> GetUserToken(int userId)
        {
            //if (!string.IsNullOrWhiteSpace(_userToken))
            //    return;
            _userId = userId;
            var tokenFromDb = await _bizMcredit.GetUserTokenByIdAsync(_userId);
            if (tokenFromDb != null)
            {
                _userToken = tokenFromDb.Token;
                return _userToken;
            }
            else
            {
                var tokenFromMCApi = await Authen();
                if (tokenFromMCApi == null)
                    return null;
                _userToken = tokenFromMCApi.Obj.Token;
                _bizMcredit.InsertUserToken(new MCreditUserToken { UserId = _userId,Token = _userToken });
                return _userToken;
            }
        }
    }
}
