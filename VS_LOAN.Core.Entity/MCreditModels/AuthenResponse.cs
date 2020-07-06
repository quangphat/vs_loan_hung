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
        public OptionSimple Cities { get; set; }
        public OptionSimple LoanPeriods { get; set; }
    }
}
