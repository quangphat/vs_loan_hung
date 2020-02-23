using Dapper;
using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace LoanRepository.Classes
{
    public class EcLocationRepository : BaseRepository, IEcLocationRepository
    {
        public async Task<List<StringOptionSimple>> GetIssuePlace()
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<StringOptionSimple>("sp_GetEcIssuePlace",null, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetDistricts(int provinceId)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<OptionSimple>("sp_GetEcLocation", new
                {
                    type = 2,
                    provinceCode = provinceId
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetProvinces()
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<OptionSimple>("sp_GetEcLocation", new
                {
                    type = 1
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetWards(int districtId)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<OptionSimple>("sp_GetEcLocation", new
                {
                    type = 0,
                    provinceCode = 0,
                    districtCode = districtId
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
