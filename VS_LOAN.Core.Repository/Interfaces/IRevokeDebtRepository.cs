using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IRevokeDebtRepository
    {
        Task<bool> InsertManyByParameter(DynamicParameters param, int userId);
    }
}
