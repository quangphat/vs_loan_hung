using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IMCeditRepository
    {
        Task<int> CreateProfile(MCredit_TempProfile model);
        Task<bool> DeleteMCTableDatas(int type);
        Task<MCreditUserToken> GetUserTokenByIdAsync(int userId);
        Task<bool> InsertUserToken(MCreditUserToken model);
        Task<bool> InsertLocations(List<MCreditlocations> locations);
        Task<bool> InsertProducts(List<MCreditProduct> products);
        Task<bool> InsertLoanPeriods(List<MCreditLoanPeriod> loanPeriods);
        Task<bool> InsertCities(List<MCreditCity> cities);
        Task<List<OptionSimple>> GetMCProductSimpleList();
        Task<List<OptionSimple>> GetMCLocationSimpleList();
        Task<List<OptionSimple>> GetMCLoanPerodSimpleList();
        Task<List<OptionSimple>> GetMCCitiesSimpleList();
        Task<MCredit_TempProfile> GetTemProfileById(int id);
    }
}
