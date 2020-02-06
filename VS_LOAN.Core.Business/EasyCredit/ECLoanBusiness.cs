﻿using EasyCreditService.Interfaces;
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
        public async Task<string> CreateLoanToEc(LoanInfoRequestModel model, int type =0)
        {
            if (model == null)
                return "model is null";
            return await _svLoanrequest.CreateLoan(model, type);
        }
        public async Task<string> GetToken()
        {
            _log.Info("token: business");
            return await _svApi.GetECToken();
        }
    }
}
