using MCreditService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Employee;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Repository.Interfaces;
using System.Web;
using VS_LOAN.Core.Utility.Exceptions;
using System.Web.Security;

namespace VS_LOAN.Core.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        protected readonly MCreditService.Interfaces.IMCreditService _svMCredit;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly ICustomerRepository _rpCustomer;
        public EmployeeController(
            ICustomerRepository customerRepository,
            IPartnerRepository partnerRepository,
            IEmployeeRepository employeeRepository)
        {
            _rpPartner = partnerRepository;
            _rpCustomer = customerRepository;
            _rpEmployee = employeeRepository;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_6_1, _href = "Employee/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_6, _href = "Employee/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }

        public async Task<JsonResult> GetRoles()
        {
            var rs = await _rpEmployee.GetRoleList();
            return ToJsonResponse(true, null, rs);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string workFromDate, string workToDate,
            string freetext = "",
            int roleId = 0,
            int page = 1, int limit = 10)
        {
            var fromDate = string.IsNullOrWhiteSpace(workFromDate) ? DateTime.Now.AddDays(-7) : DateTimeFormat.ConvertddMMyyyyToDateTime(workFromDate);
            var toDate = string.IsNullOrWhiteSpace(workToDate) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(workToDate);
            BusinessExtension.ProcessPaging(ref page, ref limit);
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            var datas = await _rpEmployee.Gets(fromDate, toDate, roleId, freetext, page, limit, GlobalData.User.OrgId);
            if (datas == null || !datas.Any())
            {
                return ToJsonResponse(true, null, DataPaging.Create(null as List<EmployeeViewModel>, 0));
            }
            var result = DataPaging.Create(datas, datas[0].TotalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = "";//LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.account = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> Create([FromBody] UserCreateModel entity)
        {
            var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            if (!isAdmin)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            if (entity == null)
            {
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                return ToJsonResponse(false, "Tên đăng nhập không được để trống");
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                return ToJsonResponse(false, "Mật khẩu không được để trống");
            }
            if (entity.Password.Trim().Length < 5)
            {
                return ToJsonResponse(false, "Mật khẩu phải có ít nhất 5 ký tự");
            }
            if (string.IsNullOrWhiteSpace(entity.PasswordConfirm))
            {
                return ToJsonResponse(false, "Mật khẩu xác thực không được để trống");
            }
            if (entity.Password != entity.PasswordConfirm)
            {
                return ToJsonResponse(false, "Mật khẩu không khớp");
            }
            //if (string.IsNullOrWhiteSpace(entity.Email))
            //{
            //    return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            //}
            if (!string.IsNullOrWhiteSpace(entity.Email) && !BusinessExtension.IsValidEmail(entity.Email, 50))
            {
                return ToJsonResponse(false, "Email không hợp lệ");
            }
            //if (entity.ProvinceId <= 0)
            //{
            //    return ToJsonResponse(false, "Vui lòng chọn tỉnh");
            //}
            //if (entity.DistrictId <= 0)
            //{
            //    return ToJsonResponse(false, "Vui lòng chọn quận/huyện");
            //}
            var existUserName = await _rpEmployee.GetByUserName(entity.UserName.Trim(), 0);
            if (existUserName != null)
            {
                return ToJsonResponse(false, "Tên đăng nhập đã tồn tại");
            }
            if (entity.WorkDateStr == null)
            {
                return ToJsonResponse(false, "Vui lòng chọn ngày vào làm");
            }
            if (string.IsNullOrWhiteSpace(entity.WorkDateStr))
            {
                return ToJsonResponse(false, "Vui lòng chọn ngày vào làm", null);
            }
            try
            {
                entity.WorkDate = DateTimeFormat.ConvertddMMyyyyToDateTime(entity.WorkDateStr);
            }
            catch (Exception ex)
            {
                return ToJsonResponse(false, "Định dạng ngày tháng không hợp lệ", null);
            }
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                return ToJsonResponse(false, "MÃ nhân viên không được để trống", 0);
            }
            var existCode = await _rpEmployee.GetByCode(entity.Code.Trim());
            if (existCode != null)
            {
                return ToJsonResponse(false, "Mã đã tồn tại", 0);
            }
            entity.UserName = entity.UserName.Trim();
            entity.Password = entity.Password.Trim();
            entity.Password = MD5.getMD5(entity.Password);
            var result = await _rpEmployee.Create(entity);
            return ToJsonResponse(true, null, result);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            if (!isAdmin)
            {
                return View();
            }
            var employee = await _rpEmployee.GetById(id);
            ViewBag.employee = employee;
            ViewBag.account = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> Update([FromBody] EmployeeEditModel model)
        {

            var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            if (!isAdmin)
            {
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            }
            if (model == null || model.Id <= 0)
            {
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(model.WorkDateStr))
            {
                return ToJsonResponse(false, "Vui lòng chọn ngày vào làm", null);
            }
            try
            {
                model.WorkDate = DateTimeFormat.ConvertddMMyyyyToDateTime(model.WorkDateStr);
            }
            catch (Exception ex)
            {
                return ToJsonResponse(false, "Định dạng ngày tháng không hợp lệ", null);
            }
            model.UpdatedBy = GlobalData.User.IDUser;
            var result = await _rpEmployee.Update(model);
            return ToJsonResponse(result);
        }
        public async Task<JsonResult> GetPartner(int customerId)
        {
            var customerCheck = await _rpCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = await _rpPartner.GetListForCheckCustomerDuplicateAsync();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            foreach (var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }

            return ToJsonResponse(true, null, partners);
        }
        public async Task<JsonResult> GetNotes(int customerId)
        {
            var datas = await _rpCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true, null, datas);
        }
        public async Task<JsonResult> GetUserByProvinceId(int provinceId)
        {

            var datas = await _rpEmployee.GetByProvinceId(provinceId);
            return ToJsonResponse(true, null, datas);
        }
        public async Task<JsonResult> GetUserByDistrictId(int districtId)
        {

            var datas = await _rpEmployee.GetByDistrictId(districtId);
            return ToJsonResponse(true, null, datas);
        }
        public async Task<JsonResult> ExcuteSql(SqlBody model)
        {

            var result = await _rpEmployee.QuerySQLAsync(model.Sql);
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> ResetPassword(ResetPasswordModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
                return ToJsonResponse(false);
            var result = await _rpEmployee.ResetPassord(model.UserName.Trim(), MD5.getMD5(model.Password.Trim()));
            return ToJsonResponse(true, "Thành công", result);
        }
        public async Task<JsonResult> GetAllEmployee()
        {
            var result = await _rpEmployee.GetAllEmployee(GlobalData.User.OrgId);
            return ToJsonResponse(true, data: result);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult UserProfile()
        {
            ViewBag.formindex = HomeController.LstRole["Index"]._formindex;

            ViewBag.ThongTin = _rpEmployee.GetById(GlobalData.User.IDUser);
            return View();
        }
        public async Task<ActionResult> DangNhap(string userName, string password, string rememberMe)
        {

            string newUrl = string.Empty;
            try
            {
                UserPMModel user = new UserPMBLL().DangNhap(userName, MD5.getMD5(password));
                //user = new UserPMModel {
                //    IDUser = 1,
                //    UserName = "t"
                //};
                if (user != null)
                {
                    GlobalData.User = user;
                    GlobalData.User.UserType = (int)UserTypeEnum.Sale;
                    var isTeamLead = new GroupRepository().checkIsTeamLeadByUserId(user.IDUser);
                    var isAdmin = await _rpEmployee.CheckIsAdmin(user.IDUser);
                    if (isAdmin)
                        GlobalData.User.UserType = (int)UserTypeEnum.Admin;
                    else if (isTeamLead)
                        GlobalData.User.UserType = (int)UserTypeEnum.Teamlead;
                    GlobalData.User.OrgId = user.OrgId;
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
                    return ToResponse(true, null, newUrl);
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
        public ActionResult Login()
        {
            if (GlobalData.User != null)
                return RedirectToAction("Index", "Home");
            string userName = "", password = "";
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
        public ActionResult DangXuat()
        {
            RemoveAllSession();
            return Json(new { newurl = "/Employee/Login" });
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
                    if (oldPassword.Trim().Equals(string.Empty) && !user.Password.Equals(string.Empty))
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
                    else if (MD5.getMD5(oldPassword.Trim()) != user.Password.Trim() && user.Password != string.Empty)
                    {
                        return ToResponse(false, null, Resources.Global.NhanVien_UserProfile_Password_Error_Old);

                    }
                    else
                    {
                        bool result = new UserPMBLL().ChangePass(user.IDUser.ToString(), MD5.getMD5(newPassword.Trim()));
                        if (result)
                        {

                            newUrl = Url.Action("UserProfile", "Employee");
                            return ToResponse(true, null, newUrl);
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
    }
}
