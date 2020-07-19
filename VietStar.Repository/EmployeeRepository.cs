using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Account> Login(string userName, string password)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Account>($"sp_NHAN_VIEN_Login", new
                {
                    userName,
                    password
                }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
    }
}
