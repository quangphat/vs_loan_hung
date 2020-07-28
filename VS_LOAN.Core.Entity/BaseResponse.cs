using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class BaseResponse<T>
    {
        public BaseResponse(string message, T data, bool success)
        {
            this.Message = message;
            this.Data = data;
            this.IsSuccess = success;
        }
        public BaseResponse(T data)
        {
            this.Message = string.Empty;
            this.Data = data;
            this.IsSuccess = true;
        }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
    }
}
