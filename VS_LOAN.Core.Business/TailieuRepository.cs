using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business
{
    public class TailieuRepository : BaseRepository, ITailieuRepository
    {
        public TailieuRepository() : base(typeof(TailieuRepository)) { }
        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<bool> RemoveAllTailieu(int hosoId, int typeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("MaHS", hosoId);
                p.Add("typeId", typeId);
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
                p.Add("Maloai", model.TypeId);
                p.Add("DuongDan", model.FilePath);
                p.Add("Ten", model.FileName);
                p.Add("MaHS", model.HosoId);
                p.Add("typeId", model.LoaiHoso);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_Them", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }
    }
}
