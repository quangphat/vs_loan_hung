using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class ProfileSearchSql:BaseSqlEntity
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string IdNumber { get; set; }
        public string MoneyReceiveDate { get; set; }
        public string McId { get; set; }
        public int Status { get; set; }
        public string CreatedUser { get; set; }
        public string LastNote { get; set; }
        public string SaleName { get; set; }
        public string ProductName { get; set; }
        public string LoanPeriodName { get; set; }
        public string Phone { get; set; }
        public int TotalRecord { get; set; }
        public string StatusName { get; set; }
    }
}
