using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class ECLoanBusiness : BaseBusiness, IECLoanBusiness
    {
        protected ILoanRequestService _svLoanrequest;
        protected readonly IApiService _svApi;
        public readonly CurrentProcess _process;
        public ECLoanBusiness(ILoanRequestService loanRequest, IApiService apiService, HttpClient httpClient, CurrentProcess currentProcess) : base(typeof(ECLoanBusiness), httpClient)
        {
            _svLoanrequest = loanRequest;
            _svApi = apiService;
            _process = currentProcess;
        }
        public async Task<EcResponseModel<bool>> UploadFile(StringModel model)
        {
            var x = _process.UserName;
            return await _svLoanrequest.UploadFile(model);
        }
        public async Task<EcResponseModel<EcDataResponse>> CreateLoanToEc(LoanInfoRequestModel model)
        {
            if (model == null)
                return null;
            var result = await _httpClient.Post<EcResponseModel<EcDataResponse>>(EcApiPath.BasePathDev, EcApiPath.Step2, model);
            if (result != null)
                return result.Data;
            return null;
        }

    }
}
