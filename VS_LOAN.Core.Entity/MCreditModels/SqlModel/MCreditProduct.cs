using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{

    public class MCreditProduct
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string MaxLoanAmount { get; set; }

        public string MinLoanAmount { get; set; }

        public string MaxTenor { get; set; }

        public string MinTenor { get; set; }

        public string IsCheckCat { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string McId { get; set; }

    }
}
