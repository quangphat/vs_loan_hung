using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Common;
using VS_LOAN.Core.Web.Helpers;
using System.Threading.Tasks;
using F88Service;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Business.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class HoSoController : BaseController
    {
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly INoteRepository _rpNote;
        public HoSoController(ITailieuRepository tailieuBusiness,
            IEmployeeRepository employeeRepository,
            IMediaBusiness mediaBusiness,
            IPartnerRepository partnerRepository,
            INoteRepository noteRepository) : base()
        {
            _rpTailieu = tailieuBusiness;
            _rpPartner = partnerRepository;
            _bizMedia = mediaBusiness;
            _rpEmployee = employeeRepository;
            _rpNote = noteRepository;
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult AddNew()
        {
            ViewBag.formindex = "";// Infrastructures.ControllerRoles.Roles[RouteData.Values["action"].ToString()]._formindex;
            Session["AddNewHoSoID"] = 0;
            Session["LstFileHoSo"] = new List<TaiLieuModel>();
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> LayDSTaiLieu()
        {
            var rs = await _rpTailieu.GetLoaiTailieuList();
            return ToJsonResponse(true, null, rs);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> LayDSDoiTac()
        {
            var rs = await _rpPartner.LayDS();
            return ToJsonResponse(true, null, rs);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> LayDSCourier()
        {
            var rs = await _rpEmployee.GetCourierList();
            return ToJsonResponse(true, null, rs);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSSanPham(int maDoiTac)
        {
            List<SanPhamModel> rs = new SanPhamBLL().LaySanPhamByID(maDoiTac);
            return ToJsonResponse(true, null, rs);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSSanPhamByHS(int maDoiTac, int maHS)
        {
            List<SanPhamModel> rs = new SanPhamBLL().LaySanPhamByID(maDoiTac, maHS);
            return ToJsonResponse(true, null, rs);
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<JsonResult> LayDSSale()
        {
            List<UserPMModel> rs = new List<UserPMModel>();
            var lstNhom = new GroupRepository().LayDSCuaNhanVien(GlobalData.User.IDUser);
            if (lstNhom != null)
            {
                foreach (var item in lstNhom)
                {
                    var lstNhanVien =await _rpEmployee.LayDSThanhVienNhomCaConAsync(item.ID, GlobalData.User.IDUser);
                    if (lstNhanVien != null)
                    {
                        foreach (var jtem in lstNhanVien)
                        {
                            var user = new UserPMModel();
                            user.FullName = jtem.Name;
                            user.Code = jtem.Code;
                            user.IDUser = jtem.Id;
                            rs.Add(user);
                        }
                    }
                }
            }
            rs.Add(GlobalData.User);
            rs = rs.GroupBy(p => p.IDUser).Select(g => g.First()).ToList();
            return ToJsonResponse(true, null, rs);
        }
        private async Task<bool> AddGhichu(int hosoId, string ghiChu)
        {
            GhichuModel ghichu = new GhichuModel
            {
                UserId = GlobalData.User.IDUser,
                HosoId = hosoId,
                Noidung = ghiChu,
                CommentTime = DateTime.Now,
                TypeId = (int)NoteType.Hoso
            };
            
            await _rpNote.AddNoteAsync(ghichu);
            return true;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("PostToF88")]
        public async Task<ActionResult> PostToF88([FromBody] Entity.F88Model.LadipageModel model)
        {
            var f88Service = new F88Service.F88Service();
            var result = await f88Service.LadipageReturnID(model);
            if (result.Success)
                return ToJsonResponse(true, null, result);
            return ToJsonResponse(false, result.Message, null);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<ActionResult> Save(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
           , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, int baoHiem, int thoiHanVay,
            string soTienVay, string ghiChu, string birthDayStr, string cmndDayStr, string link = null, int provinceId = 0, int doitacF88Value = 0, List<int> FileRequireIds = null, int partnerType = 0)
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

                var lstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList();
                lstLoaiTaiLieu.RemoveAll(x => x.BatBuoc == 0);
                if (lstLoaiTaiLieu != null)
                {
                    var missingNames = BusinessExtension.GetFilesMissingV2(lstLoaiTaiLieu, FileRequireIds);
                    if (!string.IsNullOrWhiteSpace(missingNames))
                    {
                        return ToResponse(false, $"Vui lòng nhập: {missingNames}", 0);
                    }
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
                    await AddGhichu(hs.ID, ghiChu);
                    return ToResponse(true, Resources.Global.Message_Succ, hs.ID);
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
                    await AddGhichu(result, ghiChu);
                }
                if (result > 0)
                {
                    return ToResponse(true, Resources.Global.Message_Succ, result);
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
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
                return ToJsonResponse(false);

            if (isReset)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieu(hosoId, (int)HosoType.Hoso);
                if (!deleteAll)
                    return ToJsonResponse(false);
            }
            foreach (var item in filesGroup)
            {
                if (item.files.Any())
                {
                    foreach (var file in item.files)
                    {
                        var tailieu = new TaiLieu
                        {
                            FileName = file.FileName,
                            FilePath = file.FileUrl,
                            ProfileId = hosoId,
                            FileKey = Convert.ToInt32(file.Key),
                            ProfileTypeId = (int)HosoType.Hoso,
                            Folder = file.FileUrl
                        };
                        await _rpTailieu.Add(tailieu);
                    }
                }
            }
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> UploadFile(int key, int fileId, int type)
        {
            string fileUrl = "";
            var _type = string.Empty;
            string deleteURL = string.Empty;
            var file = new FileModel();
            try
            {
                foreach (string f in Request.Files)
                {
                    var fileContent = Request.Files[f];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");
                        stream.Position = 0;
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateProfileFolder());
                        using (var fileStream = System.IO.File.Create(file.FullPath))
                        {
                            await stream.CopyToAsync(fileStream);
                            fileStream.Close();
                            fileUrl = file.FileUrl;
                        }
                        deleteURL = fileId <= 0 ? $"/hoso/delete?key={key}" : $"/hoso/delete/0/{fileId}";
                        if (fileId > 0)
                        {
                           
                            await _rpTailieu.UpdateExistingFile(new TaiLieu
                            {
                                FileName = file.Name,
                                Folder = file.Folder,
                                FilePath = file.FileUrl,
                                ProfileId = 0,
                                ProfileTypeId = type
                            }, fileId);
                        }
                        _type = System.IO.Path.GetExtension(fileContent.FileName);
                    }

                }
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
                //return Json(result);
            }
            catch (Exception)
            {
                Session["LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        public async Task<JsonResult> RemoveTailieuByHoso(int hosoId, int fileId)
        {
            if (fileId <= 0)
            {
                return ToJsonResponse(false, null, "Dữ liệu không hợp lệ");
            }
            var result = await _rpTailieu.RemoveTailieu(hosoId, fileId);
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> TailieuByHosoForEdit(int hosoId, int typeId = 1)
        {
            if (hosoId <= 0)
            {
                return ToJsonResponse(false, null, "Dữ liệu không hợp lệ");
            }
            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList();
            if (lstLoaiTailieu == null || !lstLoaiTailieu.Any())
                return ToJsonResponse(false);

            var filesExist = await _rpTailieu.GetTailieuByHosoId(hosoId, typeId);

            var result = new List<HosoTailieu>();

            foreach (var loai in lstLoaiTailieu)
            {
                var tailieus = filesExist.Where(p => p.Key == loai.ID);

                var item = new HosoTailieu
                {
                    ID = loai.ID,
                    Ten = loai.Ten,
                    BatBuoc = loai.BatBuoc,
                    Tailieus = tailieus != null ? tailieus.ToList() : new List<FileUploadModel>()
                };
                result.Add(item);

            }
            return ToJsonResponse(true, null, result);
        }
        public async Task<JsonResult> TailieuByHoso(int hosoId, int type = 1)
        {
            var result = await _rpTailieu.GetTailieuByHosoId(hosoId, type);
            if (result == null)
                result = new List<FileUploadModel>();
            return ToJsonResponse(true, null, result);
        }
        public JsonResult Delete(int key)
        {
            string fileUrl = "";

            return Json(new { Result = fileUrl });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<ActionResult> SaveDaft(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
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
                List<TaiLieuModel> lstTaiLieu = new List<TaiLieuModel>();
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
                    await AddGhichu(hs.ID, ghiChu);
                    return ToResponse(true, Resources.Global.Message_Succ, hs.ID);
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
                    await AddGhichu(result, ghiChu);
                }
                if (result > 0)
                {
                    return ToResponse(true, Resources.Global.Message_Succ, result);
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
            ViewBag.formindex = "";// Infrastructures.ControllerRoles.Roles[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult TimHS(string fromDate, string toDate, string maHS, string sdt)
        {
            List<HoSoCuaToiModel> rs = new List<HoSoCuaToiModel>();
            try
            {
                DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTime(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTime(toDate);
                string trangthai = "";
                //trangthai +=  ((int)TrangThaiHoSo.KhongDuyet).ToString()+"," + ((int)TrangThaiHoSo.Nhap).ToString() + "," + ((int)TrangThaiHoSo.Duyet).ToString() + "," + ((int)TrangThaiHoSo.ChoDuyet).ToString();
                rs = new HoSoBLL().TimHoSoCuaToi(GlobalData.User.IDUser, dtFromDate, dtToDate, maHS, sdt, trangthai);
                if (rs == null)
                    rs = new List<HoSoCuaToiModel>();
                return ToJsonResponse(true, null, rs);
            }
            catch (BusinessException ex)
            {
                return ToJsonResponse(false, ex.Message);
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult XemHSByID(int id, string fromDate, string toDate, string mahs)
        {
            Session["HoSo_ChiTietHoSo_ID"] = id;
            return RedirectToAction("ChiTietHoSo", new { fromDate = fromDate, toDate = toDate, mahs });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<ActionResult> ChiTietHoSo()
        {
            ViewBag.formindex = Infrastructures.ControllerRoles.Roles["profile_list"]._formindex;
            if (Session["HoSo_ChiTietHoSo_ID"] == null)
                return RedirectToAction("Index");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["HoSo_ChiTietHoSo_ID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac =await _rpPartner.LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            ViewBag.LstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList();

            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult SuaHSByID(int id, string fromDate, string toDate, string makh)
        {
            Session["AddNewHoSoID"] = id;
            return RedirectToAction("SuaHoSo", new { fromDate = fromDate, toDate = toDate, makh });
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public async Task<ActionResult> SuaHoSo()
        {
            ViewBag.formindex = Infrastructures.ControllerRoles.Roles["profile_list"]._formindex;
            if (Session["AddNewHoSoID"] == null)
                return RedirectToAction("Index");
            var hoso = new HoSoBLL().LayChiTiet((int)Session["AddNewHoSoID"]);
            ViewBag.HoSo = hoso;
            ViewBag.MaDoiTac = await _rpPartner.LayMaDoiTac(hoso.SanPhamVay);
            ViewBag.MaTinh = new KhuVucBLL().LayMaTinh(hoso.MaKhuVuc);
            Session["LstFileHoSo"] = hoso.LstTaiLieu;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult XoaHS(int hsID)
        {
            try
            {
                bool rs = new HoSoBLL().XoaHS(hsID, GlobalData.User.IDUser, DateTime.Now);
                if (rs)
                {
                    return ToJsonResponse(true);
                }
                return ToJsonResponse(false);
            }
            catch (Exception)
            {
                return ToJsonResponse(false);
            }

        }


    }
}
