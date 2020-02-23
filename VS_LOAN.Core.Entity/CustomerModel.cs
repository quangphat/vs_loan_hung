using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class CustomerModel : BaseSqlEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public bool Gender { get; set; }
        public string MatchCondition { get; set; }
        public string NotMatch { get; set; }
        public string LastNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssuePlaceCode { get; set; }
        public int PermanentProvinceCode { get; set; }
        public int PermanentDistrictCode { get; set; }
        public int PermanentWardCode { get; set; }
        public string PermanentAddress { get; set; }

    }
}
