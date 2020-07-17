using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class CheckStatusResponseModel : MCResponseModelBase
    {
        public List<CheckStatusResponseObject> Objs { get; set; }
    }
    public class CheckStatusResponseObject
    {
        public string CaseNumber { get; set; }
        public string CustName { get; set; }
        public string Status { get; set; }
    }
}
