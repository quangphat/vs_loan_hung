using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.HosoCourrier
{
    public class CourierSearchRequestModel : BaseFindRequest
    {
        public string freeText { get; set; }
        public int provinceId { get; set; }
        public int courierId { get; set; }
        public string status { get; set; }
        public int groupId { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public string salecode { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}
