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
    public class MiraeStatusController : BaseController
    {

        protected readonly IMiraeRepository _rpMCredit;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IMiraeService _odcService;
        public readonly IOcbBusiness _ocbBusiness;
        public readonly IMiraeMaratialRepository _rpTailieu;
        public static ProvinceResponseModel _provinceResponseModel;
        public MiraeStatusController(IMiraeRepository rpMCredit ,
              IMediaBusiness mediaBusiness,
            IMiraeService odcService, IOcbBusiness ocbBusiness, IMiraeMaratialRepository tailieuBusiness) : base()
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

            DateTime dtFromDate = DateTime.Now.AddYears(-2);
            DateTime dtToDate = DateTime.Now.AddDays(3);
            if (fromDate != "")
                dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
            if (toDate != "")
                dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

            var profiles = await _rpMCredit.GetTempProfiles(page, limit, freeText, GlobalData.User.IDUser, status, dtFromDate, dtToDate, loaiNgay, manhom, mathanhvien );
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<MiraeModelSearchModel>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }
           public async Task<ActionResult> Detail(int id)
        {
            var result = await _rpMCredit.GetTemProfileByMcId(id);
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;
            ViewBag.LstTaiLieu = new List<TaiLieuModel>();
            ViewBag.LstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList(8);
            return View ("~/Views/Mirae/ViewDetailStatus.cshtml");
        }
    
        public async Task<JsonResult> SendFile(int id)
        {
            int profileId = id;
            var model = await _rpMCredit.GetTemProfileByMcId(profileId);
            var allTaiLieu = await _rpTailieu.GetTailieuMiraeHosoId(model.Id, 7);
            var multiForm = new MultipartFormDataContent();
            multiForm.Add(new StringContent("EXT_SBK"), "usersname");
            multiForm.Add(new StringContent("mafc123"), "password");
            multiForm.Add(new StringContent(model.AppId), "appid");
            multiForm.Add(new StringContent("EXT_SBK"), "salecode");
            multiForm.Add(new StringContent(""), "warning");
            multiForm.Add(new StringContent(""), "warning_msg");
            if (allTaiLieu.Count < 6)
            {
                return ToJsonResponse(false, "Ít nhất phải là 6 chứng từ bắt buộc", new PushToUNDReponse()
                {
                    Data = "Ít nhất phải là 6 chứng từ bắt buộc",
                    Message = "Ít nhất phải là 6 chứng từ bắt buộc",
                    Success = false
                });
            }

          
            foreach (var item in allTaiLieu)
            {

                string filePath = Server.MapPath(item.FileUrl);
                multiForm.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)),item.Keycode , System.IO.Path.GetFileName(filePath));
            }
            var result =  await _odcService.PushToUND(multiForm);
            if (result.Success)
            {
                await _rpMCredit.UpdateStatus(model.Id,3, int.Parse(model.AppId), GlobalData.User.IDUser);
            }
            return ToJsonResponse(result.Success,result.Data,result);

        }
        public async Task<ActionResult> Temp()
        {

          await _odcService.CheckAuthen();


            return View();
        }
      
        public async Task<ActionResult> Index()
        {
            await _odcService.CheckAuthen();

            return View("miraeStatus");
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
       
       


    }
}