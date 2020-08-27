using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Infrastuctures;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.RevokeDebt;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IRevokeDebtBusiness
    {
        Task<BaseResponse<bool>> InsertFromFileAsync(MemoryStream stream, int userId);
        Task<DataPaging<List<RevokeDebtSearch>>> SearchAsync(int userId, string freeText, string status, int page, int limit, int groupId = 0, int assigneeId = 0, DateTime? fromDate = null, DateTime? toDate = null, int loaiNgay = 1, int ddlProcess =-1);
        Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId);
        Task<bool> DeleteByIdAsync(int userId, int profileId);
        Task<BaseResponse<bool>> AddNoteAsync(int profileId, string content, int userId);
        Task<bool> UpdateStatusAsync(int userId, int profileId, int status);
        Task<List<GhichuViewModel>> GetCommentsAsync(int profileId);
        Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int updateBy, int profileId);
    }
}
