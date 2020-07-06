using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class AuthenResponseObj
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string CurrentVersion { get; set; }
    }
}
