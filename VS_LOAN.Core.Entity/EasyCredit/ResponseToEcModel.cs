using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class ResponseToEcModel
    {
        public ResponseToEcModel()
        {
            code = "INFORMED";
            message = "Case was informed.";
        }
        public string code { get; set; }
        public string message { get; set; }
    }
}
