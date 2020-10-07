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
    public class OcbRepository : BaseRepository, IOcbRepository
    {

        public OcbRepository() : base(typeof(OcbRepository))
        {
        }

        public async Task<int> CreateDraftProfile(OcbProfile model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
            
            });

                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_insert_Ocb_Item", param, commandType: CommandType.StoredProcedure);
                    return param.Get<int>("Id");
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public Task<List<int>> GetPeopleCanViewMyProfile(int profileId)
        {
            throw new NotImplementedException();
        }

      

        public async Task<List<OcbSerarchSql>> GetTempProfiles(int page, int limit, string freeText, int userId,
         string status = null,
         DateTime? fromDate = null,
         DateTime? toDate = null,
         int dateType = 0,
         int maNhom = 0,
         int maThanhVien = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OcbSerarchSql>("sp_ocb_Gets", new
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

        public async Task<OcbProfile> GetTemProfileByMcId(int id)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<OcbProfile>("sp_ocb_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public Task<bool> InsertPeopleWhoCanViewProfile(int profileisUpdateMCIId, string peopleIds)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDraftProfile(OcbProfile model)
        {
            var param = GetParams(model, ignoreKey: new string[]
            {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.CreatedBy)
               
            });

            using (var con = GetConnection())
            {
                var storeExecute = "sp_update_ocb";
                await con.ExecuteAsync(storeExecute, param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
