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
        Task<bool> InsertManyByParameter(DynamicParameters param, int userId);
        Task<List<RevokeDebtSearch>> Search(int userId, string freeText, string status, int page, int limit, int groupId = 0);
    }
}
