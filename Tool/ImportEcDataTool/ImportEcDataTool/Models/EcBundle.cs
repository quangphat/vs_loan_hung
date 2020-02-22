using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcBundle
    {
        public int Id { get; set; }
        public string DocType { get; set; }
        public string Description_Vi { get; set; }
        public string Description_En { get; set; }
        public string RefBundleCode { get; set; }
        public string BundleName { get; set; }
        public string RefCodeId { get; set; }
    }
}
