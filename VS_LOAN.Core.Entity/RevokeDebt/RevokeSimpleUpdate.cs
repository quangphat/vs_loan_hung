﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.RevokeDebt
{
    public class RevokeSimpleUpdate
    {
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int AssigneeId { get; set; }
        public int Status { get; set; }
    }
}
