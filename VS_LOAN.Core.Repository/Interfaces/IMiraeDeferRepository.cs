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
    public interface IMiraeDeferRepository
    {
        Task<int> Add(MiraeDeferModel model);
        Task<bool> Resove(int id, int status, int appid, int userid);
        Task<MiraeDeferModel> GetTemProfileByMcId(int id);


        Task<List<MiraeDeferSearchModel>> GetTempProfiles(int page, int limit, string freeText, int userId, string status = null, DateTime? fromDate = null, DateTime? toDate = null, int loaiNgay = 0, int manhom = 0,

      int mathanhvien = 0);
    }
}
