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
        public static ProvinceResponseModel _provinceResponseModel;
        public OcbController(IOcbRepository rpMCredit , IOdcService odcService) : base()
        {

            _rpMCredit = rpMCredit;
            _odcService = odcService;


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
            profile.idIssueDate = "";
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



            return ToJsonResponse(true, "", id);

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
            profile.idIssueDate = "";
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
            //var peopleCanView = await _rpMCredit.GetPeopleCanViewMyProfile(id);
            //if (peopleCanView != null && peopleCanView.Any())
            //{
            //    if (!peopleCanView.Contains(GlobalData.User.IDUser))
            //        return RedirectToAction("Temp");
            //}
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


        public ActionResult Temp()
        {
             _odcService.GetAllCity(new CityRequestModel());
             _odcService.GetAllProvince();
            _odcService.GetAllWard(new WardRequestModel());
            _odcService.GetAllDictionary();
            return View();
        }


       
        public ActionResult AddNew()

        {
            return View();
        }

       
  

        
    }
}