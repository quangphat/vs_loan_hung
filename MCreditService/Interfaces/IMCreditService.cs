using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

namespace MCreditService.Interfaces
{
    public interface IMCreditService
    {
        Task<AuthenResponse> Authen();
        Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber);
        Task<CheckDupResponseModel> CheckDup(string value);
        Task<CheckCICResponseModel> CheckCIC(string value);
        Task<CheckStatusResponseModel> CheckStatus(string value);
    }
}
