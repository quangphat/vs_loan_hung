using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VietBankApi.Infrastructures;

namespace VietBankApi.Business.Classes
{
    public class BaseBusiness
    {
        protected readonly ApiSetting _appSetting;
        public readonly CurrentProcess _process;
        public BaseBusiness(CurrentProcess currentProcess, IOptions<ApiSetting> appSettings)
        {
            _process = currentProcess;
            _appSetting = appSettings.Value;
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_appSetting.SqlConn);
            con.Open();
            return con;
        }
    }
}
