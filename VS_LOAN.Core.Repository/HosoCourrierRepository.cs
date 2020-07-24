using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class HosoCourrierRepository : BaseRepository, IHosoCourrierRepository
    {
        public HosoCourrierRepository() : base(typeof(HosoCourrierRepository))
        {

        }
        public async Task<UserPMModel> GetEmployeeById(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<UserPMModel>("sp_NHAN_VIEN_LayDSByMaQL", new { MaQL = id },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<HosoCourierViewModel> GetById(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<HosoCourierViewModel>("sp_GetHosoCourierById", new { id = id },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<int> GetGroupIdByNguoiQuanLyId(int leaderId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<int>("sp_GetMaNhomChaByMaNguoiQuanLy", new { leaderId },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<bool> Update(int id, HosoCourier hoso)
        {
            using (var con = GetConnection())
            {
                var param = GetParams(hoso, ignoreKey: new string[] {
                    nameof(hoso.CreatedTime),
                    nameof(hoso.UpdatedTime),
                    nameof(hoso.AssigneeIds),
                    nameof(hoso.CreatedBy)
                });

                await con.ExecuteAsync("sp_UpdateHosoCourier", param, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<int> Create(HosoCourier hoso, int groupId = 0)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var param = GetParams(hoso, "Id", DbType.Int32, ignoreKey: new string[] {
                    nameof(hoso.CreatedTime),
                    nameof(hoso.UpdatedTime),
                    nameof(hoso.UpdatedBy),
                    nameof(hoso.AssigneeIds)

                });
                    await con.ExecuteAsync("sp_InsertHosoCourrier", param, commandType: CommandType.StoredProcedure);
                    return param.Get<int>("Id");
                }
            }
           catch(Exception e)
            {
                return 0;
            }

        }
        public async Task<bool> InsertCourierAssignee(int courierId, int assigneeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("CourierId", courierId);
                p.Add("AssigneeId", assigneeId);
                await con.ExecuteAsync("sp_CourierAssignee_Insert", p, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<int> CountHosoCourrier(string freeText, int userId, int courierId, string status, int groupId = 0, int provinceId = 0, string saleCode = null)
        {
            status = string.IsNullOrWhiteSpace(status) ? string.Empty : status;
            using (var con = GetConnection())
            {
                return await con.ExecuteScalarAsync<int>("sp_CountHosoCourier", new { freeText, userId, courierId, status, groupId, provinceId, saleCode }, commandType: CommandType.StoredProcedure);

            }
        }
        public async Task<List<HosoCourierViewModel>> GetHosoCourrier(string freeText, int userId, int courierId, string status, int page, int limit, int groupId = 0, int provinceId = 0, string saleCode = null)
        {
            status = string.IsNullOrWhiteSpace(status) ? string.Empty : status;
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<HosoCourierViewModel>("sp_GetHosoCourier",
                    new { freeText,userId, courierId, page, limit_tmp = limit, status, groupId, provinceId, saleCode },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
