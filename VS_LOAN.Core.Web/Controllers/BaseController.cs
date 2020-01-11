using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseController : Controller
    {
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
        //public JsonResult ToJsonResponse(bool success, string message = null, object data = null)
        //{
        //    var result = new RMessage();
        //    if (success)
        //    {

        //        result.code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
        //    }
        //    else
        //    {
        //        result.code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
        //    }
        //    result.success = success;
        //    result.data = data;
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}
    }
    public class BaseApiController : ApiController
    {
        protected JsonnResponseModel ToResponse(bool success = true, string message = null)
        {
            return new JsonnResponseModel { Success = success, Message = message };
        }
    }
    public class JsonnResponseModel

    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}