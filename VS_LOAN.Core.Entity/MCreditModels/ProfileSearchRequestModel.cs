using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class ProfileSearchRequestModel : MCreditRequestModelBase
    {
        public string str { get; set; }
        public string status { get; set; }
        public int page { get; set; }
        public string type { get; set; }
    }
}
