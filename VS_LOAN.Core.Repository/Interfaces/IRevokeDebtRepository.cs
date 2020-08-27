using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.RevokeDebt;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IRevokeDebtRepository
    {
        Task<bool> InsertManyByParameterAsync(DynamicParameters param, int userId);
        Task<List<RevokeDebtSearch>> SearchAsync(int userId, string freeText, string status, int page, int limit, int groupId = 0, int assigneeId = 0, DateTime? fromDate = null, DateTime? toDate = null, int loaingay = 1, int ddlProcess =-1);
        Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId);
        Task<bool> DeleteByIdAsync(int userId, int profileId);
        Task<bool> UpdateStatusAsync(int userId, int profileId, int status);
        Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int updateBy, int profileId);
    }
}
