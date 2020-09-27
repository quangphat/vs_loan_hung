using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.CheckDup;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class CheckDupRepository :BaseRepository, ICheckDupRepository
    {
        protected readonly ILogRepository _rpLog;
        public CheckDupRepository(ILogRepository logRepository) : base(typeof(CheckDupRepository))
        {
            _rpLog = logRepository;
        }


        public async Task<RepoResponse<int>> CreateAsync(CheckDupAddSql model, int createdBy)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var pars = GetParams(model, nameof(model.Id),ignoreKey: new string[] {
                        nameof(model.CreatedTime),
                          nameof(model.StatusValue),
                        nameof(model.UpdatedTime),
                        nameof(model.UpdatedBy),
                        nameof(model.CreatedBy)
                    });
                    pars.Add("CreatedBy", createdBy);
                   
                    await con.ExecuteAsync("sp_InsertCustomer_v2", pars, commandType: CommandType.StoredProcedure);
                    return RepoResponse<int>.Create(pars.Get<int>(nameof(model.Id)));

                }
            }
            catch (Exception e)
            {
                return RepoResponse<int>.Create(0, GetException(e));
            }
        }

        public async Task<CheckDupAddSql> GetByIdAsync(int id)
        {
            string sql = $"select * from Customer where Id = @id";
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<CheckDupAddSql>(sql, new
                {
                    id
                }, commandType: CommandType.Text);
                return result;
            }
        }

        public async Task<List<GhichuViewModel>> GetNoteByIdAsync(int customerId)
        {
            var p = new DynamicParameters();
            p.Add("profileId", customerId);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<GhichuViewModel>("sp_getCheckDupNotesById", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

        public async Task<List<CheckDupIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit,
            int userId)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= 100) ? 100 : limit;
            int offset = (page - 1) * limit;
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("offset", offset);
            p.Add("limit", limit);
            p.Add("userId", userId);

            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CheckDupIndexModel>("sp_GetCustomer_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

        public async Task<RepoResponse<bool>> UpdateAsync(CheckDupAddSql model, int updateBy)
        {
            var pars = GetParams(model,ignoreKey: new string[]
                        {
                            nameof(model.CreatedBy),
                            nameof(model.CreatedTime),
                            nameof(model.UpdatedTime),
                              nameof(model.StatusValue),
                            nameof(model.UpdatedBy),
                        });
            pars.Add(nameof(model.UpdatedBy), updateBy);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_UpdateCustomer_v2", pars, commandType: CommandType.StoredProcedure);
                    return RepoResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return RepoResponse<bool>.Create(false, GetException(e));
            }


        }
    }
}
