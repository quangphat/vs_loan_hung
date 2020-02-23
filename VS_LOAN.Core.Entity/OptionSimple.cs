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
}
