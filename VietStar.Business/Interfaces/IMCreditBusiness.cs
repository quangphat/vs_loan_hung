using VietStar.Entities.Mcredit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IMCreditBusiness
    {
        Task<string> CheckSaleAsync(CheckSaleModel model);
        Task<CheckCatResponseModel> CheckCatAsync(StringModel model);
        Task<bool> IsCheckCatAsync(StringModel model);
        Task<CheckCICResponseModel> CheckCICAsync(CheckCICModel model);
        Task<CheckDupResponseModel> CheckDupAsync(StringModel model);
        Task<CheckStatusResponseModel> CheckStatusAsync(StringModel model);
        Task<DataPaging<List<ProfileSearchSql>>> SearchsTemsAsync(string freeText, string status, int page = 1, int limit = 20);
        Task<ProfileSearchResponse> SearchsAsync(string freeText, string status, string type, int page = 1);
        Task<int> CreateDraftAsync(MCredit_TempProfileAddModel model);
        Task<bool> AddRefuseReasonToNoteAsync(int profileId, string type);
        Task<bool> UpdateTempProfileStatusAsync(int profileId);
        Task<bool> UpdateDraftAsync(MCredit_TempProfileAddModel model);
        Task<MCredit_TempProfile> GetTempProfileByIdAsync(int id);
        Task<List<OptionSimple>> GetSimpleListByTypeAsync(string type);
    }
}
