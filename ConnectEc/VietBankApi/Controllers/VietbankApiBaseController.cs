using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;
using VietBankApi.Infrastructures;

namespace VietBankApi.Controllers
{
    public class VietbankApiBaseController : ControllerBase
    {
        public readonly HttpClient _httpClient;
        protected readonly ApiSetting _appSettings;
        public readonly CurrentProcess _process;
        protected readonly ILogBusiness _log;
        public VietbankApiBaseController(HttpClient httpClient, IOptions<ApiSetting> appSettings, ILogBusiness logBusiness, CurrentProcess currentProcess)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            _log = logBusiness;
            _process = currentProcess;
        }
        public async Task<HttpClientResult<T>> Post<T>(string basePath = null, string path = "/", object param = null, object data = null)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                basePath = _appSettings.BasePath;
            return await _httpClient.SendRequestAsync<T>(HttpContext.Request, HttpMethod.Post, basePath, path, param, data, _process, _log);
        }
    }
}
