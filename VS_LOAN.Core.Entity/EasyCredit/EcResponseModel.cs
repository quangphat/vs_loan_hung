using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class EcResponseModel<T>
    {
        public string code { get; set; }
        public string message { get; set; }
        public object error { get; set; }
        public int status { get; set; }
        public T data { get; set; }
    }
    public class EcDataResponse
    {
        public string request_id { get; set; }
        public string response_message { get; set; }
        public string response_code { get; set; }
    }
}
