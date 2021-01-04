using MCreditService;
using MCreditService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MiraeDeferController : BaseController
    {

        protected readonly IMiraeRepository _rpMCredit;

        protected readonly IMiraeDeferRepository _deferRepository;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IMiraeService _odcService;
        public readonly IOcbBusiness _ocbBusiness;
        public readonly IMiraeMaratialRepository _rpTailieu;
        public static ProvinceResponseModel _provinceResponseModel;
        public MiraeDeferController(IMiraeRepository rpMCredit,
              IMediaBusiness mediaBusiness,
            IMiraeService odcService, IOcbBusiness ocbBusiness, IMiraeMaratialRepository tailieuBusiness,

            IMiraeDeferRepository deferRepository
            ) : base()
        {
            _rpTailieu = tailieuBusiness;
            _rpMCredit = rpMCredit;
            _odcService = odcService;
            _bizMedia = mediaBusiness;
            _ocbBusiness = ocbBusiness;
            _deferRepository = deferRepository;
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

            var profiles = await _deferRepository.GetTempProfiles(page, limit, freeText, GlobalData.User.IDUser, status, dtFromDate, dtToDate, loaiNgay, manhom, mathanhvien = 0);
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<MiraeDeferSearchModel>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> UpdateDDEAsync(MiraeDDEEditModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            var profilerequest = await _rpMCredit.GetTemProfileByMcId(model.Id);
            var request = new MiraeDDEEditModel();
            request.Id = profilerequest.Id;
            request.Maritalstatus = model.Maritalstatus;
            request.Qualifyingyear = model.Qualifyingyear;
            request.Qualifyingyear = "0";
            request.Eduqualify = model.Eduqualify;
            request.Noofdependentin = model.Noofdependentin;
            request.Paymentchannel = model.Paymentchannel;
            request.Nationalidissuedate = model.Nationalidissuedate;
            request.Familybooknumber = model.Familybooknumber;
            request.Idissuer = model.Idissuer;
            request.Spousename = model.Spousename;
            request.Spouse_id_c = model.Spouse_id_c;
            request.Categoryid = model.Categoryid;
            request.Categoryid = "SBK";
            request.Bankname = model.Bankname;
            request.Bankbranch = model.Bankbranch;
            request.Acctype = model.Acctype;
            request.Accno = model.Accno;
            request.Dueday = model.Dueday;
            request.Notecode = "DE_MOBILE";
            request.Notedetails = model.Notedetails;
            request.UpdatedBy = GlobalData.User.IDUser;
            request.Familybooknumber = model.Familybooknumber;
            request.Spousename = model.Spousename;
            request.Spouse_id_c = model.Spouse_id_c;
            request.Notedetails = model.Notedetails;


            var result = await _rpMCredit.UpdateDDE(request);

            if (!result)
            {
                return ToJsonResponse(result, "Lỗi cập nhật");
            }
            return ToJsonResponse(true);
        }
  
        public async Task<JsonResult> SumbitToDDE(int id)
        {
            var model = await _rpMCredit.GetTemProfileByMcId(id);

            var appId = 0;
            var bankName = model.Bankname;
            try
            {
                appId = int.Parse(model.AppId);
            }
            catch (Exception)
            {
            }
            try
            {
                if (bankName.Length < 8)
                {
                    bankName = "0" + bankName;
                }
            }
            catch (Exception)
            {


            }
            var request = new MiraeDDELeadReQuest()
            {
                in_appid = appId,
                in_maritalstatus = model.Maritalstatus,
                in_qualifyingyear = model.Qualifyingyear,
                in_eduqualify = model.Eduqualify,
                in_noofdependentin = model.Noofdependentin,
                in_paymentchannel = model.Paymentchannel,
                in_nationalidissuedate = model.Nationalidissuedate.Value.ToShortDateString(),
                in_familybooknumber = "123456789",
                in_idissuer = model.Idissuer,
                in_spousename = model.Spousename,
                in_spouse_id_c = model.Spouse_id_c,
                in_categoryid = model.Categoryid,
                in_bankname = bankName,
                in_bankbranch = model.Bankbranch,
                in_acctype = model.Acctype,
                in_accno = model.Accno,
                in_dueday = model.Dueday,
                in_notecode = model.Notecode,
                in_notedetails = model.Notedetails
            };
            request.in_qualifyingyear = "0";
            request.in_notecode = "DE_MOBILE";
            request.in_channel = "SBK";
            request.in_userid = "EXT_SBK";
            request.in_categoryid = "SBK";
            var result = await _odcService.DDESubmit(request);
            if (result.Success)
            {
                var response = await _odcService.DDEToPoR(new DDEToPORReQuest()
                {
                    in_channel = "SBK",
                    in_userid = "EXT_SBK",
                    msgName = "procDDEChangeState",
                    p_appid = request.in_appid

                });
                if (response.Success)
                {
                    await _rpMCredit.UpdateStatus(id, 2, request.in_appid, GlobalData.User.IDUser);
                }
            }
            return ToJsonResponse(result.Success, "", result);

        }

        public async Task<ActionResult> Index()
        {

            await _odcService.CheckAuthen();


            return View("Temp");
        }


        public async Task<ActionResult> Mirae(int id)
        {

            var result = await _deferRepository.GetTemProfileByMcId(id);
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;
            //ViewBag.LstTaiLieu = LoaiTaiLieuModel();
            //var tailieu = new LoaiTaiLieuMiraeDeferModle()
            //{
            //    ProfileId = result.Id, 
            //    BatBuoc = 1,
            //    FileKey = result.Defer_code,

            //};

            //ViewBag.LstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList(8);

            return View("viewDDE");
        }

        public async Task<JsonResult> SendFile(int id)
        {
            int profileId = id;
            id = 19;
            var model = await _rpMCredit.GetTemProfileByMcId(profileId);
            var allTaiLieu = await _rpTailieu.GetTailieuMiraeHosoId(model.Id, 7);
            var multiForm = new MultipartFormDataContent();
            multiForm.Add(new StringContent("EXT_SBK"), "usersname");
            multiForm.Add(new StringContent("mafc123"), "password");
            multiForm.Add(new StringContent(model.AppId), "appid");
            multiForm.Add(new StringContent("EXT_SBK"), "salecode");

            multiForm.Add(new StringContent(""), "warning");
            multiForm.Add(new StringContent(""), "warning_msg");

            foreach (var item in allTaiLieu)
            {
                string filePath = Server.MapPath(item.FileUrl);
                multiForm.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)), item.Keycode, System.IO.Path.GetFileName(filePath));
            }

            var result = await _odcService.PushToUND(multiForm);

            if (result.Success)
            {
                await _rpMCredit.UpdateStatus(id, 3, int.Parse(model.AppId), GlobalData.User.IDUser);
            }
            return ToJsonResponse(result.Success, result.Data, result);

        }
     
     
        public ActionResult AddNew()

        {
            return View();
        }
        
        public async Task<JsonResult> GetAllStatus()
        {
            var result = await _rpMCredit.GetAllStatus();
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
                var deleteAll = await _rpTailieu.RemoveAllTailieuMirae(hosoId, (int)HosoType.Ocb);
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
                        await _rpTailieu.AddMirae(tailieu);
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
                        deleteURL = fileId <= 0 ? $"/Mirae/delete?key={key}" : $"/Mirae/delete/0/{fileId}";
                        if (fileId > 0)
                        {

                            await _rpTailieu.UpdateExistingFileMirae(new TaiLieu
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
            var result = await _rpTailieu.RemoveTailieuMirae(hosoId, fileId);
            return ToJsonResponse(true);
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

            var filesExist = await _rpTailieu.GetTailieuMiraeHosoId(hosoId, typeId);

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
            var result = await _rpTailieu.GetTailieuMiraeHosoId(hosoId, type);
            if (result == null)
                result = new List<FileUploadModel>();
            return ToJsonResponse(true, null, result);
        }
    }
}