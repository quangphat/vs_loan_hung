using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.CheckDup;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface ICheckDupBusiness
    {
        Task<BaseResponse<bool>> UpdateAsync(CheckDupEditModel model, int updatedBy);
        Task<List<GhichuViewModel>> GetNotesAsync(int checkDupId);
        Task<DataPaging<List<CheckDupIndexModel>>> GetsAsync(
            string freeText,
            int page,
            int limit, int userId);
        Task<CheckDupAddSql> GetByIdAsync(int id);
        Task<BaseResponse<int>> CreateAsync(CheckDupAddModel model, int createdBy);
    }
}
