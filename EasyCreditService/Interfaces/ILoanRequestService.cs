using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace EasyCreditService.Interfaces
{
    public interface ILoanRequestService
    {
        Task<EcResponseModel<EcDataResponse>> CreateLoan(LoanInfoRequestModel model, string token);
        Task<List<string>> TestVietbankApi();
    }
}
