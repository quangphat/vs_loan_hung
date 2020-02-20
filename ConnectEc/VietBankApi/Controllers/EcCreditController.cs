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
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
namespace VietBankApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EcCreditController : VietbankApiBaseController
    {
        private const string LogKey = "ECApi";
        protected readonly IMediaBusiness _bizMedia;
        public EcCreditController(HttpClient httpClient
            , IOptions<ApiSetting> appSettings
            , ILogBusiness logBusiness
            , CurrentProcess currentProcess
            ,IMediaBusiness mediaBusiness)
            : base(httpClient, appSettings, logBusiness, currentProcess)
        {
            _bizMedia = mediaBusiness;
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
        [HttpPost("sftp")]
        public async Task<IActionResult> UploadFile([FromBody] StringModel model)
        {
            if (model == null)
                return Ok(new EcResponseModel<bool>() {
                    error = "model null",
                    data = false
                });
            var result = await _bizMedia.UploadSFtp(model.Value);
            return Ok(result);
        }
        [HttpPost("step2")]
        public async Task<IActionResult> Step2([FromBody] object model)
        {
            try
            {
                var result = await Post<object>(basePath: _appSettings.BasePath, path: _appSettings.Step2, data: model);
                if (result != null && result.Data != null)
                {
                    await _log.InfoLog("result: ", result.Data.ToJson());
                }
                
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                await _log.InfoLog("step2 error: ", e.ToJson());
                return Ok();
            }
        }
    }
}