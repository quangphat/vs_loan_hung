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
    public class CommonRepository : RepositoryBase, ICommonRepository
    {
        protected readonly ILogRepository _rpLog;
        public CommonRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<OptionSimple>> GetProfileStatusByRoleId(string profileType, int orgId, int roleId = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_ProfileStatus_Gets", new { orgId, profileType, roleId }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetProfileStatusByRoleCode(string profileType, int orgId, string roleCode = null)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_ProfileStatus_GetsByroleCode", new { orgId, profileType, roleCode }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
