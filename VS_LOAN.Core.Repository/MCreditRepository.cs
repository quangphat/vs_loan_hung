using Dapper;
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
        public async Task<List<int>> GetPeopleCanViewMyProfile(int profileId)
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<int>("sp_MCProfilePeople_GetPeopleCanViewProfile", new { profileId}, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<bool> UpdateSale(UpdateSaleModel model, int profileId)
        {

                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_MCredit_TempProfile_update_Sale", new
                    {
                        profileId,
                        model.SaleId,
                        model.SaleNumber,
                        model.SaleName
                    }, commandType: CommandType.StoredProcedure);
                }
            
            // await Task.WhenAll(tasks);
            return true;
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
        public async Task<MCredit_TempProfile> GetTemProfileByMcId(string id)
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryFirstOrDefaultAsync<MCredit_TempProfile>("sp_MCredit_TempProfile_GetByMCId", new { id }, commandType: CommandType.StoredProcedure);
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
        public async Task<bool> InsertProfileStatus(List<OptionSimple> status)
        {
            foreach (var p in status)
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_MCreditProfileStatus_Insert", new
                    {
                        p.Code,
                        p.Name

                    }, commandType: CommandType.StoredProcedure);
                }
            }
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
        public async Task<bool> InsertPeopleWhoCanViewProfile(int profileId, string peopleIds)
        {
            using (var con = GetConnection())
            {

                await con.ExecuteAsync("sp_MCProfilePeople_Insert", new
                {
                    profileId,
                    peopleIds
                }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<List<OptionSimple>> GetMCProfileStatusList()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_MCStatus_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
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
        public async Task<int> CreateDraftProfile(MCredit_TempProfile model)
        {
            model.Id = 0;
            var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.isAddr),
                nameof(model.isInsur),
                nameof(model.MCId)
            });

            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_insert_MCredit_TempProfile", param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("Id");
            }
        }
        public async Task<bool> DeleteById(int profileId)
        {
           
            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_MCProfile_DeleteProfile", new { profileId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateDraftProfile(MCredit_TempProfile model)
        {
            var param = GetParams(model, ignoreKey: new string[] 
            {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.CreatedBy),
                nameof(model.isAddr),
                nameof(model.isInsur)
            });

            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_update_MCredit_TempProfile", param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<ProfileSearchSql>> GetTempProfiles(int page, int limit, string freeText, int userId, string status = null)
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<ProfileSearchSql>("sp_MCredit_TempProfile_Gets", new {
                    freeText,
                    userId,
                    page,
                    limit_tmp = limit,
                    status
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> CountTempProfiles(string freeText, int userId, string status =null)
        {
            using (var con = GetConnection())
            {
                var result = await _connection.ExecuteScalarAsync<int>("sp_MCredit_TempProfile_Counts", new
                {
                    freeText,
                    userId,
                    status
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
