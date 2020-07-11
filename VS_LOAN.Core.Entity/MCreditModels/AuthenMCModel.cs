using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class AuthenMCModel
    {
        public int UserId { get; set; }
        public bool IsUpdateToken { get; set; }
        public bool IsUpdateProduct { get; set; }
        public bool IsUpdateLocation { get; set; }
        public bool IsUpdateLoanPeriod { get; set; }
        public bool IsUpdateCity { get; set; }
    }
}
