using MCreditService.Interfaces;
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

        protected readonly IOcbRepository _rpMCredit;

        protected readonly IOdcService _odcService;
        public readonly IOcbBusiness _ocbBusiness;
        public static ProvinceResponseModel _provinceResponseModel;
        public OcbController(IOcbRepository rpMCredit , IOdcService odcService, IOcbBusiness ocbBusiness) : base()
        {

            _rpMCredit = rpMCredit;
            _odcService = odcService;
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
            var id = await _rpMCredit.CreateDraftProfile(profile);
            if (id > 0)
            {
               
            }
            return ToJsonResponse(true, "", id);
        }

           public async Task<ActionResult> OcbProfile(int id)
        {
            var result = await _rpMCredit.GetTemProfileByMcId(id);
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;         
            return View();
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

    }
}