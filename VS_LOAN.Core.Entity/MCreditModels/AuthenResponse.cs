using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class AuthenResponse
    {
        public AuthenResponseObj Obj { get; set; }
        public List<OptionSimple> Cities { get; set; }
        public List<OptionSimple> LoanPeriods { get; set; }
    }
}
