﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Entity.CheckDup
{
    public class CheckDupIndexModel : SqlBaseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public int PartnerStatus { get; set; }
        public string CICStatusName { get; set; }
        public string LastNote { get; set; }
        public string PartnerName { get; set; }
        public string PartnerStatusName { get; set; }
        public bool IsMatch { get; set; }
        public string ProvinceName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string MaVBF { get; set; }
        public string NameVBF { get; set; }
        public int TotalRecord { get; set; }
        public string MatchCondition { get; set; }
        public string NotMatchCondition { get; set; }
    }
}
