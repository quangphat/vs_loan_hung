using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.CheckDup;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ICheckDupRepository
    {
        Task<RepoResponse<bool>> UpdateAsync(CheckDupAddSql model, int updateBy);
        Task<List<CheckDupIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit,
            int userId);
        Task<CheckDupAddSql> GetByIdAsync(int id);
        Task<RepoResponse<int>> CreateAsync(CheckDupAddSql model, int createdBy);
        Task<List<GhichuViewModel>> GetNoteByIdAsync(int customerId);
    }
}
