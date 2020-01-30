using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly CurrentProcess _process;
        public BaseController(CurrentProcess currentProcess)
        {
            _process = currentProcess;
        }
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new DefaultContractResolver()
        };
        public JsonResult ToResponseV2<T>(T data)
        {
            var model = new ResponseJsonModel<T>();
            if (!checkHasError(model))
                model.data = data;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ToResponse(bool success = true, string message = null, object data = null)
        {
            string code = string.Empty;
            if(success)
            {

                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            return Json(new { data, success, code }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult ToJsonResponse(bool success = true, string message = null, object data = null)
        {
            string code = string.Empty;
            if (success)
            {

                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            return Json(new { data, success, code }, JsonRequestBehavior.AllowGet);
        }
        protected bool checkHasError(ResponseJsonModel model)
        {
            var hasError = _process.HasError;
            if(hasError)
            {
                model.code = _process.Errors[0].Message;
            }
            model.success = !hasError;
            return hasError;
        }
    }
    public class BaseApiController : ApiController
    {
        protected JsonResponseModel ToResponse(bool success = true, string message = null)
        {
            return new JsonResponseModel { Success = success, Message = message };
        }
    }
    public class JsonResponseModel

    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}