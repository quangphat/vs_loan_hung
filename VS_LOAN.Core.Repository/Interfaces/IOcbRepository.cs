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
    public interface IOcbRepository
    {
       
        Task<List<int>> GetPeopleCanViewMyProfile(int profileId);
        Task<bool> InsertPeopleWhoCanViewProfile(int profileisUpdateMCIId, string peopleIds);
        Task<OcbProfile> GetTemProfileByMcId(int id);
        Task<List<OcbSerarchSql>> GetTempProfiles(int page, int limit, string freeText, int userId, string status = null,DateTime? fromDate = null, DateTime? toDate = null, int loaiNgay=0, int manhom = 0,

              int mathanhvien = 0);
        Task<int> CreateDraftProfile(OcbProfile model);
        Task<bool> UpdateDraftProfile(OcbProfile model);
        Task<bool> UpdateOCBProileReport(OcbStatusImportModel model);

        Task<List<OcbProfileStatus>> GetAllStatus();
        Task<List<OcbProductLoan>> GetLoanProduct(int MaDoiTac);
        Task<List<GhichuViewModel>> GetCommentsAsync(int profileId);


        Task<BaseResponse<bool>> AddNoteAsync(int profileId, string content, int userId);
      


    }
}
