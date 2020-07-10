using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business
{
    public class PartnerBLL:BaseRepository
    {
        public PartnerBLL():base(typeof(PartnerBLL))
        {

        }
        public async Task<List<OptionSimple>> GetListForCheckCustomerDuplicateAsync()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_getListPartnerForCustomerCheck", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
               
        }
    }
}
