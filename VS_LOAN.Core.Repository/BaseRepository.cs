using Dapper;
using log4net;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Nhibernate;

namespace VS_LOAN.Core.Repository
{
    public abstract class BaseRepository
    {
        protected IDbConnection _connection;
        private readonly string _connectionString;
        protected readonly ILog _log;
        public BaseRepository(Type inheritBiz)
        {
            var cfg = new Configuration();
            cfg.Configure(System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, DBConfig.DB_LOAN));
            _connectionString = cfg.GetProperty("connection.connection_string");
            _connection = new SqlConnection(_connectionString);
            _log = LogManager.GetLogger(inheritBiz);
        }

        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
        protected DynamicParameters GetParams<T>(T model, string outputParam = "", DbType type = DbType.Int32, string[] ignoreKey = null)
        {
            var p = new DynamicParameters();
            var properties = model.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(model);
                if (ignoreKey != null && ignoreKey.Contains(key))
                    continue;
                if(!string.IsNullOrWhiteSpace(outputParam))
                {
                    if (key.ToLower() == outputParam.ToLower())
                    {
                        p.Add(key, value, dbType: type, direction: ParameterDirection.Output);
                        continue;
                    }
                }
                p.Add(key, value);
            }
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
