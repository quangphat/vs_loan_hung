using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class AuthenRequestModel
    {
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string Token { get; set; }
    }
}
