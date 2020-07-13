using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class AuthenResponse
    {
        public AuthenResponseObj Obj { get; set; }
        public List<MCreditCity> Cities { get; set; }
        public List<MCreditLoanPeriod> LoanPeriods { get; set; }
        public List<MCreditProduct> Products { get; set; }
        public List<MCreditlocations> Locations { get; set; }
        public Dictionary<string, string> ProfileStatus { get; set; }
    }
    
}
