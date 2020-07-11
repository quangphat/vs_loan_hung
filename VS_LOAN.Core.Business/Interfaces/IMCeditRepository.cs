using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IMCeditRepository
    {
        Task<bool> DeleteMCTableDatas(int type);
        Task<MCreditUserToken> GetUserTokenByIdAsync(int userId);
        Task<bool> InsertUserToken(MCreditUserToken model);
        Task<bool> InsertLocations(List<MCreditlocations> locations);
        Task<bool> InsertProducts(List<MCreditProduct> products);
        Task<bool> InsertLoanPeriods(List<MCreditLoanPeriod> loanPeriods);
        Task<bool> InsertCities(List<MCreditCity> cities);
    }
}
