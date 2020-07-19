using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
namespace VietStar.Repository
{
    public abstract class RepositoryBase
    {
        private IDbConnection _connection;
        protected readonly IConfiguration _configuration;

        public RepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("kingoffice"));
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_configuration.GetConnectionString("kingoffice"));
            con.Open();
            return con;
        }
        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
    }
}
