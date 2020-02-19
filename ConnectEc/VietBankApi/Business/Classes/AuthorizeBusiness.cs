using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;
using VietBankApi.Infrastructures;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VietBankApi.Business.Classes
{
    public class AuthorizeBusiness : BaseBusiness, IAuthorizeBusiness
    {
        protected readonly ILogBusiness _log;
        public AuthorizeBusiness(IOptions<ApiSetting> appSettings, CurrentProcess currentProcess,ILogBusiness logBusiness) :base(currentProcess,appSettings)
        {
            _log = logBusiness;
        }
        public async Task<string> GetToken()
        {
            await _log.LogInfo("Get Token start");
            var httpClient = new HttpClient();
            try
            {
                var result = await httpClient.GetToken<TokenResponseModel>(_appSetting.BasePath, _appSetting.TokentPath, _appSetting.ClientId, _appSetting.ClientSecret);
                await _log.LogInfo("Get Token result: ", result.ToJson());
                if (result != null)
                    return result.access_token;
                return string.Empty;
            }
            catch(Exception e)
            {
                await _log.LogInfo("Get Token error: ", e.Message);
                return string.Empty;
            }
        }
    }
}
