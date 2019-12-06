using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Employee;

namespace VS_LOAN.Core.Business
{
    public class EmployeeBusiness :BaseBusiness
    {
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
    }
}
