using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class ECLoanBusiness : BaseBusiness, IECLoanBusiness
    {
        protected ILoanRequestService _svLoanrequest;
        protected readonly IApiService _svApi;
        public ECLoanBusiness(ILoanRequestService loanRequest, IApiService apiService) : base(typeof(ECLoanBusiness))
        {
            _svLoanrequest = loanRequest;
            _svApi = apiService;
        }
        public async Task<EcResponseModel<EcDataResponse>> CreateLoanToEc(LoanInfoRequestModel model)
        {
            if (model == null)
                return null;
            //await _svLoanrequest.TestVietbankApi();
            //var token = await _svApi.GetECToken();
            var result = await _svLoanrequest.CreateLoan(model);
            if(result!=null)
                return result;
            return null;
        }
        public async Task<string> GetToken()
        {
            _log.Info("token: business");
            return await _svApi.GetECToken();
        }
    }
}
