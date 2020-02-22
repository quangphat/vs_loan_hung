using Dapper;
using log4net;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Nhibernate;

namespace VS_LOAN.Core.Business
{
    public abstract class BaseBusiness
    {
        protected IDbConnection _connection;
        private readonly string _connectionString;
        protected readonly ILog _log;
        protected HttpClient _httpClient;
        public BaseBusiness(Type inheritBiz, HttpClient httpClient = null)
        {
            var cfg = new Configuration();
            cfg.Configure(System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, DBConfig.DB_LOAN));
            _connectionString = cfg.GetProperty("connection.connection_string");
            _connection = new SqlConnection(_connectionString);
            _log = LogManager.GetLogger(inheritBiz);
            _httpClient = httpClient;
        }

        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_connectionString);
            con.Open();
            return con;
        }
         
    }
}
