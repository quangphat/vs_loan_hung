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
    public interface IMiraeRepository
    {
       
        Task<List<int>> GetPeopleCanViewMyProfile(int profileId);
        Task<bool> InsertPeopleWhoCanViewProfile(int profileisUpdateMCIId, string peopleIds);
        Task<MiraeModel> GetTemProfileByMcId(int id);
        Task<List<MiraeModelSearchModel>> GetTempProfiles(int page, int limit, string freeText, int userId, string status = null,DateTime? fromDate = null, DateTime? toDate = null, int loaiNgay=0, int manhom = 0,

              int mathanhvien = 0);
        Task<int> CreateDraftProfile(MiraeModel model);
        Task<bool> UpdateDraftProfile(MiraeModel model);
        Task<bool> UpdateStatusComplete(int customerId, int profileid);
        Task<List<OcbProfileStatus>> GetAllStatus();
        Task<List<OcbProductLoan>> GetLoanProduct(int MaDoiTac);
        Task<List<GhichuViewModel>> GetCommentsAsync(int profileId);
        
        Task<BaseResponse<bool>> AddNoteAsync(int profileId, string content, int userId);



        Task<bool> UpdateStatus(int id, int status, int appid, int userid);
        Task<bool> UpdateDDE(MiraeDDEEditModel model);


    }
}
