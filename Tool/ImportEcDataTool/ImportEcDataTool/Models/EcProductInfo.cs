using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcDataTool.Models
{
    public class EcProductInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description_Vi { get; set; }
        public string Description_En { get; set; }
        public string OccupationCode { get; set; }
        public string MinLoanAmountStr { get; set; }
        public decimal MinLoanAmount
        {
            get
            {
                return Convert.ToDecimal(MinLoanAmountStr.Replace(',', '.'));
            }
        }
        public string MaxLoanAmountStr { get; set; }
        public decimal MaxLoanAmount
        {
            get
            {
                return Convert.ToDecimal(MaxLoanAmountStr.Replace(',', '.'));
            }
        }
        public int MinTenor { get; set; }
        public int MaxTenor { get; set; }
    }
}
