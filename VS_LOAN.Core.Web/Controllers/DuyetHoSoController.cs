using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using VS_LOAN.Core.Utility.OfficeOpenXML;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class DuyetHoSoController : BaseController
    {
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly IGroupRepository _rpGroup;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly INoteRepository _rpNote;
        public DuyetHoSoController(IMediaBusiness mediaBusiness,
            IPartnerRepository partnerRepository,
            IGroupRepository groupRepository,
            IEmployeeRepository employeeRepository,
            INoteRepository noteRepository)
        {
            _bizMedia = mediaBusiness;
            _rpEmployee = employeeRepository;
            _rpGroup = groupRepository;
            _rpPartner = partnerRepository;
            _rpNote = noteRepository;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_2_3, _href = "DuyetHoSo/Index", _mangChucNang = new int[] { (int)QuyenIndex.DuyetHoSo } });
                return _lstRole;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.DuyetHoSo })]
        public ActionResult Index()
        {
            ViewBag.formindex = "";// LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult TimHS(
            string fromDate,
            string toDate,
            string maHS,
            string cmnd,
            int loaiNgay,
            int maNhom = 0,
            string status = null,
            string freetext = null,
            int page = 1, int limit = 10,
            int maThanhVien = 0)
        {
            List<HoSoDuyetModel> lstHoso = new List<HoSoDuyetModel>();
            if (!string.IsNullOrWhiteSpace(freetext) && freetext.Length > 50)
            {

                return ToJsonResponse(false, null, "Từ khóa tìm kiếm không được nhiều hơn 50 ký tự");
            }
            int totalRecord = 0;
            try
            {
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                string trangthai = string.IsNullOrWhiteSpace(status) ? Helpers.Helpers.GetAllStatusString() : status;

                totalRecord = new HoSoBLL().CountHosoDuyet(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, loaiNgay, trangthai, freetext);
                lstHoso = new HoSoBLL().TimHoSoDuyet(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, loaiNgay, trangthai, freetext, page, limit);
                if (lstHoso == null)
                    lstHoso = new List<HoSoDuyetModel>();
                var result = DataPaging.Create(lstHoso, totalRecord);
                return ToJsonResponse(true, null, result);
            }
            catch (BusinessException ex)
            {
                return ToJsonResponse(false, ex.Message);
            }
        }


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> UploadHoSo(string key)
        {
            string fileUrl = "";
            var file = new FileModel();
            try
            {
                foreach (string f in Request.Files)
                {
                    var fileContent = Request.Files[f];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        string[] p = fileContent.ContentType.Split('/');
                        // get a stream
                        string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");
                        stream.Position = 0;
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateProfileFolder());
                        using (var fileStream = System.IO.File.Create(file.FullPath))
                        {
                            await stream.CopyToAsync(fileStream);
                            fileStream.Close();
                            fileUrl = file.FileUrl;
                        }
                        string deleteURL = Url.Action("Delete", "QuanLyHoSo") + "?key=" + key;
                        var _type = System.IO.Path.GetExtension(fileContent.FileName);
                        List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["Duyet_LstFileHoSo"];
                        var find = lstTaiLieu.Find(x => x.MaLoai.ToString().Equals(key.Trim()));
                        if (find != null)
                        {
                            find.LstFile[0].DuongDan = fileUrl;
                            find.LstFile[0].Ten = file.Name;
                        }
                        else
                        {
                            TaiLieuModel taiLieu = new TaiLieuModel();
                            taiLieu.MaLoai = Convert.ToInt32(key.Trim());
                            Entity.Model.FileInfo _file = new Entity.Model.FileInfo();
                            _file.Ten = file.Name;
                            _file.DuongDan = fileUrl;
                            taiLieu.LstFile.Add(_file);
                            lstTaiLieu.Add(taiLieu);
                        }
                        Session["Duyet_LstFileHoSo"] = lstTaiLieu;
                        if (_type.IndexOf("pdf") > 0)
                        {
                            var config = new
                            {
                                initialPreview = fileUrl,
                                initialPreviewConfig = new[] {
                                    new {
                                        caption = file.Name,
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
                                        caption = file.Name,
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
                Session["Duyet_LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult Delete(string key)
        {
            string fileUrl = "";
            key = Request["key"];
            try
            {

                List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["Duyet_LstFileHoSo"];
                lstTaiLieu.RemoveAll(x => x.MaLoai.ToString().Equals(key.ToString()));
                Session["Duyet_LstFileHoSo"] = lstTaiLieu;
            }
            catch (BusinessException ex)
            {

            }
            return Json(new { Result = fileUrl });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult XemHSByID(int id)
        {
            Session["DuyetHoSo_ChiTietHoSo_ID"] = id;
            return RedirectToAction("Edit", new { id = id });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.DuyetHoSo })]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.formindex = LstRole["Index"]._formindex;
            if (id <= 0)
                return RedirectToAction("Index");
            ViewBag.ID = id;
            var hoso = new HoSoBLL().LayChiTiet(id);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = await _rpPartner.LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            ViewBag.LstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
            ViewBag.SellCode = new UserPMBLL().GetUserByID(hoso.HoSoCuaAi.ToString());
            //Session["Duyet_LstFileHoSo"] = hoso.LstTaiLieu;
            new HoSoDuyetXemBLL().DaXem(hoso.ID);
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> CapNhat(int id, string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
                   , int maKhuVuc, string diaChi, int sanPhamVay, string tenCuaHang,
            int baoHiem, int thoiHanVay, string soTienVay, int trangThai,
            int ketQua, string ghiChu, string birthDayStr, string cmndDayStr, int courier = 0, List<int> FileRequireIds = null)
        {
            try
            {
                if (hoten == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập họ tên");
                }
                if (phone == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập số điện thoại");
                }
                if (ngayNhanDon == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập ngày nhận đơn");
                }
                if (cmnd == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập CMND");
                }
                if (diaChi == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập địa chỉ");
                }
                if (maKhuVuc == 0)
                {
                    return ToJsonResponse(false, "Vui lòng chọn quận/ huyện");
                }
                if (sanPhamVay == 0)
                {
                    return ToJsonResponse(false, "Vui lòng chọn sản phẩm vay");
                }
                if (soTienVay == string.Empty)
                {
                    return ToJsonResponse(false, "Vui lòng nhập số tiền vay");
                }
                if (string.IsNullOrWhiteSpace(birthDayStr))
                {
                    return ToJsonResponse(false, "Vui lòng nhập ngày sinh");

                }
                if (string.IsNullOrWhiteSpace(cmndDayStr))
                {
                    return ToJsonResponse(false, "Vui lòng nhập ngày cấp cmnd");

                }
                //List<TaiLieuModel> lstTaiLieu = (List<TaiLieuModel>)Session["Duyet_LstFileHoSo"];
                List<LoaiTaiLieuModel> lstLoaiTaiLieu = new LoaiTaiLieuBLL().LayDS();
                lstLoaiTaiLieu.RemoveAll(x => x.BatBuoc == 0);
                if (lstLoaiTaiLieu != null)
                {
                    var missingNames = BusinessExtension.GetFilesMissingV2(lstLoaiTaiLieu, FileRequireIds);
                    if (!string.IsNullOrWhiteSpace(missingNames))
                    {
                        return ToJsonResponse(false, $"Vui lòng nhập: {missingNames}");
                    }
                }

                HoSoModel hs = new HoSoModel();
                hs.ID = id;
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
                hs.CoBaoHiem = baoHiem;
                hs.HanVay = thoiHanVay;
                var dtBirthDayConvert = DateTimeFormat.ConvertddMMyyyyToDateTimeV2(birthDayStr);
                if (!dtBirthDayConvert.Success)
                {
                    return ToJsonResponse(false, dtBirthDayConvert.Message);
                }
                else
                {
                    hs.BirthDay = dtBirthDayConvert.Value;
                }

                var dtCmnd = DateTimeFormat.ConvertddMMyyyyToDateTimeV2(cmndDayStr);
                if (!dtCmnd.Success)
                {
                    return ToJsonResponse(false, dtCmnd.Message);
                }
                else
                {
                    hs.CmndDay = dtCmnd.Value;
                }

                if (soTienVay == string.Empty)
                    soTienVay = "0";
                hs.SoTienVay = Convert.ToDecimal(soTienVay);
                hs.MaTrangThai = trangThai;
                hs.MaKetQua = ketQua;
                int result = 0;
                if (hs.ID > 0)
                {
                    var hosoOld = new HoSoBLL().LayChiTiet(id);
                    if (!(hosoOld.MaTrangThai == trangThai && hosoOld.MaKetQua == ketQua))
                    {
                        new HoSoXemBLL().Them(hosoOld.ID);
                    }

                    hs.DisbursementDate = hosoOld.DisbursementDate;
                    if (hs.MaTrangThai == (int)TrangThaiHoSo.GiaiNgan)
                        hs.DisbursementDate = DateTime.Now;
                    bool isCheckMaSanPham = false;
                    //// chỉnh sửa
                    if (new HoSoBLL().CapNhatHoSo(hs, null, ref isCheckMaSanPham))
                    {
                        result = 1;
                    }
                    else
                    {
                        if (isCheckMaSanPham)
                            return ToJsonResponse(false, "Mã sản phẩm đã được sử dụng bởi 1 hồ sơ khác, vui lòng chọn mã sản phẩm khác");
                    }
                }

                if (result > 0)
                {
                    bool rs = new HoSoBLL().CapNhatTrangThaiHS(id, GlobalData.User.IDUser, DateTime.Now, trangThai, ketQua, ghiChu);

                    if (rs)
                    {
                        GhichuModel ghichu = new GhichuModel
                        {
                            UserId = GlobalData.User.IDUser,
                            HosoId = hs.ID,
                            Noidung = ghiChu,
                            CommentTime = DateTime.Now,
                            TypeId = (int)NoteType.Hoso
                        };
                       
                        await _rpNote.AddNoteAsync(ghichu);
                        return ToJsonResponse(true, Resources.Global.Message_Succ, hs.ID);
                    }
                    return ToJsonResponse(false, "Không thành công, xin thử lại sau");
                }
                return ToJsonResponse(false, "Không thành công, xin thử lại sau");
            }
            catch (Exception e)
            {
                return ToJsonResponse(false, e.Message);
            }

        }
        public async Task<JsonResult> SendToF88(int hosoId, string customerName, string phone, string provinceName, string district, string link = null)
        {
            if (hosoId <= 0)
            {
                return ToJsonResponse(false, "Mã hồ sơ không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return ToJsonResponse(false, "Tên khách hàng không được bỏ trống");
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                return ToJsonResponse(false, "Số điện thoại khách hàng không được bỏ trống");
            }
            if (!string.IsNullOrWhiteSpace(district))
            {
                if (district.Contains("Huyện"))
                {
                    district = district.Replace("Huyện", "").Trim();
                }
                if (district.Contains("Quận"))
                {
                    district = district.Replace("Quận", "").Trim();
                }
                if (district.Contains("Thành phố"))
                {
                    district = district.Replace("Thành phố", "").Trim();
                }
                if (district.Contains("Thị xã"))
                {
                    district = district.Replace("Thị xã", "").Trim();
                }
            }
            var f88Service = new F88Service.F88Service();
            var f88Model = new Entity.F88Model.LadipageModel
            {
                Name = customerName,
                Phone = phone,
                Link = link,
                Select1 = null,
                District = district,
                Select2 = district + " - " + provinceName,
                TransactionId = hosoId,
                ReferenceType = 0,
                Province = provinceName
            };
            var result = await f88Service.LadipageReturnID(f88Model);
            return ToJsonResponse(result.Success, result.Message);
        }
        public JsonResult LayDSNhom()
        {
            List<NhomDropDownModel> rs = new GroupRepository().LayDSDuyetCuaNhanVien(GlobalData.User.IDUser);
            if (rs == null)
                rs = new List<NhomDropDownModel>();
            return ToJsonResponse(true, null, rs);
        }

        public async Task<JsonResult> LayDSThanhVienNhom(int maNhom)
        {
            var rs = new List<OptionSimple>();
            if (maNhom > 0)
                rs = await _rpEmployee.LayDSThanhVienNhomCaConAsync(maNhom, GlobalData.User.IDUser);
            else
            {
                // Lấy ds nhóm của nv quản lý
                List<NhomDropDownModel> lstNhom = new GroupRepository().LayDSDuyetCuaNhanVien(GlobalData.User.IDUser);
                if (lstNhom != null)
                {
                    for (int i = 0; i < lstNhom.Count; i++)
                    {
                        var lstThanhVien = await _rpGroup.LayDSThanhVienNhomAsync(lstNhom[i].ID, GlobalData.User.IDUser);
                        if (lstThanhVien != null)
                        {
                            for (int j = 0; j < lstThanhVien.Count; j++)
                            {
                                if (rs.Find(x => x.Id == lstThanhVien[j].Id) == null)
                                    rs.Add(lstThanhVien[j]);
                            }
                        }
                    }
                }
            }
            if (rs == null)
                rs = new List<OptionSimple>();
            return ToJsonResponse(true, null, rs);
        }
        public JsonResult LayDSGhichu(int id)
        {
            List<GhichuViewModel> rs = new HoSoBLL().LayDanhsachGhichu(id);
            if (rs == null)
                rs = new List<GhichuViewModel>();
            return ToJsonResponse(true, null, rs);
        }
        public JsonResult LayDSTrangThai()
        {
            //var isTeamlead = GlobalData.User.UserType == (int)UserTypeEnum.Teamlead ? true : false;
            //var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            //if (!isTeamlead && !isAdmin)
            //    return Json(new { DSTrangThai = new List<TrangThaiHoSoModel>() });
            List<TrangThaiHoSoModel> rs = new TrangThaiHoSoBLL().LayDSTrangThai();
            if (rs == null)
                rs = new List<TrangThaiHoSoModel>();
            //rs.RemoveAll(x => x.ID == (int)TrangThaiHoSo.Nhap);
            //rs.RemoveAll(x => x.ID == (int)TrangThaiHoSo.NhapLieu);
            //if (isAdmin)
            //{
            //    rs.RemoveAll(x => x.ID == (int)TrangThaiHoSo.DaDoiChieu);
            //}
            return ToJsonResponse(true, null, rs);
        }

        public JsonResult LayDSKetQua()
        {
            List<KetQuaHoSoModel> rs = new KetQuaHoSoBLL().LayDSKetQua();
            if (rs == null)
                rs = new List<KetQuaHoSoModel>();
            return ToJsonResponse(true, null, rs);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult DownloadReport(int maNhom, int maThanhVien, string fromDate, string toDate, string maHS, string cmnd, int loaiNgay)
        {
            string newUrl = string.Empty;
            try
            {
                List<HoSoDuyetModel> rs = new List<HoSoDuyetModel>();
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                string trangthai = "";
                trangthai += ((int)TrangThaiHoSo.TuChoi).ToString() + ","
                    + ((int)TrangThaiHoSo.NhapLieu).ToString() + ","
                    + ((int)TrangThaiHoSo.ThamDinh).ToString() + ","
                    + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + ","
                    + ((int)TrangThaiHoSo.Cancel).ToString() + ","
                    + ((int)TrangThaiHoSo.DaDoiChieu).ToString() + ","
                    + ((int)TrangThaiHoSo.PCB).ToString() + ","
                    + ((int)TrangThaiHoSo.GiaiNgan).ToString();
                int totalRecord = new HoSoBLL().CountHosoDuyet(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, loaiNgay, trangthai);
                rs = new HoSoBLL().TimHoSoDuyet(GlobalData.User.IDUser, maNhom, maThanhVien, dtFromDate, dtToDate, maHS, cmnd, loaiNgay, trangthai, string.Empty, 1, totalRecord, true);
                if (rs == null)
                    rs = new List<HoSoDuyetModel>();
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
                                excelOOXML.SetCellData(nameSheet, "N" + rowindex, rs[i].DiaChiKH);
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
                    return ToJsonResponse(true, null, newUrl);
                }
                return ToJsonResponse(false);
            }
            catch (BusinessException ex)
            {
                return ToJsonResponse(false, ex.Message);
            }
        }
    }
}
