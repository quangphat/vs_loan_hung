using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class BaseSqlEntity
    {
        public DateTime CreatedTime
        {
            get { return DateTime.Now; }
        }
        public int CreatedBy { get; set; }
        public DateTime UpdatedTime
        {
            get { return DateTime.Now; }
        }
        public int UpdatedBy { get; set; }
    }
}
