using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        public readonly CurrentProcess _process;
        public BaseApiController(CurrentProcess currentProcess)
        {
            
            _process = currentProcess;
        }
        protected JsonnResponseModel ToResponse(bool success = true, string message = null)
        {
            return new JsonnResponseModel { Success = success, Message = message };
        }
    }
}