using Dapper;
using ImportEcDataTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Repository
{
    public class EcDataRepository
    {
        protected readonly string _connectionString = "Data Source=QUANGPHAT\\CLARKKENT;Initial Catalog=Test13;User ID=sa;Password=number8";
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_connectionString);
            con.Open();
            return con;
        }
        public async Task<bool> InsertProductInfo(EcProductInfo model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_EcProductInfo", new
                {
                    model.Id,
                    model.Code,
                    model.Description_Vi,
                    model.Description_En,
                    model.OccupationCode,
                    model.MinLoanAmount,
                    model.MaxLoanAmount,
                    model.MinTenor,
                    model.MaxTenor
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertPersonalInfo(EcPersonalInfo model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_EcPersonalInfo", new
                {
                    model.Id,
                    model.Code,
                    model.Description_Vi,
                    model.Description_En,
                    model.Type
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertEcIssuePlace(EcIssuePlace model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_EcIssuePlace", new
                {
                    model.Id,
                    model.Code,
                    model.Description_Vi,
                    model.Description_En
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertEcEmployeeType(EcEmployeeType model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_EcEmployeeType", new
                {
                    model.Id,
                    model.RefCode,
                    model.Description_Vi,
                    model.Description_En,
                    model.TypeDescription
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertEcBundle(EcBundle model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_ECBundle", new
                {
                    model.Id,
                    model.DocType,
                    model.Description_Vi,
                    model.Description_En,
                    model.RefBundleCode,
                    model.RefCodeId,
                    model.BundleName
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertEcLocation(EcLocation model)
        {
            
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_ECLocation", new {
                    model.Id,
                    model.WardCode,
                    model.WardName,
                    model.DistrictCode,
                    model.DistrictName,
                    model.ProvinceCode,
                    model.ProvinceName,
                    model.OrderValue

                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> InsertEcBank(EcBank model)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_Insert_ECBank", new
                {
                    model.Id,
                    model.RefIndividual,
                    model.BankCode,
                    model.BankName,
                    model.BranchCode,
                    model.BranchName,
                    model.BankProvince
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
