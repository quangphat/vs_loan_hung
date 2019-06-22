﻿using System;
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
    public class ToNhomController : LoanController
    {
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

        public JsonResult LayDSNhanVien()
        {
            List<UserPMModel> rs = new NhanVienBLL().LayDSNhanVien();
            if (rs == null)
                rs = new List<UserPMModel>();
            return Json(new { DSNhanVien = rs });
        }

        public JsonResult LayDSNhomCha()
        {
            List<NhomDropDownModel> rs = new NhomBLL().LayTatCa();
            if (rs == null)
                rs = new List<NhomDropDownModel>();
            return Json(new { DSNhom = rs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
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
                    if (nhom.MaNhomCha <= 0)
                    {
                        message.Result = true;
                        message.ErrorMessage = Resources.Global.Message_Succ;
                    }
                    else
                    {
                        var updateQuyenResult = false;
                        var nhomCha = new NhomBLL().LayTheoMa(nhom.MaNhomCha);
                        if(nhomCha!=null && nhomCha.MaNguoiQuanLy>0)
                            updateQuyenResult = updateNhanvienQuyen(nhomCha.MaNguoiQuanLy);
                        if(updateQuyenResult)
                        {
                            message.Result = true;
                            message.ErrorMessage = Resources.Global.Message_Succ;
                        }
                        else
                        {
                            message.Result = false;
                            message.ErrorMessage = "Không thể cập nhật quyền";
                        }
                    }

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
            if (Session["ToNhom_Sua_ID"] == null)
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
            if (Session["ToNhom_ChiTiet_ID"] == null)
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
                var currentGroup = new NhomBLL().LayTheoMa(maNhom);
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
                    if (nhom.MaNhomCha <= 0)
                    {
                        message.Result = true;
                        message.ErrorMessage = Resources.Global.Message_Succ;
                    }
                    else
                    {
                        if (currentGroup.MaNhomCha != nhom.MaNhomCha)
                        {
                            var updateOldResult = false;
                            var updateNewResult = false;
                            var nhomChaOld = new NhomBLL().LayTheoMa(currentGroup.MaNhomCha);
                            if(nhomChaOld!=null && nhomChaOld.MaNguoiQuanLy>0)
                                updateOldResult = updateNhanvienQuyen(nhomChaOld.MaNguoiQuanLy);
                            var nhomChaNew = new NhomBLL().LayTheoMa(nhom.MaNhomCha);
                            if (nhomChaNew != null && nhomChaNew.MaNguoiQuanLy > 0)
                                updateNewResult = updateNhanvienQuyen(nhomChaNew.MaNguoiQuanLy);
                            if(!updateNewResult || !updateOldResult)
                            {
                                message.Result = false;
                                message.ErrorMessage = "Không thể cập nhật quyền";
                            }
                            else
                            {
                                message.Result = true;
                                message.ErrorMessage = Resources.Global.Message_Succ;
                            }
                        }
                    }
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
        private bool updateNhanvienQuyen(int userId)
        {
            if (userId <= 0)
                return false;
            var lstNhom = new NhomBLL().LayDSNhomByNhanvienQuanly(userId);
            if (lstNhom == null || !lstNhom.Any())
            {
                new GrantRightBLL().DeleteNhanvienQuyen(userId);
                return true;
            }
            var exist = new GrantRightBLL().GetNhanvienQuyenByUserId(userId);
            if (exist != null)
            {
                new GrantRightBLL().UpdateNhanvienQuyen(new NhanvienQuyenModel { Ma_NV = userId, Quyen = "fffff" });
                return true;
            }
            else
            {
                new GrantRightBLL().InsertNhanvienQuyen(new NhanvienQuyenModel { Ma_NV = userId, Quyen = "fffff" });
                return true;
            }
        }

    }
}
