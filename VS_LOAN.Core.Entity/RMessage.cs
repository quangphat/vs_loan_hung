using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Entity.Model
{
    public class RMessage
    {
        public bool success { get; set; }
        public object data { get; set; }
        public string code { get; set; }
        
    }
}