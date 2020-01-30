using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public class ResponseJsonModel
    {
        public bool success { get; set; }
        public string code { get; set; }
    }
    public class ResponseJsonModel<T> : ResponseJsonModel
    {
        public T data { get; set; }

    }
}
