using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.MCreditModels;

namespace VS_LOAN.Core.Business
{
    public class MCreditRepository : BaseRepository, IMCeditRepository
    {
        public MCreditRepository() : base(typeof(MCreditRepository))
        {

        }
        public async Task<MCreditUserToken> GetUserTokenByIdAsync(int userId)
        {
            string sql = $"sp_MCreditUserToken_GetTokenByUserId";
            using (var con = GetConnection())
            {
                var result = await _connection.QueryFirstOrDefaultAsync<MCreditUserToken>(sql, new
                {
                    userId
                }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<bool> InsertUserToken(MCreditUserToken model)
        {
            using (var con = GetConnection())
            {
                
                await con.ExecuteAsync("sp_MCUserToken_Insert", new {
                    model.UserId,
                    model.Token
                }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
    }
}
