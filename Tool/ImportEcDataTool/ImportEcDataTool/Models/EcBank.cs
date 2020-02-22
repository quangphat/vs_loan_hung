using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcBank
    {
        public int Id { get; set; }
        public string RefIndividual { get; set; }
        public int BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BankProvince { get; set; }

    }
}
