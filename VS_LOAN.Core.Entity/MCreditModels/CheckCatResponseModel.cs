using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class CheckCatResponseModel:MCResponseModelBase
    {
        public string name { get; set; }
        public string cat { get; set; }
        public string addr { get; set; }
        public string phone { get; set; }
    }
}
