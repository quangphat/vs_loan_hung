using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Collection;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IRevokeDebtRepository
    {
        Task<List<RevokeDebtSearch>> SearchAsync(int userId,
            string freeText,
            string status,
            int page,
            int limit,
            int groupId = 0,
            int assigneeId = 0,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int processStatus = -1);
    }
}

