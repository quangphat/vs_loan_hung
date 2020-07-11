using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MCredit_TempProfileAddModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Hometown { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public string IdNumber { get; set; }

        public string CCCDNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsAddr { get; set; }

        public bool Gender { get; set; }

        public int ProvinceId { get; set; }

        public int? DistrictId { get; set; }
        public string Address { get; set; }

        public string SaleNumber { get; set; }

        public int ProductId { get; set; }

        public string LoanPeriodCode { get; set; }

        public decimal LoanMoney { get; set; }

        public bool IsInsurrance { get; set; }

        public int Status { get; set; }
    }
}
