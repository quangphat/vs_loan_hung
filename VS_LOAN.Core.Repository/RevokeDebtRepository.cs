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

        public async Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<RevokeDebtSearch>("sp_RevokeDebt_GetById", new { profileId, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<bool> InsertManyByParameterAsync(DynamicParameters param, int userId)
        {
            param.Add("CreatedBy", userId);
            try
            {
                var con = GetOneConnection();
                await con.ExecuteAsync("sp_insert_RevokeDebt",
                    param, commandType: CommandType.StoredProcedure);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<List<RevokeDebtSearch>> SearchAsync(int userId, string freeText, string status, int page, int limit, int groupId = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<RevokeDebtSearch>("sp_RevokeDebt_Search",
                    new { freeText, page, limit_tmp = limit, status, groupId, userId }
                    , commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<bool> DeleteByIdAsync(int userId,int profileId)
        {
            using (var con = GetConnection())
            {
               await con.ExecuteAsync("sp_RevokeDebt_Delete",
                    new { profileId, userId }
                    , commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateStatusAsync(int userId, int profileId, int status)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_RevokeDebt_UpdateStatus",
                     new { profileId, userId ,status}
                     , commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
