using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IMCeditRepository
    {
        Task<List<int>> GetPeopleCanViewMyProfile(int profileId);
        Task<bool> InsertPeopleWhoCanViewProfile(int profileId, string peopleIds);
        Task<MCredit_TempProfile> GetTemProfileByMcId(string id);
        Task<List<OptionSimple>> GetMCProfileStatusList();
        Task<bool> UpdateSale(UpdateSaleModel model, int profileId);
        Task<int> CountTempProfiles(string freeText, int userId);
        Task<List<ProfileSearchSql>> GetTempProfiles(int page, int limit, string freeText, int userId);
        Task<int> CreateDraftProfile(MCredit_TempProfile model);
        Task<bool> UpdateDraftProfile(MCredit_TempProfile model);
        Task<bool> DeleteMCTableDatas(int type);
        Task<MCreditUserToken> GetUserTokenByIdAsync(int userId);
        Task<bool> InsertUserToken(MCreditUserToken model);
        Task<bool> InsertLocations(List<MCreditlocations> locations);
        Task<bool> InsertProducts(List<MCreditProduct> products);
        Task<bool> InsertLoanPeriods(List<MCreditLoanPeriod> loanPeriods);
        Task<bool> InsertProfileStatus(List<OptionSimple> status);
        Task<bool> InsertCities(List<MCreditCity> cities);
        Task<List<OptionSimple>> GetMCProductSimpleList();
        Task<List<OptionSimple>> GetMCLocationSimpleList();
        Task<List<OptionSimple>> GetMCLoanPerodSimpleList();
        Task<List<OptionSimple>> GetMCCitiesSimpleList();
        Task<MCredit_TempProfile> GetTemProfileById(int id);
    }
}
