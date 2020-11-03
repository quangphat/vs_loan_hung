using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{

    public class CheckValidOdcRequestModel
    {

        public string MobilePhone { get; set; }

        public string IdNo { get; set; }
    }
    public class CheckVavlidOdcResponseModel
    {

        public string Status { get; set; }

        public string Messsage { get; set; }
        public ValidDataOdbResponse Data { get; set; }

    }

    public class ValidDataOdbResponse
    {
        public bool ValidData { get; set; }
    }
    
}
