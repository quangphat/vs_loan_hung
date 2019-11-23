﻿using Dapper;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Nhibernate;

namespace VS_LOAN.Core.Business
{
    public abstract class BaseBusiness
    {
        protected IDbConnection _connection;
        public BaseBusiness()
        {
            var cfg = new Configuration();
            cfg.Configure(System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, DBConfig.DB_LOAN));
            string connectionString = cfg.GetProperty("connection.connection_string");
            _connection = new SqlConnection(connectionString);
        }

        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
        public void CloseConnection()
        {
            if(_connection!=null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}