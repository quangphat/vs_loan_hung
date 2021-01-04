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
using VS_LOAN.Core.Nhibernate;
using System.Data.SqlClient;

namespace VS_LOAN.Core.Repository
{
    public class MiraeMaratialRepository : BaseRepository, IMiraeMaratialRepository
    {
        public MiraeMaratialRepository() : base(typeof(TailieuRepository)) { }
  


        public async Task<List<FileUploadModel>> GetTailieuMiraeHosoId(int hosoId, int type)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("profileId", hosoId);
                p.Add("profileTypeId", type);
                var result = await con.QueryAsync<FileUploadModel>("getTailieuMiraeByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<bool> RemoveTailieuMirae(int hosoId, int tailieuId)
        {
            var p = new DynamicParameters();
            p.Add("hosoId", hosoId);
            p.Add("tailieuId", tailieuId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("removeTailieuMirae", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int profileType = 8)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEUMirate_LayDS", new { type = 8 }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<bool> RemoveAllTailieuMirae(int hosoId, int typeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("ProfileId", hosoId);
                p.Add("ProfileTypeId", typeId);
                await con.ExecuteAsync("sp_TAI_LIEU_Mirae_XoaTatCa", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }



        public async Task<bool> AddMirae(TaiLieu model)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("FileKey", model.FileKey);
                p.Add("FilePath", model.FilePath);
                p.Add("Folder", model.Folder);
                p.Add("FileName", model.FileName);
                p.Add("ProfileId", model.ProfileId);
                p.Add("ProfileTypeId", model.ProfileTypeId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_ThemMirate", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }

        public async Task<bool> UpdateExistingFileMirae(TaiLieu model, int fileId)
        {
            var p = GetParams(model, ignoreKey: new string[] { nameof(model.FileKey), nameof(model.ProfileId) });
            p.Add("fileId", fileId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("updateExistingFileMirae", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }

  
    }
}
