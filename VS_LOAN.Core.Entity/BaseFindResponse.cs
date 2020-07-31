using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class BaseFindResponse<T> where T : class
    {
        public BaseFindResponse()
        {
            Results = new List<T>();
        }

        public List<T> Results { get; set; }

        public int TotalRecords { get; set; }
        public int? MinId { get; set; }
        public int? MaxId { get; set; }

    }
}
