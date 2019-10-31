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
    public class PartnerBLL:BaseBusiness
    {
        public PartnerBLL():base()
        {

        }
        public List<OptionSimple> GetListForCheckCustomerDuplicate()
        {
            var result = _connection.Query<OptionSimple>("sp_getListPartnerForCustomerCheck", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
