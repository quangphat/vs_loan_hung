using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

namespace MCreditService.Interfaces
{
    public interface ILoanContractService
    {
        Task<AuthenResponse> Authen();
        Task<AuthenResponse> Step2(int userId); 
    }
}
