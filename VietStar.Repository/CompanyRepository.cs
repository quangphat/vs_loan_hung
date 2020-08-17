using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Commons;
using VietStar.Entities.Company;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        protected readonly ILogRepository _rpLog;
        public CompanyRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<List<CompanyIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= Constants.Limit_Max_Page) ? Constants.Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("offset", offset);
            p.Add("limit", limit);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CompanyIndexModel>("sp_GetCompany_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
    }
}

