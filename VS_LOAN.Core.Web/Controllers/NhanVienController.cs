
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VS_LOAN.Core.Web.Controllers
{
    public class NhanVienController : LoanController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
              
                return _lstRole;
            }

        }
       
        public ActionResult Login()
        {
            if (GlobalData.User != null)
                return RedirectToAction("Index", "Home");
            string userName = "" , password = "";
            if (Request.Cookies["userName"] != null)
                userName = Request.Cookies["userName"].Value;
            if (Request.Cookies["password"] != null)
                password = Request.Cookies["password"].Value;
            ViewBag.UserName = userName;
            ViewBag.Password = password;
            if (userName != "" && password != "")
                ViewBag.SaveAccount = true;
            else
                ViewBag.SaveAccount = false;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult ChangePass(string newPassword, string oldPassword, string confirmPassword)
        {
            
            string newUrl = string.Empty;
            try
            {
                if (oldPassword == null)
                    oldPassword = "";
                UserPMModel user = new UserPMBLL().GetUserByID(GlobalData.User.IDUser.ToString());
                if (user != null)
                {
                    if (oldPassword.Trim().Equals(string.Empty)&& !user.Password.Equals(string.Empty))
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_PassOld_Empty);
                        
                    }
                    else if (newPassword.Trim().Equals(string.Empty))
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_PassNew_Empty);

                    }
                    else if (confirmPassword.Trim().Equals(string.Empty))
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_PassComfirm_Empty);

                    }
                    else if (!newPassword.Trim().Equals(confirmPassword.Trim()))
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_PassNewConform);

                    }
                    else if (MD5.getMD5(oldPassword.Trim()) != user.Password.Trim()&& user.Password!=string.Empty)
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_Old);
                        
                    }
                    else
                    {
                        bool result = new UserPMBLL().ChangePass(user.IDUser.ToString(), MD5.getMD5(newPassword.Trim()));
                        if (result)
                        {

                            newUrl = Url.Action("UserProfile", "NhanVien");
                            return ToResponse(true, newUrl);
                        }
                        return ToResponse(false, string.Empty);
                    }
                }
                return ToResponse(false, string.Empty);

            }
            catch (Exception ex)
            {
                return ToResponse(false, ex.Message);
            }
           
        }
        public ActionResult DangNhap(string userName, string password, string rememberMe)
        {
           
            string newUrl = string.Empty;
            try
            {
                UserPMModel user = new UserPMBLL().DangNhap(userName, MD5.getMD5(password));
                if (user != null)
                {
                   
                    GlobalData.User = user;
                    GlobalData.User.UserType = (int)UserTypeEnum.Sale;
                    var isTeamLead = new NhomBLL().checkIsTeamLeadByUserId(user.IDUser);
                    var isAdmin = new NhomBLL().CheckIsAdmin(user.IDUser);
                    if(isAdmin)
                        GlobalData.User.UserType = (int)UserTypeEnum.Admin;
                    else if(isTeamLead)
                        GlobalData.User.UserType = (int)UserTypeEnum.Teamlead;
                    var cookieUserName = new HttpCookie("userName");
                    var cookiePassword = new HttpCookie("password");
                    if (rememberMe != null && rememberMe.ToLower().Equals("on"))
                    {
                        cookieUserName.Expires = DateTime.Now.AddDays(30);
                        cookiePassword.Expires = DateTime.Now.AddDays(30);

                        FormsAuthentication.SetAuthCookie(userName, true);
                    }
                    else
                    {
                        cookieUserName.Expires = DateTime.Now.AddDays(-1);
                        cookiePassword.Expires = DateTime.Now.AddDays(-1);
                        FormsAuthentication.SetAuthCookie(userName, false);
                    }
                    cookieUserName.Value = userName;
                    cookiePassword.Value = password;

                    Response.SetCookie(cookieUserName);
                    Response.SetCookie(cookiePassword);
                    GlobalData.Rules = new GrantRightBLL().GetListRule(user.IDUser.ToString());

                    if (GlobalData.LinkBack != string.Empty)
                    {
                        newUrl = GlobalData.LinkBack;
                        GlobalData.LinkBack = string.Empty;
                    }
                    else
                    {
                        newUrl = "/Home/Index";
                    }
                    return ToResponse(true, newUrl);    
                }
                else
                {
                    return ToResponse(false, null, Resources.Global.NhanVien_Login_Message_DangNhap_Error_TDNORMK);
                   
                }
            }
            catch (BusinessException ex)                        
            {
                return ToResponse(false, ex.Message);          
            }
            
        }
        
        public ActionResult DangXuat()
        {
            RemoveAllSession();
            return Json(new { newurl = "/NhanVien/Login" });
        }
        
        private void RemoveAllSession()
        {
            Session.RemoveAll();

            var cookieUserName = new HttpCookie("userName");
            var cookiePassword = new HttpCookie("password");
            cookieUserName.Expires = DateTime.Now.AddDays(-1);
            cookiePassword.Expires = DateTime.Now.AddDays(-1);
            
            cookieUserName.Value = null;
            cookiePassword.Value = null;

            Response.SetCookie(cookieUserName);
            Response.SetCookie(cookiePassword);
            GlobalData.FirstAD = false;
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult UserProfile()
        {
            ViewBag.formindex = HomeController.LstRole["Index"]._formindex;

            ViewBag.ThongTin = new UserPMBLL().GetUserByID(GlobalData.User.IDUser.ToString());
            return View();
        }
        public JsonResult LayDSNhanVien()
        {
            List<UserPMModel> rs = new NhanVienBLL().LayDSNhanVien();
            if (rs == null)
                rs = new List<UserPMModel>();
            return Json(new { DSNhanVien = rs });
        }

    }
}
