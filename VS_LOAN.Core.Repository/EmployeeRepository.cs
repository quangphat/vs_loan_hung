using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Employee;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using System.Data.SqlClient;
using NHibernate;

namespace VS_LOAN.Core.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository() : base(typeof(EmployeeRepository))
        {

        }
        public async Task<bool> CheckIsInvokeOrg(int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.ExecuteScalarAsync<bool>("sp_CheckIsAdmin", new { userId }, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<OptionSimple> GetEmployeeByCode(string code)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<OptionSimple>("sp_Employee_GetByCode", new { code }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<bool> CheckIsAdmin(int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.ExecuteScalarAsync<bool>("sp_CheckIsAdmin", new { userId }, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<int>> GetPeopleIdCanViewMyProfile(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<int>("sp_GetUserIDCanViewMyProfile", new { userId }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        public async Task<List<NhanVienInfoModel>> GetCourierList()
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<NhanVienInfoModel>("sp_NHAN_VIEN_LayDSCourierCode", null, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetByProvinceId(int provinceId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("spGetAllUserByProvinceId", new { provinceId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }


        }
        public async Task<List<IDictionary<string, object>>> QuerySQLAsync(string sql)
        {


            using (var db = GetConnection())
            {

                var salesOrders = await db.QueryAsync(sql, commandType: CommandType.Text, commandTimeout: 500);
                return salesOrders.Select(a => (IDictionary<string, object>)a).ToList();
            }
        }
        public async Task<List<OptionSimple>> GetByDistrictId(int districtId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("spGetAllUserByDistrictId", new { districtId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }


        }
        public async Task<Nhanvien> GetByUserName(string userName, int userId)
        {
            
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>("sp_Employee_GetByUsername", new { userName, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
        public async Task<Nhanvien> GetByCode(string code, int userId)
        {
            
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>("sp_Employee_GetByCode", new { code,userId  }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
        public async Task<int> Count(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText)
        {
            var p = new DynamicParameters();
            p.Add("workFromDate", workFromDate);
            p.Add("workToDate", workToDate);
            p.Add("roleId", roleId);
            p.Add("freeText", freeText);
            using (var con = GetConnection())
            {
                var total = await con.ExecuteScalarAsync<int>("sp_CountEmployee", p, commandType: CommandType.StoredProcedure);
                return total;
            }

        }
        public async Task<bool> ResetPassord(int id, string password, int updatedBy)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_ResetPassword",
                    new {
                        id,
                        password,
                        updatedBy
                    }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<List<EmployeeViewModel>> Gets(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText,
            int page,
            int limit, int OrgId)
        {
            var p = new DynamicParameters();
            p.Add("workFromDate", workFromDate);
            p.Add("workToDate", workToDate);
            p.Add("freeText", freeText);
            p.Add("page", page);
            p.Add("roleId", roleId);
            p.Add("limit", limit);
            p.Add("OrgId", OrgId);

            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<EmployeeViewModel>("sp_GetEmployees", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
        public async Task<Nhanvien> GetByIdAsync(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>("sp_GetEmployeeById", new { userId }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
        public async Task<List<OptionSimple>> GetRoleList(int userId)
        {
          
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_Role_GetRoles",new { userId}, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }


        }
        public async Task<bool> Update(EmployeeEditModel entity)
        {
            var p = new DynamicParameters();
            p.Add("id", entity.Id);
          
            p.Add("fullName", entity.FullName);
            p.Add("phone", entity.Phone);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            
            p.Add("UpdatedBy", entity.UpdatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Employee_UpdateUser_v2", p, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<int> Create(UserCreateModel entity)
        {
            var p = AddOutputParam("id");
            p.Add("code", entity.Code);
           
            p.Add("userName", entity.UserName);
            p.Add("password", entity.Password);
            p.Add("fullName", entity.FullName);
            p.Add("phone", entity.Phone);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            p.Add("createdby", entity.CreatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Employee_InsertUser_v2", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }

        }
        public async Task<List<OptionSimple>> GetAllEmployee(int orgId)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetFull", new { orgId }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetAllEmployeePaging(int orgId, int page, string freeText)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetPaging", new { orgId, page, freeText }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetEmployeesByGroupId(int groupId, bool isLeader = false)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_NHOM_GetEmployeesByGroupId",
                    new { groupId, isGetLeader = isLeader },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public bool CapNhat(int maNhanVien, List<int> lstIDNhom)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand commandXoaNhanVienNhom = new SqlCommand();
                    commandXoaNhanVienNhom.Connection = session.Connection;
                    commandXoaNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandXoaNhanVienNhom.CommandText = "sp_NHAN_VIEN_CF_Xoa";
                    session.Transaction.Enlist(commandXoaNhanVienNhom);
                    commandXoaNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", maNhanVien));
                    commandXoaNhanVienNhom.ExecuteNonQuery();

                    IDbCommand commandNhanVienNhom = new SqlCommand();
                    commandNhanVienNhom.Connection = session.Connection;
                    commandNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandNhanVienNhom.CommandText = "sp_NHAN_VIEN_CF_Them";
                    session.Transaction.Enlist(commandNhanVienNhom);
                    if (lstIDNhom != null)
                    {
                        for (int i = 0; i < lstIDNhom.Count; i++)
                        {
                            commandNhanVienNhom.Parameters.Clear();
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhom", lstIDNhom[i]));
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", maNhanVien));
                            commandNhanVienNhom.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<List<OptionSimple>> LayDSThanhVienNhomCaConAsync(int groupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v2",
                        new { groupId, userId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
