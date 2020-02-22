using Dapper;
using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business
{
    public class HosoBusiness : BaseBusiness, IHosoBusiness
    {
        public HosoBusiness(CurrentProcess currentProcess) : base(typeof(HosoBusiness),currentProcess)
        {
        }
        public async Task<List<OptionSimple>> GetStatusListByType(int typeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("typeId", typeId);

                var result = await con.QueryAsync<OptionSimple>("sp_GetStatusListByType", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("hosoId", hosoId);
                p.Add("result", f88Result);
                p.Add("reason", reason);
                await con.ExecuteAsync("updateF88Result", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId, int type)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("hosoId", hosoId);
                p.Add("typeId", type);
                var result = await con.QueryAsync<FileUploadModel>("getTailieuByHosoId", p,
                    commandType: CommandType.StoredProcedure);
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
        public async Task<HoSoInfoModel> GetDetail(int id)
        {
            var x = _process.UserName;
            var p = new DynamicParameters();
            p.Add("ID", id);
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<HoSoInfoModel>("sp_HO_SO_LayChiTiet", p,
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
