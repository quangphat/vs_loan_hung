using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.F88Model
{
    public class LadipageModel
    {
        public string  Name { get; set; }
        public string Phone { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Link { get; set; }
        public int ReferenceType { get; set; }
        public int TransactionId { get; set; }
        public int ReferenceId { get; set; }
        public int CurrentGroupId { get; set; }
    }
}
