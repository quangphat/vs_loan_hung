using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class ResponseModel
    {
        public List<string> Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string SystemMessage { get; set; }
    }
}
