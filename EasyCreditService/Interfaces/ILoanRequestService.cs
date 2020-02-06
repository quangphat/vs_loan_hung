using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace EasyCreditService.Interfaces
{
    public interface ILoanRequest
    {
        Task<string> CreateLoan(LoanInfoRequestModel model, int type = 0);
    }
}
