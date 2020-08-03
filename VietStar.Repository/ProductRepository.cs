using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        protected readonly ILogRepository _rpLog;
        public ProductRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<OptionSimple>> GetsAync(int partnerId,int orgId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_SAN_PHAM_VAY_LayDSByID_v2", new { partnerId, orgId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

