using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VS_LOAN.Core.Web.Controllers
{
    [RoutePrefix("api/easycredit")]
    public class EcController : BaseApiController
    {
        protected readonly IECLoanBusiness _bizEcLoan;
        protected readonly HttpClient _httpClient;
        //protected readonly ICurrentProcess
        public EcController(HttpClient httpClient, IECLoanBusiness ecLoanBusiness)
        {
            _bizEcLoan = ecLoanBusiness;
            _httpClient = httpClient;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> CreateLoan([FromBody]StringModel model)
        {
            var result = await _bizEcLoan.UploadFile(model);
            return Ok(result);
        }
        [HttpPost]
        [Route("step2")]
        public async Task<IHttpActionResult> CreateLoan(LoanInfoRequestModel model)
        {
            var result = await _bizEcLoan.CreateLoanToEc(model);
            
            return Ok(result);
        }
        
    }
}
