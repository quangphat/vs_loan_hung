﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult ToResponse(bool success,  string message = null, int id = 0)
        {
            var result = new RMessage();
            if(success)
            {

                result.ErrorMessage = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                result.ErrorMessage = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            result.Result = success;
            result.ReturnId = id;
            return Json(new { Message = result }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult ToJsonResponse(bool success, string message = null, int id = 0)
        {
            var result = new RMessage();
            if (success)
            {

                result.ErrorMessage = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                result.ErrorMessage = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            result.Result = success;
            result.ReturnId = id;
            return Json(new { Message = result }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ToJsonResponse(object data, bool success = true)
        {
            return Json(new { datas = data, success }, JsonRequestBehavior.AllowGet);
        }
    }
}