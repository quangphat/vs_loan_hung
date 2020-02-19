using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VietBankApi.Business.Interfaces;
using VietBankApi.Infrastructures;
using VS_LOAN.Core.Entity.EasyCredit;
namespace VietBankApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EcCreditController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSetting _appSettings;
        private const string LogKey = "ECApi";
        public readonly CurrentProcess _process;
        protected readonly ILogBusiness _log;
        public EcCreditController(HttpClient httpClient, IOptions<ApiSetting> appSettings, ILogBusiness logBusiness, CurrentProcess currentProcess)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            _log = logBusiness; 
            _process = currentProcess;
        }
        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] LoanInfoRequestModel model)
        {
            var modelresult = new EcResponseModel<EcDataResponse>
            {
                code = "FIELD_ERROR_INVALID_079",
                message = "mage_selfie must be in pattern"
            };
            var result3 = JsonConvert.SerializeObject(modelresult);
            return new OkObjectResult(modelresult);
        }
        [HttpPost("step2")]
        public async Task<IActionResult> Step2([FromBody] LoanInfoRequestModel model)
        {
            //await _log.LogInfo("step2 start");
            //await _log.LogInfo("step2 dump model: ", model.ToJson());
            //await _log.LogInfo("step2 token: ", _process.Token);
            try
            {
                var result = await CoreApiClient.SendRequestAsync<object>(_httpClient, _appSettings.BasePath, HttpContext.Request, "/api/loanServices/v1/loanRequest", data: model);
                if(result!=null && result.Data!=null)
                {
                    await _log.LogInfo("result: ", result.Data.ToJson());
                }
                var result3 = JsonConvert.SerializeObject(result.Data);
                return Ok(result.Data);
                //var url = $"{_appSettings.BasePath}/api/loanServices/v1/loanRequest";
                //var url = $"http://localhost:5000/api/EcCredit/test";
                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(url);
                //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _process.Token);
                //    var data = JsonConvert.SerializeObject(model);
                //    var content = new StringContent(data, Encoding.UTF8, "application/json");
                //    var result1 = client.PostAsync("", content).Result;
                //    string resultContent = result1.Content.ReadAsStringAsync().Result;
                //    //var result5 = JsonConvert.DeserializeObject<EcResponseModel<EcDataResponse>>(resultContent);
                //    //await _log.LogInfo("result1rror: ", resultContent);
                //    //var modelresult = new EcResponseModel<EcDataResponse> {
                //    //    code = "FIELD_ERROR_INVALID_079",
                //    //    message = "mage_selfie must be in pattern"
                //    //};
                //    //var result3 = JsonConvert.SerializeObject(modelresult);
                //    return new  OkObjectResult(result1);
                //}
                //var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

                //_httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat("Bearer ", _process.Token));
                //var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore
                //});
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                //requestMessage.Content = content;
                ////_httpClient.DefaultRequestHeaders
                ////.Accept
                ////.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //using (var response = await _httpClient.SendAsync(requestMessage))
                //{
                //    await _log.LogInfo("step2 statuscode:", response.StatusCode.ToJson());
                //    if (response!=null && response.Content != null)
                //    {
                //        var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                //            ? response.Headers.Location.AbsoluteUri
                //            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                //        await _log.LogInfo("step2 AbsoluteUri:", responseData);
                //        await _log.LogInfo("step2 responseData:", responseData);
                //        return Ok();
                //    }
                //    return Ok();
                //}
                //var result = await _httpClient.Post( HttpContext.Request, _log, _appSettings.BasePath, "/api/loanServices/v1/loanRequest", data: model, process: _process, token:_process.Token);
                //var result2 = await _httpClient.Step2(_log, _appSettings.BasePath, "/api/loanServices/v1/loanRequest", token: _process.Token);
                return Ok();   
            }
            catch(Exception e)
            {
                await _log.LogInfo("step2 error: ", e.ToJson());
                return Ok();
            }    
        }
    }
}