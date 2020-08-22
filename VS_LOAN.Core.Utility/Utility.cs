using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    public static class Utility
    {
        public const string CurrentCyFormat = "{0:#,###.##}";
        public static string FormatCurrentCy(decimal value)
        {
            return String.Format(CurrentCyFormat, value);
        }
    }
}
