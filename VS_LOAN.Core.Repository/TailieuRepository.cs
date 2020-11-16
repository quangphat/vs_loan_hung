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
    public class TailieuRepository : BaseRepository, ITailieuRepository
    {
        public TailieuRepository() : base(typeof(TailieuRepository)) { }
        public async Task<List<ImportExcelFrameWorkModel>> GetImportTypes(int type)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var reader = await con.QueryAsync<ImportExcelFrameWorkModel>("sp_ImportExcel_GetByType", new { type}
                        , commandType: CommandType.StoredProcedure);
                    //DataTable table = new DataTable();
                    //table.Load(reader);
                    return reader.ToList();
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<LoaiTaiLieuModel>> LayDS()
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_LOAI_TAI_LIEU_LayDS";
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<LoaiTaiLieuModel> rs = new List<LoaiTaiLieuModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        LoaiTaiLieuModel cm = new LoaiTaiLieuModel();
                        cm.ID = Convert.ToInt32(item["ID"].ToString());
                        cm.Ten = item["Ten"].ToString();
                        cm.BatBuoc = Convert.ToInt32(item["BatBuoc"].ToString());
                        rs.Add(cm);
                    }
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<List<LoaiTaiLieuModel>> LayDS()
        //{
        //    try
        //    {
        //        using (var con = GetConnection())
        //        {
        //            var result = await con.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS",
        //                null, commandType: CommandType.StoredProcedure);
        //            return result.ToList();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public async Task<bool> CopyFileFromProfile(int copyProfileId, int profileTypeId, int newProfileId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_ProfileFile_CopyFromProfile",
                     new { copyProfileId, profileTypeId, newProfileId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateExistingFile(TaiLieu model, int fileId)
        {
            var p = GetParams(model, ignoreKey: new string[] { nameof(model.FileKey), nameof(model.ProfileId) });
            p.Add("fileId", fileId);
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
        public async Task<bool> RemoveTailieu(int hosoId, int tailieuId)
        {
            var p = new DynamicParameters();
            p.Add("hosoId", hosoId);
            p.Add("tailieuId", tailieuId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("removeTailieu", p,
                    commandType: CommandType.StoredProcedure);
                return true;
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
                p.Add("Folder", model.Folder);
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
        public async Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId, int type)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("profileId", hosoId);
                p.Add("profileTypeId", type);
                var result = await con.QueryAsync<FileUploadModel>("getTailieuByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<FileUploadModel>> GetTailieuByMCId(string mcId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("mcId", mcId);

                var result = await con.QueryAsync<FileUploadModel>("getTailieuByMCId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<bool> UpdateTailieuHosoMCId(int profileId, string mcId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_TaiLieuHoso_UpdateMCId", new
                {
                    profileId,
                    mcId
                },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<List<FileUploadModel>> GetTailieuOCByHosoId(int hosoId, int type)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("profileId", hosoId);
                p.Add("profileTypeId", type);
                var result = await con.QueryAsync<FileUploadModel>("getTailieuOCBByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<bool> RemoveTailieuOcb(int hosoId, int tailieuId)
        {
            var p = new DynamicParameters();
            p.Add("hosoId", hosoId);
            p.Add("tailieuId", tailieuId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("removeTailieuOCB", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> RemoveAllTailieuOcb(int hosoId, int typeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("ProfileId", hosoId);
                p.Add("ProfileTypeId", typeId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_XoaTatCaOCB", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }



        public async Task<bool> AddOCB(TaiLieu model)
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
                await con.ExecuteAsync("sp_TAI_LIEU_HS_ThemOCB", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }

        public async Task<bool> UpdateExistingFileOCB(TaiLieu model, int fileId)
        {
            var p = GetParams(model, ignoreKey: new string[] { nameof(model.FileKey), nameof(model.ProfileId) });
            p.Add("fileId", fileId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("updateExistingFileOcb", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }

  
    }
}
