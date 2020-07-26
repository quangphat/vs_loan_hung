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
        Task<BaseResponse<bool>> InsertFromFile(MemoryStream stream, int userId);
        Task<DataPaging<List<RevokeDebtSearch>>> Search(int userId, string freeText, string status, int page, int limit, int groupId = 0);
    }
}
