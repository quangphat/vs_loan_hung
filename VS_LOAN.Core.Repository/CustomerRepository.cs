using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository() : base(typeof(CustomerRepository))
        {

        }
        public async Task<int> Create(Customer customer)
        {
            using (var con = GetConnection())
            {
                var p = AddOutputParam("id");
                p.Add("fullname", customer.FullName);
                p.Add("checkdate", customer.CheckDate);
                p.Add("cmnd", customer.Cmnd);
                p.Add("createdtime", DateTime.Now);
                p.Add("status", customer.CICStatus);
                p.Add("note", customer.LastNote);
                p.Add("createdby", customer.CreatedBy);
                p.Add("gender", customer.Gender);
                p.Add("IsMatch", customer.IsMatch);
                p.Add("PartnerId", customer.PartnerId);
                p.Add("match", customer.MatchCondition);
                p.Add("notMatch", customer.NotMatch);
                p.Add("ProvinceId", customer.ProvinceId);
                p.Add("Address", customer.Address);
                p.Add("BirthDay", customer.BirthDay);
                p.Add("Phone", customer.Phone);
                p.Add("Salary", customer.Salary);
                using (var _connection = GetConnection())
                {
                    await con.ExecuteAsync("sp_InsertCustomer", p, commandType: CommandType.StoredProcedure);
                    return p.Get<int>("id");
                }
               
            }
               
        }
        public async Task<bool> AddNote(CustomerNote note)
        {
            string sql = $"insert into CustomerNote(CustomerId, Note,CreatedTime,CreatedBy) values (@customerId,@note,@createdTime,@createdBy)";
            using (var _connection = GetConnection())
            {
                await _connection.ExecuteAsync(sql, new
                {
                    customerId = note.CustomerId,
                    note = note.Note,
                    createdTime = DateTime.Now,
                    createdBy = note.CreatedBy
                }, commandType: CommandType.Text);
                return true;
            }
           
        }
        public async Task<bool> Update(Customer customer)
        {
            var p = new DynamicParameters();
            p.Add("id", customer.Id);
            p.Add("fullname", customer.FullName);
            p.Add("checkdate", customer.CheckDate);
            p.Add("cmnd", customer.Cmnd);
            p.Add("status", customer.CICStatus);
            p.Add("note", string.IsNullOrWhiteSpace(customer.LastNote) ? null : customer.LastNote);
            p.Add("gender", customer.Gender);
            p.Add("match", customer.MatchCondition);
            p.Add("notmatch", customer.NotMatch);
            p.Add("updatedtime", DateTime.Now);
            p.Add("updatedby", customer.UpdatedBy);
            p.Add("ProvinceId", customer.ProvinceId);
            p.Add("Address", customer.Address);
            p.Add("BirthDay", customer.BirthDay);
            p.Add("Phone", customer.Phone);
            p.Add("Salary", customer.Salary);
            using (var _connection = GetConnection())
            {
                await _connection.ExecuteAsync("sp_UpdateCustomer", p, commandType: CommandType.StoredProcedure);
                return true;
            }
           
        }
        public async Task<Customer> GetById(int customerId)
        {
            string sql = $"select * from Customer where Id = @id";
            using (var _connection = GetConnection())
            {
                var customer = _connection.QueryFirstOrDefault<Customer>(sql, new
                {
                    id = customerId
                }, commandType: CommandType.Text);
                return customer;
            }
            
        }
        public async Task<bool> DeleteCustomerCheck(int customerId)
        {
            string sql = $"delete  CustomerCheck where CustomerId = @CustomerId";
            using (var _connection = GetConnection())
            {
                await _connection.ExecuteAsync(sql, new
                {
                    CustomerId = customerId
                }, commandType: CommandType.Text);
                return true;
            }
           
        }
        public async Task<List<int>> GetCustomerCheckByCustomerId(int customerId)
        {
            string sql = $"select PartnerId from CustomerCheck where CustomerId = @CustomerId";
            using (var _connection = GetConnection())
            {
                var result = await _connection.QueryAsync<int>(sql, new
                {
                    CustomerId = customerId
                }, commandType: CommandType.Text);
                return result.ToList();
            }
           
        }
        public async Task<bool> InserCustomerCheck(CustomerCheck check)
        {
            var p = AddOutputParam("id");
            p.Add("customerId", check.CustomerId);
            p.Add("partnerId", check.PartnerId);
            p.Add("status", check.Status);
            p.Add("createdtime", DateTime.Now);
            p.Add("createdby", check.CreatedBy);
            using (var _connection = GetConnection())
            {
                await _connection.ExecuteAsync("sp_InserCustomerCheck", p, commandType: CommandType.StoredProcedure);
                return true;
            }
            
        }
        public async Task<int> Count(string freeText, int userId)
        {
            var p = new DynamicParameters();
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            p.Add("freeText", freeText);
            p.Add("userId", userId);
            using (var _connection = GetConnection())
            {
                var total = await _connection.ExecuteScalarAsync<int>("sp_CountCustomer", p, commandType: CommandType.StoredProcedure);
                return total;
            }
           
        }
        public async Task<List<Customer>> Gets(
            string freeText,
            int page,
            int limit
            ,int userId)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= Constant.Limit_Max_Page) ? Constant.Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("offset", offset);
            p.Add("limit", limit);
            p.Add("userId", userId);
            using (var _connection = GetConnection())
            {
                var results = await _connection.QueryAsync<Customer>("sp_GetCustomer", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            
        }
        public async Task<List<CustomerNoteViewModel>>  GetNoteByCustomerId(int customerId)
        {
            var p = new DynamicParameters();
            p.Add("customerId", customerId);
            using (var _connection = GetConnection())
            {
                var results = await _connection.QueryAsync<CustomerNoteViewModel>("sp_GetNotesByCustomerId", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
                
        }
    }
}
