using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using VS_LOAN.Core.Utility.OfficeOpenXML;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class QuanLyHoSoController : LoanController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("DanhSachHoSo", new ActionInfo() { _formindex = IndexMenu.M_2_2, _href = "QuanLyHoSo/DanhSachHoSo", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult DanhSachHoSo()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            List<NhomDropDownModel> dsNhom = new NhomBLL().LayDSCuaNhanVien(GlobalData.User.IDUser);
            if (dsNhom == null)
                dsNhom = new List<NhomDropDownModel>();
            ViewBag.DSNhom = dsNhom;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult TimHSQL(int maNhom, int maThanhVien, string fromDate, string toDate, string maHS, string cmnd, int loaiNgay)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            List<HoSoQuanLyModel> rs = new List<HoSoQuanLyModel>();
            try
            {
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                message.Result = true;
                string trangthai = "";
                trangthai += ((int)TrangThaiHoSo.TuChoi).ToString() + "," + ((int)TrangThaiHoSo.NhapLieu).ToString() + "," + ((int)TrangThaiHoSo.ThamDinh).ToString() + "," + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + "," + ((int)TrangThaiHoSo.GiaiNgan).ToString() + "," + ((int)TrangThaiHoSo.Nhap).ToString();
                rs = new HoSoBLL().TimHoSoQuanLy(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, trangthai, loaiNgay);
                if (rs == null)
                    rs = new List<HoSoQuanLyModel>();
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
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSNhom()
        {
            List<NhomDropDownModel> rs = new NhomBLL().LayDSCuaNhanVien(GlobalData.User.IDUser);
            if (rs == null)
                rs = new List<NhomDropDownModel>();
            return Json(new { DSNhom = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSThanhVienNhom(int maNhom)
        {
            List<NhanVienNhomDropDownModel> rs = new List<NhanVienNhomDropDownModel>();
            if (maNhom > 0)
                rs = new NhanVienNhomBLL().LayDSThanhVienNhomCaCon(maNhom);
            else
            {
                // Lấy ds nhóm của nv quản lý
                List<NhomDropDownModel> lstNhom = new NhomBLL().LayDSCuaNhanVien(GlobalData.User.IDUser);
                if (lstNhom != null)
                {
                    for (int i = 0; i < lstNhom.Count; i++)
                    {
                        List<NhanVienNhomDropDownModel> lstThanhVien = new NhanVienNhomBLL().LayDSThanhVienNhom(lstNhom[i].ID);
                        if (lstThanhVien != null)
                        {
                            for (int j = 0; j < lstThanhVien.Count; j++)
                            {
                                if (rs.Find(x => x.ID == lstThanhVien[j].ID) == null)
                                    rs.Add(lstThanhVien[j]);
                            }
                        }
                    }
                }
            }
            if (rs == null)
                rs = new List<NhanVienNhomDropDownModel>();
            return Json(new { DSThanhVienNhom = rs });
        }

        //public JsonResult LayDSTrangThai()
        //{
        //    List<TrangThaiHoSoModel> rs = new TrangThaiHoSoBLL().LayDSTrangThai();
        //    if (rs == null)
        //        rs = new List<TrangThaiHoSoModel>();
        //    return Json(new { DSTrangThai = rs });
        //}
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult ChiTietHoSo()
        {
            ViewBag.formindex = LstRole["DanhSachHoSo"]._formindex;
            if (Session["QL_HoSoID"] == null)
                return RedirectToAction("Index");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["QL_HoSoID"]);
            new HoSoXemBLL().DaXem((int)Session["QL_HoSoID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = new DoiTacBLL().LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            ViewBag.LstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult XemHSByID(int id, string fromDate, string toDate, string mahs)
        {
            Session["QL_HoSoID"] = id;
            return RedirectToAction("ChiTietHoSo", new { fromDate = fromDate, toDate = toDate, mahs });
        }
        private bool AddGhichu(int hosoId, string ghiChu)
        {
            GhichuModel ghichu = new GhichuModel
            {
                UserId = GlobalData.User.IDUser,
                HosoId = hosoId,
                Noidung = ghiChu,
                CommentTime = DateTime.Now
            };
            new HoSoBLL().AddGhichu(ghichu);
            return true;
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult Save(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
           , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, bool baoHiem, int thoiHanVay, string soTienVay, int trangthai, string ghiChu)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            if (GlobalData.User.UserType == (int)UserTypeEnum.Sale
                || trangthai == (int)TrangThaiHoSo.Nhap)
            {
                trangthai = (int)TrangThaiHoSo.NhapLieu;
            }
            try
            {
                bool isCheck = true;
                if (hoten == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập họ tên";
                    isCheck = false;
                }
                else if (phone == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập số điện thoại";
                    isCheck = false;
                }
                else if (ngayNhanDon == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập ngày nhận đơn";
                    isCheck = false;
                }
                else if (cmnd == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập CMND";
                    isCheck = false;
                }
                else if (diaChi == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập địa chỉ";
                    isCheck = false;
                }
                else if (maKhuVuc == 0)
                {
                    message.ErrorMessage = "Vui lòng chọn quận/ huyện";
                    isCheck = false;
                }
                else if (sanPhamVay == 0)
                {
                    message.ErrorMessage = "Vui lòng chọn sản phẩm vay";
                    isCheck = false;
                }
                else if (soTienVay == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập số tiền vay";
                    isCheck = false;
                }
                if (trangthai <= 0)
                {
                    message.ErrorMessage = "Vui lòng chọn trạng thái";
                    isCheck = false;
                }
                if (!string.IsNullOrWhiteSpace(ghiChu) && ghiChu.Length > 200)
                {
                    message.ErrorMessage = "Nội dung ghi chú không được nhiều hơn 200 ký tự";
                    isCheck = false;
                }
                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["QL_LstFileHoSo"];
                List<LoaiTaiLieuModel> lstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
                lstLoaiTaiLieu.RemoveAll(x => x.BatBuoc == 0);
                if (lstLoaiTaiLieu != null)
                {
                    foreach (var item in lstLoaiTaiLieu)
                    {
                        var iFind = lstTaiLieu.Find(x => x.MaLoai == item.ID);
                        if (iFind == null)
                        {
                            message.ErrorMessage = "Vui lòng dính kèm \"" + item.Ten.ToUpper() + "\"";
                            isCheck = false;
                        }
                    }
                }
                if (isCheck)
                {
                    HoSoModel hs = new HoSoModel();
                    hs.ID = (int)Session["QL_HoSoID"];
                    hs.TenKhachHang = hoten;
                    hs.SDT = phone;
                    hs.SDT2 = phone2;
                    if (ngayNhanDon != string.Empty)
                    {
                        hs.NgayNhanDon = DateTimeFormat.ConvertddMMyyyyToDateTime(ngayNhanDon);
                    }
                    hs.HoSoCuaAi = hoSoCuaAi;
                    hs.MaNguoiCapNhat = GlobalData.User.IDUser;
                    hs.NgayCapNhat = DateTime.Now;
                    hs.CMND = cmnd;
                    hs.GioiTinh = gioiTinh;
                    hs.MaKhuVuc = maKhuVuc;
                    hs.DiaChi = diaChi;
                    hs.CourierCode = courier;
                    hs.SanPhamVay = sanPhamVay;
                    hs.TenCuaHang = tenCuaHang;
                    hs.CoBaoHiem = baoHiem ? 1 : 0;
                    hs.MaTrangThai = trangthai;
                    hs.HanVay = thoiHanVay;
                    if (soTienVay == string.Empty)
                        soTienVay = "0";
                    hs.SoTienVay = Convert.ToDecimal(soTienVay);
                    //hs.MaTrangThai = (int)TrangThaiHoSo.NhapLieu;
                    hs.MaKetQua = (int)KetQuaHoSo.Trong;
                    int result = 0;
                    if (hs.ID > 0)
                    {
                        bool isCheckMaSanPham = false;
                        //// chỉnh sửa
                        if (new HoSoBLL().CapNhatHoSo(hs, lstTaiLieu, ref isCheckMaSanPham))
                        {
                            new HoSoDuyetXemBLL().Them(hs.ID);
                            result = 1;
                        }
                        else
                        {
                            if (isCheckMaSanPham)
                                message.ErrorMessage = "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác";
                        }
                    }
                    AddGhichu(hs.ID, ghiChu);
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
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SaveDaft(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
           , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, int baoHiem, int thoiHanVay, string soTienVay, int trangthai, string ghiChu)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            if (trangthai <= 0 && GlobalData.User.UserType != (int)UserTypeEnum.Sale)
            {
                message.ErrorMessage = "Vui lòng nhập họ tên";
                message.Result = false;
                return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
            }
            try
            {

                if (hoten == string.Empty)
                {
                    message.ErrorMessage = "Vui lòng nhập họ tên";
                }
                else
                {
                    HoSoModel hs = new HoSoModel();
                    hs.ID = (int)Session["QL_HoSoID"];
                    hs.TenKhachHang = hoten;
                    hs.SDT = phone;
                    hs.SDT2 = phone2;
                    if (ngayNhanDon != string.Empty)
                    {
                        hs.NgayNhanDon = DateTimeFormat.ConvertddMMyyyyToDateTime(ngayNhanDon);
                    }
                    hs.HoSoCuaAi = hoSoCuaAi;
                    hs.MaNguoiTao = GlobalData.User.IDUser;
                    hs.NgayTao = DateTime.Now;
                    hs.CMND = cmnd;
                    hs.GioiTinh = gioiTinh;
                    hs.MaKhuVuc = maKhuVuc;
                    hs.DiaChi = diaChi;
                    hs.CourierCode = courier;
                    hs.SanPhamVay = sanPhamVay;
                    hs.TenCuaHang = tenCuaHang;
                    hs.CoBaoHiem = baoHiem;
                    hs.HanVay = thoiHanVay;
                    if (soTienVay == string.Empty)
                        soTienVay = "0";
                    hs.SoTienVay = Convert.ToDecimal(soTienVay);
                    hs.MaTrangThai = (int)TrangThaiHoSo.Nhap;
                    hs.MaKetQua = (int)KetQuaHoSo.Trong;
                    List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["QL_LstFileHoSo"];
                    int result = 0;
                    if (hs.ID > 0)
                    {
                        bool isCheckMaSanPham = false;
                        //// chỉnh sửa
                        if (new HoSoBLL().Luu(hs, lstTaiLieu, ref isCheckMaSanPham))
                            result = 1;
                        else
                        {
                            if (isCheckMaSanPham)
                                message.ErrorMessage = "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác";
                        }
                    }
                    else
                    {
                        bool isCheckMaSanPham = false;
                        result = new HoSoBLL().Them(hs, lstTaiLieu, ref isCheckMaSanPham);
                        if (result > 0)
                            Session["QL_LstFileHoSo"] = result;
                        else
                        {
                            if (isCheckMaSanPham)
                                message.ErrorMessage = "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác";
                        }
                    }
                    AddGhichu(hs.ID, ghiChu);
                    if (result > 0)
                    {

                        //Session["AddNewHoSoID"] = 0;
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
        public JsonResult UploadHoSo(string key)
        {
            string fileUrl = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string[] p = fileContent.ContentType.Split('/');
                        // get a stream
                        Stream stream = fileContent.InputStream;
                        // and optionally write the file to disk       
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileContent.FileName.Trim().Replace(" ", "_");
                        string root = System.IO.Path.Combine(Server.MapPath("~/Upload"), "HoSo");
                        string pathTemp = "";
                        if (!Directory.Exists(root))
                            Directory.CreateDirectory(root);
                        pathTemp = DateTime.Now.Year.ToString();
                        string pathYear = System.IO.Path.Combine(root, pathTemp);
                        if (!Directory.Exists(pathYear))
                            Directory.CreateDirectory(pathYear);
                        pathTemp += "/" + DateTime.Now.Month.ToString();
                        string pathMonth = System.IO.Path.Combine(root, pathTemp);
                        if (!Directory.Exists(pathMonth))
                            Directory.CreateDirectory(pathMonth);
                        pathTemp += "/" + DateTime.Now.Day.ToString();
                        string pathDay = System.IO.Path.Combine(root, pathTemp);
                        if (!Directory.Exists(pathDay))
                            Directory.CreateDirectory(pathDay);
                        string path = System.IO.Path.Combine(pathDay, fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                            fileStream.Close();
                            fileUrl = "/Upload/HoSo/" + pathTemp + "/" + fileName;
                        }
                        string deleteURL = Url.Action("Delete", "QuanLyHoSo") + "?key=" + key;
                        var _type = System.IO.Path.GetExtension(fileContent.FileName);
                        List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["QL_LstFileHoSo"];
                        var find = lstTaiLieu.Find(x => x.MaLoai.ToString().Equals(key.Trim()));
                        if (find != null)
                        {
                            find.LstFile[0].DuongDan = fileUrl;
                            find.LstFile[0].Ten = fileName;
                        }
                        else
                        {
                            TaiLieuModel taiLieu = new TaiLieuModel();
                            taiLieu.MaLoai = Convert.ToInt32(key.Trim());
                            Entity.Model.FileInfo _file = new Entity.Model.FileInfo();
                            _file.Ten = fileName;
                            _file.DuongDan = fileUrl;
                            taiLieu.LstFile.Add(_file);
                            lstTaiLieu.Add(taiLieu);
                        }
                        Session["QL_LstFileHoSo"] = lstTaiLieu;
                        if (_type.IndexOf("pdf") > 0)
                        {
                            var config = new
                            {
                                initialPreview = fileUrl,
                                initialPreviewConfig = new[] {
                                    new {
                                        caption = fileName,
                                        url = deleteURL,
                                        key =key,
                                        type="pdf",
                                        width ="120px"
                                        }
                                },
                                append = false
                            };
                            return Json(config);
                        }
                        else
                        {
                            var config = new
                            {
                                initialPreview = fileUrl,
                                initialPreviewConfig = new[] {
                                    new {
                                        caption = fileName,
                                        url = deleteURL,
                                        key =key,
                                        width ="120px"
                                    }
                                },
                                append = false
                            };
                            return Json(config);
                        }

                    }

                }
            }
            catch (Exception)
            {
                Session["QL_LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        public JsonResult Delete(string key)
        {
            string fileUrl = "";
            try
            {

                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["QL_LstFileHoSo"];
                lstTaiLieu.RemoveAll(x => x.MaLoai.ToString().Equals(key.ToString()));
                Session["QL_LstFileHoSo"] = lstTaiLieu;
            }
            catch (BusinessException ex)
            {

            }
            return Json(new { Result = fileUrl });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SuaHSByID(int id, string fromDate, string toDate, string mahs)
        {
            Session["QL_HoSoID"] = id;
            return RedirectToAction("SuaHoSo", new { fromDate = fromDate, toDate = toDate, mahs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SuaHoSo()
        {
            ViewBag.formindex = LstRole["DanhSachHoSo"]._formindex;
            if (Session["QL_HoSoID"] == null)
                return RedirectToAction("DanhSachHoSo");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["QL_HoSoID"]);
            new HoSoXemBLL().DaXem((int)Session["QL_HoSoID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = new DoiTacBLL().LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            Session["QL_LstFileHoSo"] = hoso.LstTaiLieu;
            ViewBag.LstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
            return View();
        }
        public JsonResult LayDSGhichu()
        {
            List<GhichuViewModel> rs = new HoSoBLL().LayDanhsachGhichu((int)Session["QL_HoSoID"]);
            if (rs == null)
                rs = new List<GhichuViewModel>();
            return Json(new { DSGhichu = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult DownloadReport(int maNhom, int maThanhVien, string fromDate, string toDate, string maHS, string cmnd, int loaiNgay)
        {
            var message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            string newUrl = string.Empty;
            try
            {
                List<HoSoQuanLyModel> rs = new List<HoSoQuanLyModel>();
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                message.Result = true;
                string trangthai = "";
                trangthai += ((int)TrangThaiHoSo.TuChoi).ToString() + "," + ((int)TrangThaiHoSo.NhapLieu).ToString() + "," + ((int)TrangThaiHoSo.ThamDinh).ToString() + "," + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + "," + ((int)TrangThaiHoSo.GiaiNgan).ToString() + "," + ((int)TrangThaiHoSo.Nhap).ToString();
                rs = new HoSoBLL().TimHoSoQuanLy(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, trangthai, loaiNgay);
                if (rs == null)
                    rs = new List<HoSoQuanLyModel>();
                string destDirectory = VS_LOAN.Core.Utility.Path.DownloadBill + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/";
                bool exists = System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                if (!exists)
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                string fileName = "Report-DSHS" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".xlsx";
                using (FileStream stream = new FileStream(Server.MapPath(destDirectory + fileName), FileMode.CreateNew))
                {
                    Byte[] info = System.IO.File.ReadAllBytes(Server.MapPath(VS_LOAN.Core.Utility.Path.ReportTemplate + "Report-DSHS.xlsx"));
                    stream.Write(info, 0, info.Length);
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        string nameSheet = "DSHS";
                        ExcelOOXML excelOOXML = new ExcelOOXML(archive);
                        int rowindex = 4;
                        if (rs != null)
                        {
                            excelOOXML.InsertRow(nameSheet, rowindex, rs.Count - 1, true);
                            for (int i = 0; i < rs.Count; i++)// dòng
                            {
                                excelOOXML.SetCellData(nameSheet, "A" + rowindex, (i + 1).ToString());
                                excelOOXML.SetCellData(nameSheet, "B" + rowindex, rs[i].MaHoSo.ToString());
                                excelOOXML.SetCellData(nameSheet, "C" + rowindex, rs[i].NgayTao.ToString("dd/MM/yyyy"));
                                excelOOXML.SetCellData(nameSheet, "D" + rowindex, rs[i].DoiTac);
                                excelOOXML.SetCellData(nameSheet, "E" + rowindex, rs[i].CMND);
                                excelOOXML.SetCellData(nameSheet, "F" + rowindex, rs[i].TenKH);
                                excelOOXML.SetCellData(nameSheet, "G" + rowindex, rs[i].TrangThaiHS);
                                excelOOXML.SetCellData(nameSheet, "H" + rowindex, rs[i].KetQuaHS);
                                excelOOXML.SetCellData(nameSheet, "I" + rowindex, rs[i].NgayCapNhat == DateTime.MinValue ? "" : rs[i].NgayCapNhat.ToString("dd/MM/yyyy"));
                                excelOOXML.SetCellData(nameSheet, "J" + rowindex, rs[i].MaNV);
                                excelOOXML.SetCellData(nameSheet, "K" + rowindex, rs[i].NhanVienBanHang);
                                excelOOXML.SetCellData(nameSheet, "L" + rowindex, rs[i].DoiNguBanHang);
                                excelOOXML.SetCellData(nameSheet, "M" + rowindex, rs[i].CoBaoHiem == true ? "N" : "Y");
                                excelOOXML.SetCellData(nameSheet, "N" + rowindex, rs[i].KhuVucText);
                                excelOOXML.SetCellData(nameSheet, "O" + rowindex, rs[i].GhiChu);
                                excelOOXML.SetCellData(nameSheet, "P" + rowindex, rs[i].MaNVLayHS);
                                rowindex++;
                            }
                        }
                        archive.Dispose();
                    }
                    stream.Dispose();
                }

                bool result = true;
                if (result)
                {
                    newUrl = "/File/GetFile?path=" + destDirectory + fileName;
                    message.Result = true;
                }
                message.ErrorMessage = Resources.Global.Message_Succ;
            }
            catch (BusinessException ex)
            {
                message.Result = false;
                message.MessageId = ex.getExceptionId();
                message.ErrorMessage = ex.Message;
                message.SystemMessage = ex.ToString();
            }
            return Json(new { Message = message, newurl = newUrl }, JsonRequestBehavior.AllowGet);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSTrangThai()
        {
            var isLimit = GlobalData.User.UserType == (int)UserTypeEnum.Teamlead;
            List<TrangThaiHoSoModel> rs = new TrangThaiHoSoBLL().LayDSTrangThai(isLimit);
            if (rs == null)
                rs = new List<TrangThaiHoSoModel>();
            rs.RemoveAll(x => x.ID == (int)TrangThaiHoSo.Nhap);
            if (GlobalData.User.UserType != (int)UserTypeEnum.Teamlead)
                rs.RemoveAll(x => x.ID == (int)TrangThaiHoSo.NhapLieu);
            return Json(new { DSTrangThai = rs });
        }
    }
}
