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
        protected ILoanRequest _svLoanrequest;
        public ECLoanBusiness(ILoanRequest loanRequest) : base(typeof(ECLoanBusiness))
        {
            _svLoanrequest = loanRequest;
        }
        public async Task CreateLoanToEc(LoanInfoRequestModel model)
        {
            if (model == null)
                return;
            await _svLoanrequest.CreateLoan(model);
        }
    }
}
