using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IMCreditRepository
    {
        Task<BaseResponse<bool>> UpdateTempProfileStatusAsync(int profileId, int status);
        Task<bool> IsCheckCat(string productCode);
        
        Task<bool> InsertPeopleWhoCanViewProfile(int profileId, string peopleIds);
        Task<BaseResponse<MCredit_TempProfile>> GetTemProfileByMcId(string id);
        Task<List<OptionSimple>> GetMCProfileStatusList();
        Task<bool> UpdateSale(UpdateSaleModel model, int profileId);
        Task<int> CountTempProfiles(string freeText, int userId, string status = null);
        Task<List<ProfileSearchSql>> GetTempProfiles(int page, int limit, string freeText, int userId, string status = null);
        Task<BaseResponse<int>> CreateDraftProfile(MCredit_TempProfile model);
        Task<BaseResponse<bool>> UpdateDraftProfile(MCredit_TempProfile model);
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
        Task<BaseResponse<MCredit_TempProfile>> GetTemProfileById(int id);
        Task<bool> DeleteById(int profileId);
    }
}

