using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using VS_LOAN.Core.Entity.RevokeDebt;
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
        public async Task<List<RevokeDebtSearch>> Search(int userId,string freeText, string status, int page, int limit, int groupId = 0)
        {
           
                var result = await _connection.QueryAsync<RevokeDebtSearch>("sp_RevokeDebt_Search",
                     new { freeText, page, limit_tmp = limit, status, groupId, userId}
                     , commandType: CommandType.StoredProcedure);
                return result.ToList();

           
        }
    }
}
