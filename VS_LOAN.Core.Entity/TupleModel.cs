using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    //public class TupleModel
    //{
    //    public bool success { get; set; }
    //    public string message { get; set; }
    //}
    public class TupleModel
    {
        public List<HosoCourrier.HosoCourier> Hosos { get; set; }
        public List<int> AssigneeIds { get; set; }
    }
}
