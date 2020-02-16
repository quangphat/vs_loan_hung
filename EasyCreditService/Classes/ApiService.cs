using EasyCreditService.Infrastructure;
using EasyCreditService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;
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
                var result = await _httpClient.Get<TokenResponseModel>(ECApiPath.BasePath, ECApiPath.TokenPath);
                _log.InfoFormat("get token success, result is {0}", result);
                
                _log.InfoFormat("token content:{0}", result);
                return result.access_token;
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
