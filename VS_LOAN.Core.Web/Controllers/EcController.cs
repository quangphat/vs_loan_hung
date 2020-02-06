using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VS_LOAN.Core.Web.Controllers
{
    [RoutePrefix("api/easycredit")]
    public class EcController : BaseApiController
    {
        protected readonly IECLoanBusiness _bizEcLoan;
        protected readonly HttpClient _httpClient;
        public EcController(HttpClient httpClient, IECLoanBusiness ecLoanBusiness)
        {
            _bizEcLoan = ecLoanBusiness;
            _httpClient = httpClient;
        }
        [HttpPost]
        [Route("create/{type}")]
        public async Task<IHttpActionResult> CreateLoan(LoanInfoRequestModel model,int  type = 0)
        {
            var result = await _bizEcLoan.CreateLoanToEc(model, type);
            return Ok(result);
        }
        [HttpGet]
        [Route("token")]
        public async Task<IHttpActionResult> GetTokent()
        {
            var result = await _bizEcLoan.GetToken();
            return Ok(result);
        }
    }
}
