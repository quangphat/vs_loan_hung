using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LoanRepository.Classes
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        public BaseRepository()
        {
            var _connectionString = System.Configuration.ConfigurationManager.AppSettings["sql_connection_string"];    
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
