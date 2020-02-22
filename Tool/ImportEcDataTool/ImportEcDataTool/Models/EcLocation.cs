using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcLocation
    {
        public int Id { get; set; }
        public int WardCode { get; set; }
        public string WardName { get; set; }
        public int DistrictCode {get;set;}
        public string DistrictName { get; set; }
        public int ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }
}
