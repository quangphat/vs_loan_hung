using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class BaseFindRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;


        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > 1 ? value : 1; }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value > 1 ? value : 1; }
        }

        public int TotalPages { get; set; }

        public string Sort { get; set; }

        public int? MinId { get; set; }
        public int? MaxId { get; set; }
    }
}
