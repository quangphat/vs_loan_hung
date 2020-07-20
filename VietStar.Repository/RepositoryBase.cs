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
        protected int offset;
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
        protected void ProcessInputPaging(int page, ref int limit, out int offset)
        {
            page = page <= 0 ? 1 : page;
            if (limit <= 0)
                limit = 20;
            if (limit > 1000)
                limit = 100;
            offset = (page - 1) * limit;
        }
    }
}
