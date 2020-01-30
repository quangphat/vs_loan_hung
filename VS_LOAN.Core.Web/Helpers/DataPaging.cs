using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Web.Helpers
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

        [DataMember]
        public long TotalRecord { get; set; }
    }
    public class DataPaging<T> : DataPaging where T : class
    {
        [DataMember]
        public T Datas { get; set; }
    }
}