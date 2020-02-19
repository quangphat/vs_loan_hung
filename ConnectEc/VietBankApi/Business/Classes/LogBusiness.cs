using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;
using VietBankApi.Infrastructures;

namespace VietBankApi.Business.Classes
{
    public class LogBusiness : BaseBusiness, ILogBusiness
    {

        public LogBusiness(CurrentProcess currentProcess, IOptions<ApiSetting> appSettings) : base(currentProcess, appSettings)
        {
        }
        public async Task<bool> InfoLog(string name, string content= null)
        {
            var p = new DynamicParameters();
            p.Add("name", name);
            p.Add("content", content);

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_InsertLog", p, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
