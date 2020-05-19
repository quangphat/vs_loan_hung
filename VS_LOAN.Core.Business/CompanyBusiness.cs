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

namespace VS_LOAN.Core.Business
{
    public class CompanyBusiness : BaseBusiness
    {
        public CompanyBusiness() : base(typeof(CompanyBusiness))
        {

        }
        public int Create(Company model)
        {
            using (var con = GetConnection())
            {
                var p = AddOutputParam("id");
                p.Add("fullname", model.FullName);
                p.Add("checkdate", model.CheckDate);
                p.Add("note", model.LastNote);
                p.Add("createdtime", DateTime.Now);
                p.Add("createdby", model.CreatedBy);
                p.Add("TaxNumber", model.TaxNumber);
                p.Add("PartnerId", model.PartnerId);
                p.Add("CatType", model.CatType);
                con.Execute("sp_InsertCompany", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }
               
        }
        public bool Update(Company model)
        {
            var p = new DynamicParameters();
            p.Add("id", model.Id);
            p.Add("fullname", model.FullName);
            p.Add("checkdate", model.CheckDate);
            p.Add("status", model.Status);
            p.Add("note", string.IsNullOrWhiteSpace(model.LastNote) ? null : model.LastNote);
            p.Add("TaxNumber", model.TaxNumber);
            p.Add("CatType", model.CatType);
            p.Add("updatedtime", DateTime.Now);
            p.Add("updatedby", model.UpdatedBy);
            _connection.Execute("sp_UpdateCompany", p, commandType: CommandType.StoredProcedure);
            return true;
        }
        public Company GetById(int id)
        {
            string sql = $"select * from Company where Id = @id";
            var customer = _connection.QueryFirstOrDefault<Company>(sql, new
            {
                id = id
            }, commandType: CommandType.Text);
            return customer;
        }
        
        
        public int Count(string freeText)
        {
            var p = new DynamicParameters();
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            p.Add("freeText", freeText);
            var total = _connection.ExecuteScalar<int>("sp_CountCompany", p, commandType: CommandType.StoredProcedure);
            return total;
        }
        public List<Company> Gets(
            string freeText,
            int page,
            int limit)
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
            var results = _connection.Query<Company>("sp_GetCompnay", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
    }
}
