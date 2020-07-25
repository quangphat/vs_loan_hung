using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class BaseResponse<T>
    {
        public BaseResponse(string error, T data)
        {
            this.Error = error;
            this.Data = data;
        }
        public string Error { get; set; }
        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrWhiteSpace(Error);
            }
        }
        public T Data { get; set; }
    }
}
