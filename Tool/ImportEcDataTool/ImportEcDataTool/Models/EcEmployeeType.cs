using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcEmployeeType
    {
        public int Id { get; set; }
        public string RefCode { get; set; }
        public string Description_Vi { get; set; }
        public string Description_En { get; set; }
        public string TypeDescription { get; set; }
    }
}
