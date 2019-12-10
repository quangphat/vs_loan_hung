using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Employee;

namespace VS_LOAN.Core.Business
{
    public class EmployeeBusiness :BaseBusiness
    {
        public async Task<Nhanvien> GetByUserName(string userName, int id = 0)
        {
            string query = "select * from Nhan_Vien where Ten_Dang_Nhap = @userName";
            if (id > 0)
            {
                query += " and ID <> @id";
            }
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>(query, new { @userName = userName, @id = id }, commandType: CommandType.Text);
                return result;
            }

        }
        public async Task<Nhanvien> GetByCode(string code, int id = 0)
        {
            string query = "select * from Nhan_Vien where Ma = @code";
            if (id > 0)
            {
                query += " and ID <> @id";
            }
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>(query, new { code = code, @id = id }, commandType: CommandType.Text);
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
                var total = await con.ExecuteScalarAsync<int>("sp_CountNhanvien", p, commandType: CommandType.StoredProcedure);
                return total;
            }

        }
        public async Task<List<EmployeeViewModel>> Gets(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText,
            int page,
            int limit)
        {
            var p = new DynamicParameters();
            p.Add("workFromDate", workFromDate);
            p.Add("workToDate", workToDate);
            p.Add("freeText", freeText);
            p.Add("page", page);
            p.Add("roleId", roleId);
            p.Add("limit", limit);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<EmployeeViewModel>("sp_GetNhanvien", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
        public async Task<Nhanvien> GetById(int id)
        {
            string query = "select * from Nhan_Vien where ID = @id";
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>(query, new { @id = id }, commandType: CommandType.Text);
                return result;
            }

        }
        public async Task<List<OptionSimple>> GetRoleList()
        {
            string query = "select Id, Name from Role where isnull(Deleted,0) =0";
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>(query, commandType: CommandType.Text);
                return result.ToList();
            }


        }
        public async Task<bool> Update(EmployeeEditModel entity)
        {
            var p = new DynamicParameters();
            p.Add("id", entity.Id);
            p.Add("provinceId", entity.ProvinceId);
            p.Add("districtId", entity.DistrictId);
            p.Add("fullName", entity.FullName);
            p.Add("phone", entity.Phone);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            p.Add("workDate", entity.WorkDate);
            p.Add("UpdatedTime", DateTime.Now);
            p.Add("UpdatedBy", entity.UpdatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_UpdateUser", p, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<int> Create(UserCreateModel entity)
        {
            var p = AddOutputParam("id");
            p.Add("code", entity.Code);
            p.Add("ProvinceId", entity.ProvinceId);
            p.Add("DistrictId", entity.DistrictId);
            p.Add("userName", entity.UserName);
            p.Add("password", entity.Password);
            p.Add("fullName", entity.FullName);
            p.Add("phone", entity.Phone);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            p.Add("workDate", entity.WorkDate);
            p.Add("createdtime", DateTime.Now);
            p.Add("createdby", entity.CreatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_InsertUser", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }

        }
    }
}
