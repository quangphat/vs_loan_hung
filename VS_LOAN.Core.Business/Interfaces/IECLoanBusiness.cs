using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IECLoanBusiness
    {
        Task<string> GetToken();
        Task<string> CreateLoanToEc(LoanInfoRequestModel model, int type=0);
    }
}
