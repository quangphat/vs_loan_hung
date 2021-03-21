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

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult TaoMoi()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }


        public async Task<JsonResult> LayDSNhomCha(bool isAddAll = false)
        {
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
        public ActionResult ThemMoi(string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {

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
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        public async Task<JsonResult> GetEmployeesByGroupId(int groupId, bool isLeader = false)
        {
            var results = await _rpEmployee.GetEmployeesByGroupId(groupId, isLeader);
            return ToJsonResponse(true, null, results);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<JsonResult> LayDSToNhomCon(int maNhomCha)
        {

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

            ViewBag.model = await _rpGroup.LayTheoMaAsync(id);
            return View();
        }

        public async Task<JsonResult> GetMember(int groupId)
        {
            var result = await _rpGroup.LayDSThanhVienNhomAsync(groupId, GlobalData.User.IDUser);
            return ToJsonResponse(true, null, result);
        }
      

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> ChiTiet(int id)
        {
            ViewBag.formindex = LstRole["QLToNhom"]._formindex;

            ViewBag.model = await _rpGroup.LayChiTietTheoMaAsync(id);
            return View();
        }


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult LuuSua(int maNhom, string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
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

        public JsonResult LayDSChiTietThanhVien(int maNhom)
        {

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
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult LuuCauHinh(int maNhanVien, List<int> lstIDNhom)
        {

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


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public async Task<ActionResult> Xoa(int id)
        {
            if (id <= 0)
            {
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            }
            var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            if (!isAdmin)
            {
                return ToJsonResponse(false, "Bạn không đủ quyền để xóa nhân viên");
            }
            var result = await _rpGroup.Delete(id);
            return ToJsonResponse(result);
        }
    }
}
