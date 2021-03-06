﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.HosoCourrier
{
    public class HosoCourierListModel: HosoCourier
    {
        public string AssignUser { get; set; }
        public string ProductName { get; set; }
        public string CreatedUser { get; set; }
    }
    public class HosoCourierViewModel : HosoCourier
    {
        public string AssignUser { get; set; }
        public string ProductName { get; set; }
        public string CreatedUser { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public int TotalRecord { get; set; }
        
    }
}
