using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.CheckDup
{
    public class CheckDupIndexModel:SqlBaseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public string StatusName { get; set; }
        public bool Gender { get; set; }
        public string LastNote { get; set; }
        public string MatchCondition { get; set; }
        public string NotMatch { get; set; }
        public int PartnerId { get; set; }
        public int Status { get; set; }
        public bool IsMatch { get; set; }
        public int ProvinceId { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string MaVBF { get; set; }
        public string NameVBF { get; set; }
        public int TotalRecord { get; set; }
    }
}
