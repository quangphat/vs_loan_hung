using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IRevokeDebtBusiness
    {
        Task<BaseResponse<bool>> InsertFromFile(MemoryStream stream, int userId);
    }
}
