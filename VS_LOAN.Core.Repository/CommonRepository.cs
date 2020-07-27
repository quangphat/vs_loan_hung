using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Repository
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        public CommonRepository() : base(typeof(CommonRepository))
        {
        }

        public async Task<List<OptionSimple>> GetProfileStatusByCode(string profileType, int orgId, bool isGetAll = false, int roleId = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_ProfileStatus_Gets", new { orgId, profileType, isGetAll, roleId }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
