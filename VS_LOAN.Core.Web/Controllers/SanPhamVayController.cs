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
using VS_LOAN.Core.Web.Helpers.DataTables;

namespace VS_LOAN.Core.Web.Controllers
{
    public class SanPhamVayController : LoanController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("QuanLySanPham", new ActionInfo() { _formindex = IndexMenu.M_4_1, _href = "SanPhamVay/QuanLySanPham", _mangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay } });
                _lstRole.Add("Import", new ActionInfo() { _formindex = IndexMenu.M_4_2, _href = "SanPhamVay/Import", _mangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay } });
                return _lstRole;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay })]
        public ActionResult QuanLySanPham()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDS(string ngay)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            List<ThongTinSanPhamVayModel> rs = new List<ThongTinSanPhamVayModel>();
            try
            {
                DateTime dtDate = DateTime.MinValue;
                if (ngay != "")
                    dtDate = DateTimeFormat.ConvertddMMyyyyToDateTime(ngay);
                rs = new SanPhamBLL().LayThongTinSanPhamByID(3, dtDate);
                if (rs == null)
                    rs = new List<ThongTinSanPhamVayModel>();
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
        public ActionResult ThemMoi(string ma, string ngay)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {
                if (new SanPhamBLL().Trung(ma) == true)
                {
                    message.Result = false;
                    message.ErrorMessage = "Mã sản phẩm bị trùng!";
                }
                else
                {
                    DateTime dtDate = DateTime.MinValue;
                    try
                    {
                        if (ngay != "")
                            dtDate = DateTimeFormat.ConvertddMMyyyyToDateTime(ngay);
                    }
                    catch (Exception)
                    {
                        message.Result = false;
                        message.ErrorMessage = "Ngày không đúng định dạng!";
                    }

                    int result = 0;
                    SanPhamVayModel sp = new SanPhamVayModel();
                    sp.Ma = ma;
                    sp.Ten = ma;
                    sp.MaNguoiTao = GlobalData.User.IDUser;
                    sp.NgayTao = dtDate;
                    sp.MaDoiTac = 3;
                    sp.Loai = 1;
                    result = new SanPhamBLL().Them(sp);
                    if (result > 0)
                    {
                        message.Result = true;
                        message.ErrorMessage = Resources.Global.Message_Succ;
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

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay })]
        public ActionResult Xoa(int id)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {

                bool result = false;
                result = new SanPhamBLL().Xoa(id);
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

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay })]
        public ActionResult Import()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        public ActionResult AjaxHandler(jQueryDataTableParamModel param, List<SanPhamModel> lstSanPham)
        {
            if (lstSanPham == null)
                lstSanPham = new List<SanPhamModel>();
            var displayedCompanies = lstSanPham
                               .Skip(param.iDisplayStart)
                               .Take(param.iDisplayLength);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = lstSanPham.Count(),
                iTotalDisplayRecords = lstSanPham.Count(),
                aaData = displayedCompanies
            },
                                JsonRequestBehavior.AllowGet);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLSanPhamVay })]
        public ActionResult LuuImportSPV(List<SanPhamModel> lstSanPham, string ngay)
        {
            RMessage message = new RMessage() { Result = false, ErrorMessage = "Thao tác không thành công" };
            try
            {
                bool result = true;
                if(lstSanPham!=null)
                {
                    foreach(var item in lstSanPham)
                    {
                        if (new SanPhamBLL().Trung(item.Ten) == true)
                        {
                            item.TrangThai = 2;// Trùng
                        }
                        else
                        {
                            DateTime dtDate = DateTime.MinValue;
                            try
                            {
                                if (ngay != "")
                                    dtDate = DateTimeFormat.ConvertddMMyyyyToDateTime(ngay);
                            }
                            catch (Exception)
                            {
                                message.Result = false;
                                message.ErrorMessage = "Ngày không đúng định dạng!";
                                result = false;
                            }
                            SanPhamVayModel sp = new SanPhamVayModel();
                            sp.Ma = item.Ten;
                            sp.Ten = item.Ten;
                            sp.MaNguoiTao = GlobalData.User.IDUser;
                            sp.NgayTao = dtDate;
                            sp.MaDoiTac = 3;
                            sp.Loai = 1;
                           
                            if (new SanPhamBLL().Them(sp) > 0)
                            {
                                item.TrangThai = 1;
                            }
                            else
                            {
                                item.TrangThai = 3;
                            }
                        }

                    }
                    if (result)
                    {
                        message.ErrorMessage = "Thao tác thành công";
                        message.Result = true;
                    }
                }
               
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = "Thao tác không thành công";
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message, DSSPham = lstSanPham });
        }
        public FileResult DownloadFile(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\ImportSanPham\\" + file);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = file;
            return response;
        }
    }
}
