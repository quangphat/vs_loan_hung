using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class MCProfilePostModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string HomeTown { get; set; }

        public string Bod { get; set; }

        public string Phone { get; set; }

        public string IdNumber { get; set; }

        public string CCCDNumber { get; set; }

        public string IdNumberDate { get; set; }

        public bool IsAddr { get; set; }


        public string CityId { get; set; }
        public string SaleId { get; set; }

        public string ProductCode { get; set; }

        public string LoanPeriodCode { get; set; }

        public string LoanMoney { get; set; }
        public string LocSignCode { get; set; }

        public bool IsInsurrance { get; set; }

        public int Status { get; set; }
        public string CatNumber { get; set; }
        public string ComName { get; set; }
        public string CatName { get; set; }

        public int Sex { get; set; }
        public string Addr { get; set; }
        public decimal CustIncome { get; set; }


    }
}
