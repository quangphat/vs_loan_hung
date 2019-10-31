using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class CustomerCheck:SqlBaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PartnerId { get; set; }
        public int Status { get; set; }
        
    }
}
