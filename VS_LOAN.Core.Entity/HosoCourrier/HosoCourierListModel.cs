using System;
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
        public string ProvinceId { get; set; }
    }
}
