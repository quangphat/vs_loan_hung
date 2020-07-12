﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Repository
{
    public class MCreditRepository : BaseRepository, IMCeditRepository
    {
        public MCreditRepository() : base(typeof(MCreditRepository))
        {

        }
        public async Task<bool> DeleteMCTableDatas(int type)
        {
            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_deleteMCTable", new { type }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<MCredit_TempProfile> GetTemProfileById(int id)
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryFirstOrDefaultAsync<MCredit_TempProfile>("sp_MCredit_TempProfile_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }
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

        public async Task<bool> InsertLocations(List<MCreditlocations> locations)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in locations)
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_insert_MCreditlocations", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Addr,
                        p.Name

                    }, commandType: CommandType.StoredProcedure);
                }
            }
            //await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertCities(List<MCreditCity> cities)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in cities)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditCity", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            // await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertLoanPeriods(List<MCreditLoanPeriod> loanPeriods)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in loanPeriods)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditLoanPeriod", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            //await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertProducts(List<MCreditProduct> products)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in products)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditProduct", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name,
                        p.IsCheckCat,
                        p.MaxLoanAmount,
                        p.MinLoanAmount,
                        p.MinTenor,
                        p.MaxTenor
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            // await Task.WhenAll(tasks);
            return true;
        }

        public async Task<bool> InsertUserToken(MCreditUserToken model)
        {
            using (var con = GetConnection())
            {

                await con.ExecuteAsync("sp_MCUserToken_Insert", new
                {
                    model.UserId,
                    model.Token
                }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<List<OptionSimple>> GetMCLocationSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_MCLocation_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCProductSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_MCProduct_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCLoanPerodSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_MCLoanPeriod_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCCitiesSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_MCCities_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> CreateProfile(MCredit_TempProfile model)
        {
            model.CreatedTime = model.UpdatedTime = DateTime.Now;
            var p = GetParams(model, "Id", ignoreKey: new string[] { nameof(model.CreatedTime), nameof(model.UpdatedTime) });

            using (var con = GetConnection())
            {
                await _connection.QueryAsync<OptionSimple>("sp_insert_MCredit_TempProfile", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("Id");
            }
        }
    }
}