using MCreditService.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Utility;
using Newtonsoft.Json;
using MCreditService;

namespace VS_LOAN.Core.Web.Controllers
{
    public class OcbController : BaseController
    {
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly IOcbRepository _rpMCredit;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IOdcService _odcService;
        public readonly IOcbBusiness _ocbBusiness;
        public static ProvinceResponseModel _provinceResponseModel;
        public OcbController(IOcbRepository rpMCredit ,
              IMediaBusiness mediaBusiness,
            IOdcService odcService, IOcbBusiness ocbBusiness, ITailieuRepository tailieuBusiness) : base()
        {
            _rpTailieu = tailieuBusiness;
            _rpMCredit = rpMCredit;
            _odcService = odcService;
            _bizMedia = mediaBusiness;
            _ocbBusiness = ocbBusiness;


        }
        public async Task<JsonResult> SearchTemps(string freeText, string status, int page = 1, int limit = 10, string fromDate = null, string toDate = null, int loaiNgay = 0, int manhom = 0,

          int mathanhvien = 0)
        {
            page = page <= 0 ? 1 : page;

            DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.Now.AddDays(3);
            if (fromDate != "")
                dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
            if (toDate != "")
                dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

            var profiles = await _rpMCredit.GetTempProfiles(page, limit, freeText, GlobalData.User.IDUser, status, dtFromDate, dtToDate, loaiNgay, manhom, mathanhvien = 0);
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<OcbSerarchSql>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }

        public JsonResult LayDanhSachTinh()
        {
            return ToJsonResponse(true, "", OdbServiceService.AllProvice.Where(x => int.Parse(x.ProvinceId) < 7901).ToList());
        }



        public async Task<JsonResult> UpdateAsync(OCBProfileEditModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            //var profile = new OcbProfile();
            var profilerequest = await _rpMCredit.GetTemProfileByMcId(model.Id);
            var profile = new OcbProfile();
            profile.Id = profilerequest.Id;
            profile.FullNamme = model.FullNamme;
            profile.Gender = model.Gender;
            profile.IdCard = model.IdCard;
            profile.Mobilephone = model.Mobilephone;
            profile.RegAddressWardId = model.RegAddressWardId;
            profile.RegAddressDistId = model.RegAddressDistId;
            profile.RegAddressProvinceId = model.RegAddressProvinceId;
            profile.CurAddressDistId = model.CurAddressDistId;
            profile.CurAddressWardId = model.CurAddressWardId;
            profile.CurAddressProvinceId = model.CurAddressProvinceId;
            profile.InCome = model.InCome;
            profile.RequestLoanAmount = model.RequestLoanAmount;
            profile.RequestLoanTerm = model.RequestLoanTerm;
            profile.ProductId = model.ProductId;
            profile.SellerNote = model.SellerNote;
            profile.RegAddressNumber = model.RegAddressNumber;
            profile.RegAddressStreet = model.RegAddressStreet;
            profile.RegAddressRegion = model.RegAddressRegion;
            profile.CurAddressNumber = model.CurAddressNumber;
            profile.CurAddressStreet = model.CurAddressStreet;
            profile.CurAddressRegion = model.CurAddressRegion;
            profile.Status = model.Status;
            profile.IsDuplicateAdrees = model.IsDuplicateAdrees;
            if (model.IsDuplicateAdrees==true)
            {
                profile.RegAddressNumber = model.CurAddressNumber;
                profile.RegAddressStreet = model.CurAddressStreet;
                profile.RegAddressRegion = model.CurAddressRegion;
                profile.RegAddressProvinceId = model.CurAddressProvinceId;
                profile.RegAddressDistId = model.CurAddressDistId;
                profile.RegAddressWardId = model.CurAddressWardId;


            }
            profile.IncomeType = model.IncomeType;
            profile.Email = model.Email;
            profile.BirthDay = string.IsNullOrWhiteSpace(model.birthDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.birthDayStr);
            profile.IdIssueDate = string.IsNullOrWhiteSpace(model.IdIssueDatestr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.IdIssueDatestr);
            profile.IncomeType = model.IncomeType;
            profile.CreatedBy = GlobalData.User.IDUser;

            profile.ReferenceFullName1 = model.ReferenceFullName1;
            profile.ReferencePhone1 = model.ReferencePhone1;
            profile.Reference1Gender = model.Reference1Gender;
            profile.ReferenceRelationship1 = model.ReferenceRelationship1;

            profile.ReferenceFullName2 = model.ReferenceFullName2;
            profile.ReferencePhone2 = model.ReferencePhone2;
            profile.Reference2Gender = model.Reference2Gender;
            profile.ReferenceRelationship2 = model.ReferenceRelationship2;


            profile.ReferenceFullName3 = model.ReferenceFullName3;
            profile.ReferencePhone3 = model.ReferencePhone3;
            profile.Reference3Gender = model.Reference3Gender;
            profile.ReferenceRelationship3 = model.ReferenceRelationship3;

            var result = await _rpMCredit.UpdateDraftProfile(profile);
            
            if (!result)
            {
                return ToJsonResponse(result, "Lỗi cập nhật");
            }

            return ToJsonResponse(true);
        }

        public async Task<JsonResult> SumbitToOcb (int id)
        {

            var result = await _rpMCredit.GetTemProfileByMcId(id);
            var resultReponse =await _odcService.CreateLead(result);
            return ToJsonResponse(resultReponse.Status=="200", "",resultReponse);
        }


        public async Task<JsonResult> CreateDraft(OCBProfileAddModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = new OcbProfile();

            profile.TraceCode = model.TraceCode;
            profile.FullNamme = model.FullNamme;
            profile.Gender = model.Gender;
            profile.IdCard = model.IdCard;
            profile.Mobilephone = model.Mobilephone;
            profile.RegAddressWardId = model.RegAddressWardId;
            profile.RegAddressDistId = model.RegAddressDistId;
            profile.RegAddressProvinceId = model.RegAddressProvinceId;
            profile.CurAddressDistId = model.CurAddressDistId;
            profile.CurAddressWardId = model.CurAddressWardId;
            profile.CurAddressProvinceId = model.CurAddressProvinceId;
            profile.InCome = model.InCome;
            profile.RequestLoanAmount = model.RequestLoanAmount;
            profile.RequestLoanTerm = model.RequestLoanTerm;
            profile.ProductId = model.ProductId;
            profile.SellerNote = model.SellerNote;
            profile.RegAddressNumber = model.RegAddressNumber;
            profile.RegAddressStreet = model.RegAddressStreet;
            profile.RegAddressRegion = model.RegAddressRegion;
            profile.CurAddressNumber = model.CurAddressNumber;
            profile.CurAddressStreet = model.CurAddressStreet;
            profile.CurAddressRegion = model.CurAddressRegion;
            profile.IncomeType = model.IncomeType;
            profile.Email = model.Email;
            profile.IsDuplicateAdrees = model.IsDuplicateAdrees;
            profile.Status = model.Status;

            if (model.IsDuplicateAdrees == true)
            {
            profile.RegAddressNumber = model.CurAddressNumber;
            profile.RegAddressStreet = model.CurAddressStreet;
            profile.RegAddressRegion = model.CurAddressRegion;
            profile.RegAddressProvinceId = model.CurAddressProvinceId;
            profile.RegAddressDistId = model.CurAddressDistId;
            profile.RegAddressWardId = model.CurAddressWardId;
            }
            profile.BirthDay = string.IsNullOrWhiteSpace(model.birthDayStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.birthDayStr);
            profile.IdIssueDate = string.IsNullOrWhiteSpace(model.IdIssueDatestr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.IdIssueDatestr);
            profile.IncomeType = model.IncomeType;
            profile.CreatedBy = GlobalData.User.IDUser;


            profile.ReferenceFullName1 = model.ReferenceFullName1;
            profile.ReferencePhone1 = model.ReferencePhone1;
            profile.Reference1Gender = model.Reference1Gender;
            profile.ReferenceRelationship1 = model.ReferenceRelationship1;

            profile.ReferenceFullName2 = model.ReferenceFullName2;
            profile.ReferencePhone2 = model.ReferencePhone2;
            profile.Reference2Gender = model.Reference2Gender;
            profile.ReferenceRelationship2 = model.ReferenceRelationship2;

            profile.ReferenceFullName3 = model.ReferenceFullName3;
            profile.ReferencePhone3 = model.ReferencePhone3;
            profile.Reference3Gender = model.Reference3Gender;
            profile.ReferenceRelationship3 = model.ReferenceRelationship3;

            var id = await _rpMCredit.CreateDraftProfile(profile);
            if (id > 0)
            {
               
            }
            return ToJsonResponse(true, "", id);
        }

           public async Task<ActionResult> OcbProfile(int id)
        {
            var result = await _rpMCredit.GetTemProfileByMcId(id);
            //ViewBag.pushDocument = result.IsPushDocument.Value == true;
            //ViewBag.pushOCB = string.IsNullOrEmpty(result.CustomerId);
            //ViewBag.DisableUpdate = (result.Status == 7 || result.Status == 1 || result.Status == 4 || result.Status == 5);
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;
            ViewBag.LstTaiLieu = new List<TaiLieuModel>();
            ViewBag.LstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList(7);
            return View();
        }

      

    
        public async Task<JsonResult> SendFile(int id)
        {
            if (id <= 0)
                return ToJsonResponse(false);
            var result = await _rpMCredit.GetTemProfileByMcId(id);


            if(result==null)
            {
                return ToJsonResponse(false, "không tìm thấy hồ sơ");
            }

            if(result.IsPushDocument!=null)
            {
                if (result.IsPushDocument==true)
                {
                    return ToJsonResponse(false, "Đã đẩy chứng từ rồi, không được đẩy nữa");
                }
            }
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;

            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList(7);
            if (lstLoaiTailieu == null || !lstLoaiTailieu.Any())
                return ToJsonResponse(false);

            List<OcbSendfileReQuestModel> sendObjectRequest = new List<OcbSendfileReQuestModel>();
            var filesExist = await _rpTailieu.GetTailieuOCByHosoId(id, 7);
            foreach (var item in filesExist)
            {
                string _fieldName = "DRIVING_LICENSE_ATTACH";
                switch (item.Key)
                {
                    case 51:
                        _fieldName = "DRIVING_LICENSE_ATTACH";
                        break;

                    case 52:
                        _fieldName = "FAMILY_BOOK_ATTACH";
                        break;

                    case 53:
                        _fieldName = "IDCARD_ATTACH";
                        break;
                    case 54:
                        _fieldName = "CLIENT_PICTURE_ATTACH";
                        break;

                    case 55:
                        _fieldName = "OTHER_ATTACH";
                        break;
                    case 56:
                        _fieldName = "BUSINESS_LICENSE_ATTACH";
                        break;
                    case 57:
                        _fieldName = "LABOR_CONTRACT_ATTACH";
                        break;
                }

                string fileType = System.IO.Path.GetExtension(item.FileUrl);
                if(fileType=="" )
                {
                    continue;
                }
                string filePath = Server.MapPath(item.FileUrl);
                using (var fileContent = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] filebytes = new byte[fileContent.Length];
                    fileContent.Read(filebytes, 0, Convert.ToInt32(fileContent.Length));
                    string encodedData = Convert.ToBase64String(filebytes,
                                            Base64FormattingOptions.InsertLineBreaks);
                    var request = new OcbSendfileReQuestModel()
                    {
                        CustomerId = result.CustomerId,
                        Fieldname = _fieldName,
                        FileType = fileType,
                        FileContent = encodedData

                    };
                
                    switch (fileType.ToLower())
                    {
                        case ".pdf":
                            fileType = "pdf";
                            break;

                        case ".png":
                            fileType = "png";
                            break;
                        case ".jpg":
                            fileType = "jpg";
                            break;
                        default:
                            break;
                    }
                    request.FileType = fileType;
                    sendObjectRequest.Add(request);

                }

            }

            var isSuccess = true;
             foreach (var item in sendObjectRequest)
            {
                 var resultSendfile=   await  _odcService.SendFile(item);
                if(resultSendfile.Status !="200")
                {

                    isSuccess = false;
                }
             
            }
            return ToJsonResponse(isSuccess);

        }


        public JsonResult LayDanhSachThanhPho(string province)
        {
            return ToJsonResponse(true, "", OdbServiceService.AllCity.Where(x => x.ProvinceId == province).ToList());

        }
        public JsonResult LayDanhSachWard(string cityCode)
        {
            return ToJsonResponse(true, "", OdbServiceService.AllWard.Where(x => x.CityId == cityCode ).ToList());

        }

        public async Task<ActionResult> Temp()
        {

          await _odcService.CheckAuthen();


            return View();
        }

        public async Task<ActionResult> Index()
        {

            await _odcService.CheckAuthen();


            return View("Temp");
        }
        public async Task<JsonResult> AuthenAsync()
        {

            await _odcService.Authen();
             await _odcService.GetAllCity(new CityRequestModel());
            await _odcService.GetAllProvince();
            await  _odcService.GetAllWard(new WardRequestModel());
            await _odcService.GetAllDictionary();
            return ToJsonResponse(true, "Thành công");

        }

        public ActionResult AddNew()

        {
            return View();
        }
        public async Task<JsonResult> Import()
        {
            var file = Request.Files[0];
            if (file == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            Stream stream = file.InputStream;
            stream.Position = 0;
            try
            {
                await _ocbBusiness.HandleFileImport(stream, GlobalData.User.IDUser);
            }
            catch (Exception e)
            {
                return ToJsonResponse(false,e.Message);
            }
            return ToJsonResponse(true, "Import thành công");
        }
        public async Task<JsonResult> GetAllStatus()
        {
            var result = await _rpMCredit.GetAllStatus();
            return ToJsonResponse(true, "", data: result);
        }
        public async Task<JsonResult> GetLoanProduct(int MaDoiTac)
        {
            var result = await _rpMCredit.GetLoanProduct(MaDoiTac);
            return ToJsonResponse(true, "", data: result);
        }
        public async Task<JsonResult> Comments(int profileId)
        {
            var result = await _rpMCredit.GetCommentsAsync(profileId);
            return ToJsonResponse(true, null, result);
        }
        public async Task<JsonResult> AddNote(int profileId, StringModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _rpMCredit.AddNoteAsync(profileId, model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(result.IsSuccess, result.Message);
        }
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
                return ToJsonResponse(false);

            if (isReset)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieuOcb(hosoId, (int)HosoType.Ocb);
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
                            ProfileTypeId = (int)HosoType.Ocb,
                            Folder = file.FileUrl
                        };
                        await _rpTailieu.AddOCB(tailieu);
                    }
                }
            }
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> UploadFile(int key, int fileId, int type)
        {
            type = 7;
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
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateOcbProfile());
                        using (var fileStream = System.IO.File.Create(file.FullPath))
                        {
                            await stream.CopyToAsync(fileStream);
                            fileStream.Close();
                            fileUrl = file.FileUrl;
                        }
                        deleteURL = fileId <= 0 ? $"/Ocb/delete?key={key}" : $"/Ocb/delete/0/{fileId}";
                        if (fileId > 0)
                        {

                            await _rpTailieu.UpdateExistingFileOCB(new TaiLieu
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
            var result = await _rpTailieu.RemoveTailieuOcb(hosoId, fileId);
            return ToJsonResponse(true);
        }
        public  string ConvertToBase64( Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }
        public JsonResult Delete(int key)
        {
            string fileUrl = "";

            return Json(new { Result = fileUrl });
        }
        public async Task<JsonResult> TailieuByHosoForEdit(int hosoId, int typeId = 7)
        {
            if (hosoId <= 0)
            {
                return ToJsonResponse(false, null, "Dữ liệu không hợp lệ");
            }
            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList(7);
            if (lstLoaiTailieu == null || !lstLoaiTailieu.Any())
                return ToJsonResponse(false);

            typeId = 7;

            var filesExist = await _rpTailieu.GetTailieuOCByHosoId(hosoId, typeId);

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
            var result = await _rpTailieu.GetTailieuOCByHosoId(hosoId, type);
            if (result == null)
                result = new List<FileUploadModel>();
            return ToJsonResponse(true, null, result);
        }



        

    }
}