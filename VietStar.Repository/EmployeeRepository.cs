using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
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
                        new {orgId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetMemberByGroupId(int groupId, int userId)
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
    }
}
