using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class EcBaseModel : BaseSqlEntity
    {
        public string RequestId
        {
            get
            {
                return ModelExtensions.GenEcRequestId("VB0");
            }
        }
        public string PartnerCode
        {
            get
            {
                return "VB0";
            }
        }
    }
}
