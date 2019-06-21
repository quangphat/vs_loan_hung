using KPMG.Entity;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Common;
using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Web.Controllers
{
    public class UserPMController : LoanController
    {
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult GetUser(string userName)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            UserPMModel user = null;
            try
            {
                  user = new UserPMBLL().Get(userName);
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message, User = user }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
