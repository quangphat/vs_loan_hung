using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Business.Infrastuctures
{
    public class DataPaging
    {
        public static DataPaging<T> Create<T>(T data, long totalRecords) where T : class
        {
            DataPaging<T> d = new DataPaging<T>
            {
                Datas = data,
                TotalRecord = totalRecords
            };
            return d;
        }

        public long TotalRecord { get; set; }
    }
    public class DataPaging<T> : DataPaging where T : class
    {
        public T Datas { get; set; }
    }
}
