using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyCreditService.Infrastructure
{
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILog _log;
        protected BaseService(HttpClient httpClient, Type inheritService)
        {
            _httpClient = httpClient;
            _log = LogManager.GetLogger(inheritService);
        }
    }
}
