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
    public class EcEmploymentRepository : BaseRepository, IEcEmploymentRepository
    {
        public async Task<List<StringOptionSimple>> GetEmployment(string type)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<StringOptionSimple>("sp_GetEcEmploymenType", new
                {
                    type
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

