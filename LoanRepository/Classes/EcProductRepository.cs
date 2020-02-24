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
    public class EcProductRepository : BaseRepository, IEcProductRepository
    {
        
        public async Task<List<OptionEcProductType>> GetSimples(string occupationCode)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<OptionEcProductType>("sp_GetEcProduct", new
                {
                    occupationCode
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

