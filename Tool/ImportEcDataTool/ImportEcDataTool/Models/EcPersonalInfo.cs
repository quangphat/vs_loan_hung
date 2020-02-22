using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcPersonalInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description_Vi { get; set; }
        public string Description_En { get; set; }
        public string Type { get; set; }
    }
}
