using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseController : Controller
    {
        CurrentProcess _process;
        public BaseController(CurrentProcess currentProcess)
        {
            _process = currentProcess;
            if(GlobalData.User!=null)
            {
                _process.UserName = GlobalData.User.UserName;
                _process.UserId = GlobalData.User.IDUser;
            }
            
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = LanguageMang.GetDefaultLanguage();
                }
            }
            new LanguageMang().SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
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
    
    public class JsonnResponseModel

    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}