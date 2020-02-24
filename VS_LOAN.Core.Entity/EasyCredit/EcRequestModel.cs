using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class EcRequestModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public EcRequestDataModel data { get; set; }
    }
    public class EcRequestDataModel
    {
        public string request_id { get; set; }
        public string proposal_id { get; set; }
        public decimal? loan_amount { get; set; }
        public int? loan_tenor { get; set; }
        public decimal? installment_amount { get; set; }
    }
}
