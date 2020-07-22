using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        public LogRepository() : base(typeof(LogRepository))
        {
        }

        public async Task<bool> InsertLog(string name, string content)
        {
            
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("sp_InsertLog", new {
                    Name = name,
                    Content = content
                },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
