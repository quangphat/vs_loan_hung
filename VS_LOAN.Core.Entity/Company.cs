using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Entity
{
    public class Company : SqlBaseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string LastNote { get; set; }
        public int PartnerId { get; set; }
        public int Status { get; set; }
        public string TaxNumber { get; set; }
        public int CatType { get; set; }
    }
}
