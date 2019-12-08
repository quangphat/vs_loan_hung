using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Employee;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class EmployeeController : BaseController
    {
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
            var bizEmployee = new EmployeeBusiness();
            var rs = await bizEmployee.GetRoleList();
            return Json(new { success = true, data = rs }, JsonRequestBehavior.AllowGet);
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
            var bzEmployee = new EmployeeBusiness();
            BusinessExtension.ProcessPaging(ref page, ref limit);
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            var totalRecord = await bzEmployee.Count(fromDate, toDate, roleId, freetext);
            var datas = await bzEmployee.Gets(fromDate, toDate, roleId, freetext, page, limit);
            var result = DataPaging.Create(datas, totalRecord);
            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.account = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> Create([FromBody] UserCreateModel entity)
        {
            if (entity == null)
            {
                return Json(new { success = false, code = "Dữ liệu không hợp lệ" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                return Json(new { success = false, code = "Tên đăng nhập không được để trống" }, JsonRequestBehavior.AllowGet);
                
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {

                return Json(new { success = false, code = "Mật khẩu không được để trống" }, JsonRequestBehavior.AllowGet);
                
            }
            if (entity.Password.Trim().Length < 8)
            {
                return Json(new { success = false, code = "Mật khẩu phải có ít nhất 8 ký tự" }, JsonRequestBehavior.AllowGet);
                
            }
            if (string.IsNullOrWhiteSpace(entity.PasswordConfirm))
            {
                return Json(new { success = false, code = "Mật khẩu xác thực không được để trống" }, JsonRequestBehavior.AllowGet);
                
            }
            if (entity.Password != entity.PasswordConfirm)
            {
                return Json(new { success = false, code = "Mật khẩu không khớp" }, JsonRequestBehavior.AllowGet);
                
            }
            //if (string.IsNullOrWhiteSpace(entity.Email))
            //{
            //    return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            //}
            if (!string.IsNullOrWhiteSpace(entity.Email) && !BusinessExtension.IsValidEmail(entity.Email, 50))
            {
                return Json(new { success = false, code = "Email không hợp lệ" }, JsonRequestBehavior.AllowGet);
                
            }
            if (entity.ProvinceId <= 0)
            {
                return Json(new { success = false, code = "Dữ liệu không hợp lệ" }, JsonRequestBehavior.AllowGet);
               
            }
            if (entity.DistrictId <= 0)
            {
                return Json(new { success = false, code = "Vui lòng chọn quận/huyện" }, JsonRequestBehavior.AllowGet);
                
            }
            var bizEmployee = new EmployeeBusiness();
            var existUserName = await bizEmployee.GetByUserName(entity.UserName.Trim(), 0);
            if (existUserName != null)
            {
                return Json(new { success = false, code = "Tên đăng nhập đã tồn tại" }, JsonRequestBehavior.AllowGet);
                
            }
            if (entity.WorkDateStr == null)
            {
                return Json(new { success = false, code = "Vui lòng chọn ngày vào làm" }, JsonRequestBehavior.AllowGet);
                
            }
            if (string.IsNullOrWhiteSpace(entity.WorkDateStr))
            {
                return Json(new { success = false, code = "Vui lòng chọn ngày vào làm" }, JsonRequestBehavior.AllowGet);
           
            }
            try
            {
                entity.WorkDate = DateTimeFormat.ConvertddMMyyyyToDateTime(entity.WorkDateStr);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, code = "Định dạng ngày tháng không hợp lệ" }, JsonRequestBehavior.AllowGet);
                
            }
            entity.UserName = entity.UserName.Trim();
            entity.Password = entity.Password.Trim();
            entity.Password = MD5.getMD5(entity.Password);
            var result = await bizEmployee.Create(entity);
            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            
        }
        public async Task<ActionResult> Edit(int id)
        {
            var bzEmployee = new EmployeeBusiness();
            var employee = await bzEmployee.GetById(id);
            ViewBag.employee = employee;
            ViewBag.account = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> Update([FromBody] EmployeeEditModel model)
        {

            if (model == null || model.Id <=0)
            {
                return Json(new { success = false, code = "Dữ liệu không hợp lệ" }, JsonRequestBehavior.AllowGet);
                
            }
            var bzEmployee = new EmployeeBusiness();
            if (string.IsNullOrWhiteSpace(model.WorkDateStr))
            {
                return Json(new { success = false, code = "Vui lòng chọn ngày vào làm" }, JsonRequestBehavior.AllowGet);
                
            }
            try
            {
                model.WorkDate = DateTimeFormat.ConvertddMMyyyyToDateTime(model.WorkDateStr);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, code = "Định dạng ngày tháng không hợp lệ" }, JsonRequestBehavior.AllowGet);
                
            }
            model.UpdatedBy = GlobalData.User.IDUser;
            var result = await bzEmployee.Update(model);
            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetPartner(int customerId)
        {
            var bizCustomer = new CustomerBLL();
            var bizPartner = new PartnerBLL();
            var customerCheck = bizCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = bizPartner.GetListForCheckCustomerDuplicate();
            if (partners == null)
                return Json(new { success = true, data = new List<OptionSimple>() }, JsonRequestBehavior.AllowGet);

           
            foreach (var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }

            return Json(new { success = true, data =partners }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetNotes(int customerId)
        {
            var bizCustomer = new CustomerBLL();
            var datas = bizCustomer.GetNoteByCustomerId(customerId);
            return Json(new { success = true, data = datas }, JsonRequestBehavior.AllowGet);

        }
    }
}
