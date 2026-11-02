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
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class ToNhomController : LoanController
    {
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly IGroupRepository _rpGroup;
        public ToNhomController(IEmployeeRepository employeeRepository, IGroupRepository groupRepository) : base()
        {
            _rpEmployee = employeeRepository;
            _rpGroup = groupRepository;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("TaoMoi", new ActionInfo() { _formindex = IndexMenu.M_3_1, _href = "ToNhom/TaoMoi", _mangChucNang = new int[] { (int)QuyenIndex.QLToNhom } });
                _lstRole.Add("QLToNhom", new ActionInfo() { _formindex = IndexMenu.M_3_2, _href = "ToNhom/QLToNhom", _mangChucNang = new int[] { (int)QuyenIndex.QLToNhom } });
                _lstRole.Add("CauHinhDuyet", new ActionInfo() { _formindex = IndexMenu.M_3_3, _href = "ToNhom/CauHinhDuyet", _mangChucNang = new int[] { (int)QuyenIndex.QLToNhom } });
                return _lstRole;
            }
        }

        private async Task<bool> IsValidGroupAssignment(int managerId, List<int> memberIds)
        {
            if (managerId <= 0 || memberIds == null || memberIds.Count == 0)
                return false;

            var employees = await _rpEmployee.GetAllEmployee(GlobalData.User.OrgId);
            if (employees == null)
                return false;

            var employeeIds = new HashSet<int>(employees.Select(employee => employee.Id));
            return employeeIds.Contains(managerId) && memberIds.All(employeeIds.Contains);
        }

        private async Task<bool> IsValidParentGroup(int groupId, int parentGroupId)
        {
            if (parentGroupId == 0)
                return true;

            var groups = await _rpGroup.GetAll(GlobalData.User.IDUser);
            var parent = groups == null ? null : groups.FirstOrDefault(group => group.ID == parentGroupId);
            if (parent == null || parentGroupId == groupId)
                return false;

            if (groupId <= 0)
                return true;

            var parentPath = "." + (parent.ChuoiMaCha ?? string.Empty) + ".";
            return !parentPath.Contains("." + groupId + ".");
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult TaoMoi()
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return RedirectToAction("Index", "NoAuthorities");
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom, (int)QuyenIndex.CauHinhDuyet })]
        public async Task<JsonResult> LayDSNhomCha(bool isAddAll = false)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Chỉ Admin được cấu hình nhóm");
            List<NhomDropDownModel> rs = await _rpGroup.GetAll(GlobalData.User.IDUser);

            if (rs == null)
                rs = new List<NhomDropDownModel>();
            if (isAddAll)
            {
                rs.Add(new NhomDropDownModel { ID = 0, Ten = "Tất cả" });
            }
            return ToJsonResponse(true, null, rs);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> ThemMoi(string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToResponse(false, "Chỉ Admin được cấu hình nhóm");
            if (!await IsValidGroupAssignment(maNguoiQuanLy, lstThanhVien)
                || !await IsValidParentGroup(0, maNhomCha))
                return ToResponse(false, "Nhân viên hoặc nhóm cha không hợp lệ");

            try
            {

                int result = 0;
                NhomModel nhom = new NhomModel();
                nhom.MaNguoiQL = maNguoiQuanLy;
                nhom.MaNhomCha = maNhomCha;
                if (maNhomCha != 0)
                {
                    var parentCode = _rpGroup.LayChuoiMaCha(maNhomCha);
                    nhom.ChuoiMaCha = parentCode + "." + maNhomCha;
                }

                else
                    nhom.ChuoiMaCha = "0";
                nhom.Ten = ten;
                nhom.TenNgan = tenNgan;
                result = _rpGroup.Them(nhom, lstThanhVien, GlobalData.User.IDUser);
                if (result > 0)
                {
                    return ToResponse(true, null, result);

                }
                return ToResponse(false, "Không thành công", 0);
            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message, 0);
            }

        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult QLToNhom()
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return RedirectToAction("Index", "NoAuthorities");
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<JsonResult> GetEmployeesByGroupId(int groupId, bool isLeader = false)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Chỉ Admin được cấu hình nhóm");
            var results = await _rpEmployee.GetEmployeesByGroupId(groupId, isLeader);
            return ToJsonResponse(true, null, results);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<JsonResult> LayDSToNhomCon(int maNhomCha)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Chỉ Admin được xem danh sách nhóm");

            RMessage message = new RMessage { code = Resources.Global.Message_Succ, success = true };
            List<ThongTinToNhomModel> rs = new List<ThongTinToNhomModel>();
            try
            {
                rs = await _rpGroup.LayDSNhomConAsync(maNhomCha, GlobalData.User.IDUser);
                if (rs == null)
                    rs = new List<ThongTinToNhomModel>();
            }
            catch (BusinessException ex)
            {
                message.success = false;
                message.code = ex.Message;
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> Sua(int id)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return RedirectToAction("Index", "NoAuthorities");

            ViewBag.model = await _rpGroup.LayTheoMaAsync(id);
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<JsonResult> GetMember(int groupId)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Chỉ Admin được xem thành viên nhóm");
            var result = await _rpGroup.LayDSThanhVienNhomAsync(groupId, GlobalData.User.IDUser);
            return ToJsonResponse(true, null, result);
        }
      

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> ChiTiet(int id)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return RedirectToAction("Index", "NoAuthorities");
            ViewBag.formindex = LstRole["QLToNhom"]._formindex;

            ViewBag.model = await _rpGroup.LayChiTietTheoMaAsync(id);
            return View();
        }


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> LuuSua(int maNhom, string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToResponse(false, "Chỉ Admin được cấu hình nhóm");
            if (!await IsValidGroupAssignment(maNguoiQuanLy, lstThanhVien)
                || !await IsValidParentGroup(maNhom, maNhomCha))
                return ToResponse(false, "Nhân viên hoặc nhóm cha không hợp lệ");
            try
            {

                bool result = false;
                NhomModel nhom = new NhomModel();
                nhom.ID = maNhom;
                nhom.MaNguoiQL = maNguoiQuanLy;
                nhom.MaNhomCha = maNhomCha;
                if (maNhomCha != 0)
                {
                    var parentCode = _rpGroup.LayChuoiMaCha(maNhomCha);
                    nhom.ChuoiMaCha = parentCode + "." + maNhomCha;
                }

                else
                    nhom.ChuoiMaCha = "0";
                nhom.Ten = ten;
                nhom.TenNgan = tenNgan;
                result = _rpGroup.Sua(nhom, lstThanhVien);
                if (result)
                {
                    return ToResponse(true, null, result);
                }
                return ToResponse(false);
            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message, null);
            }

        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public JsonResult LayDSChiTietThanhVien(int maNhom)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Chỉ Admin được xem thành viên nhóm");

            RMessage message = new RMessage { code = Resources.Global.Message_Error, success = false };
            List<ThongTinNhanVienModel> rs = new List<ThongTinNhanVienModel>();
            try
            {
                rs = _rpGroup.LayDSChiTietThanhVienNhom(maNhom);
                if (rs == null)
                    rs = new List<ThongTinNhanVienModel>();
            }
            catch (BusinessException ex)
            {
                message.success = false;
                message.code = ex.Message;
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.CauHinhDuyet })]
        public ActionResult CauHinhDuyet()
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return RedirectToAction("Index", "NoAuthorities");
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.CauHinhDuyet })]
        public async Task<ActionResult> LuuCauHinh(int maNhanVien, List<int> lstIDNhom)
        {
            if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
                return ToResponse(false, "Chỉ Admin được cấu hình duyệt");
            var employees = await _rpEmployee.GetAllEmployee(GlobalData.User.OrgId);
            var groups = await _rpGroup.GetAll(GlobalData.User.IDUser);
            if (employees == null || !employees.Any(employee => employee.Id == maNhanVien)
                || groups == null || lstIDNhom == null || lstIDNhom.Count == 0
                || lstIDNhom.Any(groupId => !groups.Any(group => group.ID == groupId)))
                return ToResponse(false, "Nhân viên hoặc nhóm cấu hình không hợp lệ");

            try
            {
                bool result = _rpEmployee.CapNhat(maNhanVien, lstIDNhom);
                return ToResponse(result);
            }
            catch (BusinessException ex)
            {
                return ToResponse(false, null, ex.Message);
            }
        }
    }
}
