using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Repository
{
    public class TailieuRepository : BaseRepository, ITailieuRepository
    {

        public TailieuRepository() : base(typeof(TailieuRepository)) { }
        public async Task<bool> UpdateExistingFile(int fileId, string name, string url, int typeId = 1)
        {
            var p = new DynamicParameters();
            p.Add("fileId", fileId);
            p.Add("name", name);
            p.Add("url", url);
            p.Add("typeId", typeId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("updateExistingFile", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int profileType = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS", new { type = profileType }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<bool> RemoveAllTailieu(int hosoId, int typeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("ProfileId", hosoId);
                p.Add("ProfileTypeId", typeId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_XoaTatCa", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<bool> Add(TaiLieu model)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("FileKey", model.FileKey);
                p.Add("FilePath", model.FilePath);
                p.Add("FileName", model.FileName);
                p.Add("ProfileId", model.ProfileId);
                p.Add("ProfileTypeId", model.ProfileTypeId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_Them", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<bool> AddMCredit(MCTailieuSqlModel model)
        {
            using (var con = GetConnection())
            {
                var p = GetParams(model);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_Them", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
