using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class MCResponseModelBase
    {
        public string status { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
    }
}
