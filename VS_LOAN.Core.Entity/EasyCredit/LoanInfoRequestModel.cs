using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class LoanInfoRequestModel
    {
        public string request_id { get; set; }
        public string partner_code { get; set; }
        public string customer_name { get; set; }
        public string phone_number { get; set; }
        public string date_of_birth { get; set; }
        public string identity_card_id { get; set; }
        public string issue_date { get; set; }
        public string issue_place { get; set; }
        public string tem_province { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string employment_type { get; set; }
        public string product_type { get; set; }
        public decimal loan_amount { get; set; }
        public int loan_tenor { get; set; }
        public string otp_code { get; set; }
        public string sale_channel { get; set; }
        public string dsa_agent_code { get; set; }
        public string image_selfie { get; set; }
        public string image_id_card { get; set; }
    }
}
