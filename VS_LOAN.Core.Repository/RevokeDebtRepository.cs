using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class RevokeDebtRepository : BaseRepository, IRevokeDebtRepository
    {
        public RevokeDebtRepository() : base(typeof(RevokeDebtRepository))
        {
        }

        public async Task<bool> InsertManyByParameter(DynamicParameters param, int userId)
        {
            param.Add("CreatedBy", userId);
            try
            {
               
                    await _connection.ExecuteAsync("sp_insert_RevokeDebt",
                        param, commandType: CommandType.StoredProcedure);
                    return true;
                
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
