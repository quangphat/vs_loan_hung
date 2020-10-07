using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class CheckVavlidOdcResponseModel
    {
        
        public string status { get; set; }
        public object errorCode { get; set; }

        public string messsage { get; set; }

        public object data { get; set; }

    }
    
}
