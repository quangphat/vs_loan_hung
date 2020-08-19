using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Courier
{
    public class CourierIndexModel : CourierSql
    {
        public string AssignUser { get; set; }
        public string ProductName { get; set; }
        public string CreatedUser { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string StatusName { get; set; }
        public int TotalRecord { get; set; }
    }
}
