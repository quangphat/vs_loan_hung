﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Employee;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<int>> GetPeopleCanViewMyProfile(int profileId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<int>("sp_MCProfilePeople_GetPeopleCanViewProfile", new { profileId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetRoleList(int userId)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_Role_GetRoles", new { userId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<OptionSimple> GetEmployeeByCode(string code, int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<OptionSimple>("sp_Employee_GetByCode", new { code, userId }, commandType: CommandType.StoredProcedure);
                return result;
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

        public async Task<bool> GetStatus(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<bool>("sp_Employee_GetStatus", new
                {
                    userId
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<List<string>> GetPermissions(string roleCode)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<string>("sp_getPermissionByRoleCode", new
                {
                    roleCode,

                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<Account> Login(string userName, string password)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Account>("sp_Employee_Login", new
                {
                    userName,
                    password
                }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<List<OptionSimple>> GetMemberByGroupIdIncludeChild(int groupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v3",
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
        public async Task<List<OptionSimple>> GetCouriers(int orgId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_NHAN_VIEN_LayDSCourierCode_v2",
                        new { orgId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_Employee_Group_LayDSChonThanhVienNhom_v3",
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

        public async Task<List<OptionSimple>> GetAllEmployee(int orgId)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetFull", new { orgId }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetAllEmployeePagingAsync(int orgId, int page, string freeText)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetPaging", new { orgId, page, freeText }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }

        public async Task<List<EmployeeViewModel>> GetsAsync(
           int roleId,
           string freeText,
           int page,
           int limit, int OrgId)
        {
            ProcessInputPaging(ref page, ref limit, out int offset);
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("page", page);
            p.Add("roleId", roleId);
            p.Add("limit", limit);
            p.Add("OrgId", OrgId);

            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<EmployeeViewModel>("sp_GetEmployees_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

    }
}
