using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class CustomerNote:SqlBaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Note { get; set; }
    }
}
