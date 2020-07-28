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
    public class SystemconfigRepository : BaseRepository, ISystemconfigRepository
    {
        public SystemconfigRepository() : base(typeof(SystemconfigRepository))
        {
        }

        public async Task<Systemconfig> GetByCode(string code)
        {
            using (var con = GetConnection())
            {
                var result = con.QueryFirstOrDefault<Systemconfig>("sp_SystemConfig_GetByCode", new { code }, commandType: System.Data.CommandType.StoredProcedure);
                if(result ==null)
                {
                    result = new Systemconfig {Code ="", Value = 500 };
                }
                return result;
            }
        }
    }
}
