using VS_LOAN.Core.Business;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace VS_LOAN.Core.Web.Helpers
{
    public class CheckPermission : ActionFilterAttribute, IActionFilter
    {
        private int[] _mangChucNang = null;

        public int[] MangChucNang
        {
            get { return _mangChucNang; }
            set { _mangChucNang = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            List<string> lstQuyen = new List<string>();
            lstQuyen.Add((string)GlobalData.Rules);
            // Kiem tra dang nhap
            if (!GlobalData.IsLogin)
            {
                if (request.IsAjaxRequest())
                {
                    // response.StatusCode = 509;//SessionOut
                    response.StatusCode = 403;
                    filterContext.Result = new JsonResult() { Data = new { Url = "/NhanVien/Login", Flag = "NoLogin" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return;
                }
                else
                {
                    GlobalData.LinkBack = filterContext.HttpContext.Request.CurrentExecutionFilePath;
                    if (Constant.CHECK_AD == true&&GlobalData.FirstAD)
                    {
                        bool adRS = CheckAD();
                        if (adRS == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary { { "Controller", "NhanVien" }, { "Action", "Login" } });
                            return;
                        }
                        else
                            lstQuyen.Add((string)GlobalData.Rules);
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Controller", "NhanVien" }, { "Action", "Login" } });
                        return;
                    }
                }
            }
            // Kiem tra quyen
            if (_mangChucNang == null)
            {
                if (request.IsAjaxRequest())
                {
                    response.StatusCode = 403;
                    filterContext.Result = new JsonResult() { Data = new { Url = "/NoAuthorities/Index", Flag = "NoAuthor" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return;
                }
                else
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Controller", "NoAuthorities" }, { "Action", "Index" } });
            }
            if (_mangChucNang.Length == 1)
            {
                if (_mangChucNang[0] == (int)QuyenIndex.Public)
                    return;
            }
            int i;
            if (lstQuyen != null)
            {
                if (lstQuyen.Count > 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        for (i = 0; i < _mangChucNang.Length; i++)
                        {
                            string quyen = item.Trim();
                            int kt = (quyen.Length - (_mangChucNang[i] - 1) / 4) - 1;
                            if (kt >= 0 && kt < quyen.Length)
                            {
                                string maQuyen = quyen[kt].ToString();
                                string gt = Convert.ToString(Convert.ToInt32(maQuyen.ToString(), 16), 2).PadLeft(4, '0');

                                if (gt[gt.Length - (_mangChucNang[i] - 1) % 4 - 1] == '1')
                                    return;
                            }
                        }
                    }
                }
            }
            if (request.IsAjaxRequest())
            {
                response.StatusCode = 403;
                filterContext.Result = new JsonResult() { Data = new { Url = "/NoAuthorities/Index", Flag = "NoAuthor" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return;
            }
            else
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "Controller", "NoAuthorities" }, { "Action", "Index" } });
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        private bool CheckAD()
        {
            
            return false;
        }
    }    
}