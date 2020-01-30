using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class CustomerEditModel
    {
        public CustomerModel Customer { get; set; }
        public List<OptionSimple> Partners { get; set; }
    }
}
