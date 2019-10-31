using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;

namespace VS_LOAN.Core.Business
{
    public class CustomerBLL :BaseBusiness
    {
        public CustomerBLL() :base()
        {

        }
        public int  Create(Customer customer)
        {
            var p = AddOutputParam("id");
            p.Add("fullname", customer.FullName);
            p.Add("phone", customer.Phone);
            p.Add("cmnd", customer.Cmnd);
            p.Add("createdtime", DateTime.Now);
            p.Add("status", customer.CICStatus);
            p.Add("note", customer.LastNote);
            p.Add("createdby", customer.CreatedBy);
            p.Add("gender", customer.Gender);
            _connection.Execute("sp_InsertCustomer", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("id");
        }
        public bool AddNote(CustomerNote note)
        {
            string sql = $"insert into CustomerNote(CustomerId, Note,CreatedTime,CreatedBy) values (@customerId,@note,@createdTime,@createdBy)";
            _connection.Execute(sql, new {
                customerId = note.CustomerId,
                note = note.Note,
                createdTime = DateTime.Now,
                createdBy = note.CreatedBy
            }, commandType: CommandType.Text);
            return true;
        }
        public bool Update(Customer customer)
        {
            var p = new DynamicParameters();
            p.Add("id", customer.Id);
            p.Add("fullname", customer.FullName);
            p.Add("phone", customer.Phone);
            p.Add("cmnd", customer.Cmnd);
            p.Add("status", customer.CICStatus);
            p.Add("note", customer.LastNote);
            p.Add("gender", customer.Gender);
            p.Add("updatedtime", DateTime.Now);
            p.Add("updatedby", customer.CreatedBy);
            _connection.Execute("sp_UpdateCustomer", p, commandType: CommandType.StoredProcedure);
            return true;
        }
        public Customer GetById(int customerId)
        {
            string sql = $"select * from Customer where Id = @id";
            var customer = _connection.QueryFirstOrDefault<Customer>(sql, new
            {
                id = customerId
            }, commandType: CommandType.Text);
            return customer;
        }
        public bool DeleteCustomerCheck(int customerId)
        {
            string sql = $"delete  CustomerCheck where CustomerId = @CustomerId";
            _connection.Execute(sql, new
            {
                CustomerId = customerId
            }, commandType: CommandType.Text);
            return true;
        }
        public List<int> GetCustomerCheckByCustomerId(int customerId)
        {
            string sql = $"select PartnerId from CustomerCheck where CustomerId = @CustomerId";
            var result = _connection.Query<int>(sql, new
            {
                CustomerId = customerId
            }, commandType: CommandType.Text);
            return result.ToList();
        }
        public bool InserCustomerCheck(CustomerCheck check)
        {
            var p = AddOutputParam("id");
            p.Add("customerId", check.CustomerId);
            p.Add("partnerId", check.PartnerId);
            p.Add("status", check.Status);
            p.Add("createdtime", DateTime.Now);
            p.Add("createdby", check.CreatedBy);
            
            _connection.Execute("sp_InserCustomerCheck", p, commandType: CommandType.StoredProcedure);
            return true;
        }
        public int Count(string freeText)
        {
            var p = new DynamicParameters();
           
            p.Add("freeText", freeText);
            var total = _connection.ExecuteScalar<int>("sp_CountCustomer", p, commandType: CommandType.StoredProcedure);
            return total;
        }
        public async Task<List<Customer>> Gets(
            DateTime fromDate,
            DateTime toDate,
            int dateFilterType,
            string freeText,
            int offset,
            int limit)
        {
            var p = new DynamicParameters();
            p.Add("fromDate", fromDate);
            p.Add("toDate", toDate);
            p.Add("freeText", freeText);
            p.Add("dateFilterType", dateFilterType);
            p.Add("offset", offset);
            p.Add("limit", limit);
            var results = _connection.Query<Customer>("sp_GetCustomer", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
    }
}
