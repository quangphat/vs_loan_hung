using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IMCeditBusiness
    {
        Task<MCreditUserToken> GetUserTokenByIdAsync(int userId);
        Task<bool> InsertUserToken(MCreditUserToken model);
    }
}
