﻿using Dapper;
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
    public class MCreditBusiness : BaseBusiness, IMCeditBusiness
    {
        public MCreditBusiness() : base(typeof(MCreditBusiness))
        {

        }
        public async Task<MCreditUserToken> GetUserTokenByIdAsync(int userId)
        {
            string sql = $"select * from MCreditUserToken where UserId = @userId";
            using (var con = GetConnection())
            {
                var result = await _connection.QueryFirstOrDefaultAsync<MCreditUserToken>(sql, new
                {
                    userId
                }, commandType: CommandType.Text);
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