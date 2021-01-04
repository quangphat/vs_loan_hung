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
    public class MiraeDeferRepository : BaseRepository, IMiraeDeferRepository
    {
        protected readonly INoteRepository _rpNote;
        public MiraeDeferRepository(INoteRepository rpNote) : base(typeof(OcbRepository))
        {
            _rpNote = rpNote;

        }


        public async Task<List<MiraeDeferSearchModel>> GetTempProfiles(int page, int limit, string freeText, int userId,
        string status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int dateType = 0,
        int maNhom = 0,
        int maThanhVien = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<MiraeDeferSearchModel>("sp_MiraeDefer_Gets", new
                {
                    freeText,
                    userId,
                    page,
                    limit_tmp = limit,
                    status,
                    fromDate,
                    toDate,
                    dateType,
                    maNhom,
                    maThanhVien
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }


        public async Task<int> Add(MiraeDeferModel model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                        nameof(model.CreatedBy),
                nameof(model.UpdatedTime),
                 nameof(model.UpdatedBy),
                 nameof(model.Id)

            });

                using (var con = GetConnection())
                {
                  
                    await con.ExecuteAsync("sp_insert_MiraeDefer_Item", param, commandType: CommandType.StoredProcedure);
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public Task<bool> Resove(int id, int status, int appid, int userid)
        {
            throw new NotImplementedException();
        }

        public async Task<MiraeDeferModel> GetTemProfileByMcId(int id)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<MiraeDeferModel>("sp_miraeDefer_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
    }
}
