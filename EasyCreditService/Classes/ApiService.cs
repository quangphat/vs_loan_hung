using EasyCreditService.Infrastructure;
using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Utility;

namespace EasyCreditService.Classes
{
    public class ApiService : BaseService, IApiService
    {
        public ApiService(HttpClient httpClient) : base(httpClient, typeof(ApiService))
        {

        }
        public async Task<string> GetECToken()
        {
            _log.InfoFormat("start get token at {0}", DateTime.Now);
            try
            {
                var result = await _httpClient.GetToken(HttpMethod.Post, ECApiPath.ECGetTokenBasePath, ECApiPath.ECGetTokenPath, ECApiPath.ECClientId, ECApiPath.ECClientSecret);
                _log.InfoFormat("get token success, result is {0}", result);
                var content = await result.Content.ReadAsStringAsync();
                _log.InfoFormat("token content:{0}", content);
                return result.Content.ToString();
            }
            catch(Exception e)
            {
                _log.Info("get token error");
                _log.ErrorFormat("gettoken error: message ex {0}", e.Message);
                _log.ErrorFormat("gettoken error: ex {0}", e);
                return "error";
            }
            return "error";
        }
    }
}
