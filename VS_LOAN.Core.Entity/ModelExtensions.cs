using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public static class ModelExtensions
    {
        public static string GenEcRequestId(string prefix)
        {
            return $"{prefix}{DateTime.UtcNow.ToUnixTime()}";
        }
        public static long ToUnixTime(this DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalMilliseconds;
        }
    }

}
