using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class PermissionGroupController : LoanController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("TaoMoi", new ActionInfo() { _formindex = IndexMenu.M_5_1, _href = "PermissionGroup/TaoMoi", _mangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen } });
                _lstRole.Add("DanhsachNhomQuyen", new ActionInfo() { _formindex = IndexMenu.M_5_2, _href = "PermissionGroup/DanhsachNhomQuyen", _mangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen } });
               
                return _lstRole;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen })]
        public ActionResult TaoMoi()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen })]
        public ActionResult ThemMoi(string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {

                int result = 0;
                NhomModel nhom = new NhomModel();
                nhom.MaNguoiQL = maNguoiQuanLy;
                nhom.MaNhomCha = maNhomCha;
                if (maNhomCha != 0)
                    nhom.ChuoiMaCha = new NhomBLL().LayChuoiMaCha(maNhomCha) + "." + maNhomCha;
                else
                    nhom.ChuoiMaCha = "0";
                nhom.Ten = ten;
                nhom.TenNgan = tenNgan;
                result = new NhomBLL().Them(nhom, lstThanhVien);
                if (result > 0)
                {
                    message.Result = true;
                    message.ErrorMessage = Resources.Global.Message_Succ;
                }
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult LayDSNhomCha()
        {
            List<NhomDropDownModel> rs = new NhomBLL().LayTatCa();
            if (rs == null)
                rs = new List<NhomDropDownModel>();
            return Json(new { DSNhom = rs });
        }



        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult QLToNhom()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public JsonResult LayDSToNhomCon(int maNhomCha)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            List<ThongTinToNhomModel> rs = new List<ThongTinToNhomModel>();
            try
            {
                rs = new NhomBLL().LayDSNhomCon(maNhomCha);
                if (rs == null)
                    rs = new List<ThongTinToNhomModel>();
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult Sua()
        {
            ViewBag.formindex = LstRole["QLToNhom"]._formindex;
            if(Session["ToNhom_Sua_ID"] == null)
                return RedirectToAction("QLToNhom");
            int idNhom = (int)Session["ToNhom_Sua_ID"];
            ViewBag.ThongTinNhom = new NhomBLL().LayTheoMa(idNhom);
            return View();
        }

        public ActionResult SuaToNhomByID(int id)
        {
            Session["ToNhom_Sua_ID"] = id;
            return RedirectToAction("Sua");
        }

        public JsonResult LayThongTinThanhVienSuaNhom(int maNhom)
        {
            List<NhanVienNhomDropDownModel> lstThanhVienNhom = new NhanVienNhomBLL().LayDSThanhVienNhom(maNhom);
            List<NhanVienNhomDropDownModel> lstKhongThanhVienNhom = new NhanVienNhomBLL().LayDSKhongThanhVienNhom(maNhom);
            return Json(new { DSThanhVien = lstThanhVienNhom, DSChuaThanhVien = lstKhongThanhVienNhom });
        }
        
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult ChiTiet()
        {
            ViewBag.formindex = LstRole["QLToNhom"]._formindex;
            if(Session["ToNhom_ChiTiet_ID"] == null)
                return RedirectToAction("QLToNhom");
            int idNhom = (int)Session["ToNhom_ChiTiet_ID"];
            ViewBag.ThongTinNhom = new NhomBLL().LayChiTietTheoMa(idNhom);
            return View();
        }

        public ActionResult XemToNhomByID(int id)
        {
            Session["ToNhom_ChiTiet_ID"] = id;
            return RedirectToAction("ChiTiet");
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult LuuSua(int maNhom, string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {

                bool result = false;
                NhomModel nhom = new NhomModel();
                nhom.ID = maNhom;
                nhom.MaNguoiQL = maNguoiQuanLy;
                nhom.MaNhomCha = maNhomCha;
                if (maNhomCha != 0)
                    nhom.ChuoiMaCha = new NhomBLL().LayChuoiMaCha(maNhomCha) + "." + maNhomCha;
                else
                    nhom.ChuoiMaCha = "0";
                nhom.Ten = ten;
                nhom.TenNgan = tenNgan;
                result = new NhomBLL().Sua(nhom, lstThanhVien);
                if (result)
                {
                    message.Result = true;
                    message.ErrorMessage = Resources.Global.Message_Succ;
                }
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDSChiTietThanhVien(int maNhom)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            List<ThongTinNhanVienModel> rs = new List<ThongTinNhanVienModel>();
            try
            {
                rs = new NhanVienNhomBLL().LayDSChiTietThanhVienNhom(maNhom);
                if (rs == null)
                    rs = new List<ThongTinNhanVienModel>();
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
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
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {
                bool result = new NhanVienConfigBLL().CapNhat(maNhanVien, lstIDNhom);
                if (result)
                {
                    message.Result = true;
                    message.ErrorMessage = Resources.Global.Message_Succ;
                }
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
