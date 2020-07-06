using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class ResponseModel
    {
        public List<string> Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string SystemMessage { get; set; }
    }
    public class ExternalApiResponseModel<T>
    {
        public ExternalApiResponseModel(T data, HttpStatusCode code, string message)
        {
            Data = data;
            Code = code;
            ExceptionMessage = message;
        }
        public T Data { get; set; }
        public HttpStatusCode Code { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
