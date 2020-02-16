using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VietBankApi.Infrastructures;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Utility;
namespace VietBankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcCreditController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSetting _appSettings;
        private readonly ILogger _logger;
        private const string LogKey = "ECApi";
        public EcCreditController(HttpClient httpClient, IOptions<ApiSetting> appSettings, ILogger<EcCreditController> logger)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            _logger = logger;
        }
        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            _logger.LogInformation($"{LogKey}: {0}", "reach");
            try
            {
                var result = await _httpClient.GetToken<object>(HttpMethod.Post, _appSettings.BasePath, _appSettings.TokentPath, _appSettings.ClientId, _appSettings.ClientSecret);
                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogError($"{LogKey} error: {0}", e);
                return Ok("Call Ok");
            }
            
        }
        [HttpPost("step2")]
        public async Task<IActionResult> Step2([FromBody] object model)
        {
            return await _httpClient.Post<IActionResult>( _appSettings.BasePath, "/loanServices/v1/loanRequest", data:model);
        }
    }
}