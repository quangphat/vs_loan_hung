using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
        public class CustomerModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public bool Gender { get; set; }
        public string Note { get; set; }
    }
}
