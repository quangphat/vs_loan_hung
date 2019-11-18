using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Common;
using VS_LOAN.Core.Web.Helpers;
using System.Threading.Tasks;
using F88Service;

namespace VS_LOAN.Core.Web.Controllers
{
    public class HoSoController : BaseController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_1_1, _href = "HoSo/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_1_2, _href = "HoSo/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            Session["AddNewHoSoID"] = 0;
            Session["LstFileHoSo"] = new List<TaiLieuModel>();
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSTaiLieu()
        {
            List<LoaiTaiLieuModel> rs = new LoaiTaiLieuBLL().LayDS();
            return Json(new { DSLoaiTaiLieu = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSDoiTac()
        {
            List<DoiTacModel> rs = new DoiTacBLL().LayDS();
            return Json(new { DSDoiTac = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSCourier()
        {
            List<NhanVienInfoModel> rs = new CourierCodeBLL().LayDS();
            return Json(new { DSCourier = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSSanPham(int maDoiTac)
        {
            List<SanPhamModel> rs = new SanPhamBLL().LaySanPhamByID(maDoiTac);
            return Json(new { DSSanPham = rs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSSanPhamByHS(int maDoiTac, int maHS)
        {
            List<SanPhamModel> rs = new SanPhamBLL().LaySanPhamByID(maDoiTac, maHS);
            return Json(new { DSSanPham = rs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSSale()
        {
            List<UserPMModel> rs = new List<UserPMModel>();
            var lstNhom = new NhomBLL().LayDSCuaNhanVien(GlobalData.User.IDUser);
            if (lstNhom != null)
            {
                foreach (var item in lstNhom)
                {
                    var lstNhanVien = new NhanVienNhomBLL().LayDSThanhVienNhomCaCon(item.ID);
                    if (lstNhanVien != null)
                    {
                        foreach (var jtem in lstNhanVien)
                        {
                            var user = new UserPMModel();
                            user.FullName = jtem.Ten;
                            user.Code = jtem.Code;
                            user.IDUser = jtem.ID;
                            rs.Add(user);
                        }
                    }
                }
            }
            rs.Add(GlobalData.User);
            rs = rs.GroupBy(p => p.IDUser).Select(g => g.First()).ToList();
            return Json(new { DSSale = rs });
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
        [System.Web.Http.HttpPost]
        [Route("PostToF88")]
        public async Task<ActionResult> PostToF88([FromBody] Entity.F88Model.LadipageModel model)
        {
            //var f88Model = new Entity.F88Model.LadipageModel
            //{
            //    Name = "test",
            //    Phone = phone,
            //    Link = link,
            //    Select1 = null,
            //    Select2 = provinceId.ToString(),
            //    TransactionId = hs.ID,
            //    ReferenceType = 0
            //};
            var f88Service = new F88Service.F88Service();
            var result = await f88Service.LadipageReturnID(model);
            return Json(new { Message = result }, JsonRequestBehavior.AllowGet);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<ActionResult> Save(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
           , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, int baoHiem, int thoiHanVay,
            string soTienVay, string ghiChu, string birthDayStr, string cmndDayStr, string link = null, int provinceId = 0, int doitacF88Value = 0)
        {
            if (!string.IsNullOrWhiteSpace(ghiChu) && ghiChu.Length > 200)
            {
                return ToResponse(false, "Nội dung ghi chú không được lớn hơn 200");
            }
            try
            {

                if (hoten == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập họ tên");
                }
                if (phone == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập số điện thoại");
                }
                if (ngayNhanDon == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập ngày nhận đơn");
                }
                if (cmnd == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập CMND");
                }
                if (diaChi == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập địa chỉ");
                }
                if (maKhuVuc == 0)
                {
                    return ToResponse(false, "Vui lòng chọn quận/ huyện");
                }
                if (sanPhamVay == 0)
                {
                    return ToResponse(false, "Vui lòng chọn sản phẩm vay");
                }
                if (soTienVay == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập số tiền vay");
                }
                if (string.IsNullOrWhiteSpace(birthDayStr))
                {
                    return ToResponse(false, "Vui lòng nhập ngày sinh");

                }
                if (string.IsNullOrWhiteSpace(cmndDayStr))
                {
                    return ToResponse(false, "Vui lòng nhập ngày cấp cmnd");

                }
                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["LstFileHoSo"];
                List<LoaiTaiLieuModel> lstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
                lstLoaiTaiLieu.RemoveAll(x => x.BatBuoc == 0);
                if (lstLoaiTaiLieu != null)
                {
                    foreach (var item in lstLoaiTaiLieu)
                    {
                        var iFind = lstTaiLieu.Find(x => x.MaLoai == item.ID);
                        if (iFind == null)
                        {
                            return ToResponse(false, "Vui lòng dính kèm \"" + item.Ten.ToUpper() + "\"");
                        }
                    }
                    //return ToResponse(false,"");
                }
                HoSoModel hs = new HoSoModel();
                hs.ID = (int)Session["AddNewHoSoID"];
                hs.TenKhachHang = hoten;
                hs.SDT = phone;
                hs.SDT2 = phone2;
                if (ngayNhanDon != string.Empty)
                {
                    hs.NgayNhanDon = DateTimeFormat.ConvertddMMyyyyToDateTime(ngayNhanDon);
                }
                hs.BirthDay = string.IsNullOrWhiteSpace(birthDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(birthDayStr);
                hs.CmndDay = string.IsNullOrWhiteSpace(cmndDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(cmndDayStr);
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
                hs.MaTrangThai = (int)TrangThaiHoSo.NhapLieu;
                hs.MaKetQua = (int)KetQuaHoSo.Trong;
                int result = 0;

                if (hs.ID > 0)
                {
                    bool isCheckMaSanPham = false;
                    //// chỉnh sửa
                    if (new HoSoBLL().Luu(hs, lstTaiLieu, ref isCheckMaSanPham))
                    {
                        result = 1;
                        new HoSoDuyetXemBLL().Them(hs.ID);
                        MailCM.SendMailToAdmin(hs.ID, Request.Url.Authority);
                    }
                    else
                    {
                        if (isCheckMaSanPham)
                            return ToResponse(false, "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác");
                    }
                    AddGhichu(hs.ID, ghiChu);
                }
                else
                {
                    bool isCheckMaSanPham = false;
                    result = new HoSoBLL().Them(hs, lstTaiLieu, ref isCheckMaSanPham);
                    if (result > 0)
                    {

                        Session["AddNewHoSoID"] = result;
                        new HoSoDuyetXemBLL().Them(result);
                        MailCM.SendMailToAdmin(result, Request.Url.Authority);
                    }
                    else
                    {
                        if (isCheckMaSanPham)
                            return ToResponse(false, "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác");
                    }
                    AddGhichu(result, ghiChu);
                }
                if (result > 0)
                {
                    return ToResponse(true, Resources.Global.Message_Succ);
                }
                else
                {
                    return ToResponse(false, "Không thành công, vui lòng thử lại sau");
                }

            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message);
            }

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
                        //string[] p = fileContent.ContentType.Split('/');
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
                        string deleteURL = Url.Action("Delete", "HoSo") + "?key=" + key;
                        var _type = System.IO.Path.GetExtension(fileContent.FileName);
                        List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["LstFileHoSo"];
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
                        Session["LstFileHoSo"] = lstTaiLieu;
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
                Session["LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        public JsonResult Delete(string key)
        {
            string fileUrl = "";
            try
            {

                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["LstFileHoSo"];
                lstTaiLieu.RemoveAll(x => x.MaLoai.ToString().Equals(key.ToString()));
                Session["LstFileHoSo"] = lstTaiLieu;
            }
            catch (BusinessException ex)
            {

            }
            return Json(new { Result = fileUrl });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SaveDaft(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
            , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, int baoHiem, int thoiHanVay, string soTienVay,
            string ghiChu, string birthDayStr, string cmndDayStr)
        {
            if (!string.IsNullOrWhiteSpace(ghiChu) && ghiChu.Length > 200)
            {
                return ToResponse(false, "Nội dung ghi chú không được lớn hơn 200");
            }
            try
            {
                if (hoten == string.Empty)
                {
                    return ToResponse(false, "Vui lòng nhập họ tên");
                }

                HoSoModel hs = new HoSoModel();
                hs.ID = (int)Session["AddNewHoSoID"];
                hs.TenKhachHang = hoten;
                hs.SDT = phone;
                hs.SDT2 = phone2;
                if (ngayNhanDon != string.Empty)
                {
                    hs.NgayNhanDon = DateTimeFormat.ConvertddMMyyyyToDateTime(ngayNhanDon);
                }

                hs.BirthDay = string.IsNullOrWhiteSpace(birthDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(birthDayStr);
                hs.CmndDay = string.IsNullOrWhiteSpace(cmndDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(cmndDayStr);
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
                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["LstFileHoSo"];
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
                            return ToResponse(false, "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác");
                    }
                    AddGhichu(hs.ID, ghiChu);
                }
                else
                {
                    bool isCheckMaSanPham = false;
                    result = new HoSoBLL().Them(hs, lstTaiLieu, ref isCheckMaSanPham);
                    if (result > 0)
                        Session["AddNewHoSoID"] = result;
                    else
                    {
                        if (isCheckMaSanPham)
                            return ToResponse(false, "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác");
                    }
                    AddGhichu(result, ghiChu);
                }
                if (result > 0)
                {
                    return ToResponse(true, Resources.Global.Message_Succ);
                }
                else
                {
                    return ToResponse(false, "Không thành công, vui lòng thử lại sau");
                }

            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message);
            }
            
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult Index()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult TimHS(string fromDate, string toDate, string maHS, string sdt)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            List<HoSoCuaToiModel> rs = new List<HoSoCuaToiModel>();
            try
            {
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                message.Result = true;
                string trangthai = "";
                //trangthai +=  ((int)TrangThaiHoSo.KhongDuyet).ToString()+"," + ((int)TrangThaiHoSo.Nhap).ToString() + "," + ((int)TrangThaiHoSo.Duyet).ToString() + "," + ((int)TrangThaiHoSo.ChoDuyet).ToString();
                rs = new HoSoBLL().TimHoSoCuaToi(GlobalData.User.IDUser, dtFromDate, dtToDate, maHS, sdt, trangthai);
                if (rs == null)
                    rs = new List<HoSoCuaToiModel>();
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
        public ActionResult XemHSByID(int id, string fromDate, string toDate, string mahs)
        {
            Session["HoSo_ChiTietHoSo_ID"] = id;
            return RedirectToAction("ChiTietHoSo", new { fromDate = fromDate, toDate = toDate, mahs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult ChiTietHoSo()
        {
            ViewBag.formindex = LstRole["Index"]._formindex;
            if (Session["HoSo_ChiTietHoSo_ID"] == null)
                return RedirectToAction("Index");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["HoSo_ChiTietHoSo_ID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = new DoiTacBLL().LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            ViewBag.LstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();

            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SuaHSByID(int id, string fromDate, string toDate, string makh)
        {
            Session["AddNewHoSoID"] = id;
            return RedirectToAction("SuaHoSo", new { fromDate = fromDate, toDate = toDate, makh });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SuaHoSo()
        {
            ViewBag.formindex = LstRole["Index"]._formindex;
            if (Session["AddNewHoSoID"] == null)
                return RedirectToAction("Index");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["AddNewHoSoID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = new DoiTacBLL().LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            Session["LstFileHoSo"] = hoso.LstTaiLieu;
            ViewBag.LstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult XoaHS(int hsID)
        {
            RMessage message = new RMessage { ErrorMessage = Resources.Global.Message_Error, Result = false };
            try
            {
                bool rs = new HoSoBLL().XoaHS(hsID, GlobalData.User.IDUser, DateTime.Now);
                if (rs)
                {
                    message.Result = true;
                    message.ErrorMessage = Resources.Global.Message_Succ;
                }
            }
            catch (Exception)
            {
            }
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }


    }
}
