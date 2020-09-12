using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class StringModel
    {
        public string Value { get; set; }
        public string Value2 { get; set; }
    }
    public class StringModel2:StringModel
    {
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
    }


    public class StringModel3 : StringModel
    {
        public int HosoId { get; set; }
        public string Content { get; set; }

    }
}
