﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.CheckDup
{
    public class CheckDupAddModel
    {
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public bool Gender { get; set; }
        public string Note { get; set; }
        public int PartnerId { get; set; }
        public int PartnerStatus { get; set; }
        public int ProvinceId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
    }
}