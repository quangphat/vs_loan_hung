using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class CheckCICResponseModel : MCResponseModelBase
    {
        public string Result { get; set; }
        public string Relate { get; set; }
    }
}
