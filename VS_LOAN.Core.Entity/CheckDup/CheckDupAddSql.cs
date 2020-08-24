using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Entity.CheckDup
{
    public class CheckDupAddSql : SqlBaseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public DateTime BirthDay { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public string LastNote { get; set; }
        public int PartnerId { get; set; }
        public int PartnerStatus { get; set; }
        public int ProvinceId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public bool IsMatch
        {
            get
            {
                if (PartnerStatus == (int)CheckDupPartnerStatus.MatchCondition)
                    return true;
                return false;
            }
        }
    }
}
