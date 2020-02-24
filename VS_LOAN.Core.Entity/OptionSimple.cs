using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class OptionSimple
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
    }
    public class StringOptionSimple
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
    }
    //public class OptionForEcProductType:StringOptionSimple
    //{
    //    public StringOptionEcProductType LoanCondition { get; set; }
    //}
    public class OptionEcProductType : StringOptionSimple
    {
        public int MinTenor { get; set; }
        public int MaxTenor { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }

}
