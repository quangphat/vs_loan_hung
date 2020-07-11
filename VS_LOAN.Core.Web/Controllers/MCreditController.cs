using MCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MCreditController : BaseController
    {
        protected readonly IMCeditRepository _bizMCredit;
        protected readonly IMCreditService _svMCredit;
        public MCreditController(IMCeditRepository mCeditBusiness, IMCreditService loanContractService) : base()
        {
            _bizMCredit = mCeditBusiness;
            _svMCredit = loanContractService;
        }
        public async Task<JsonResult> AuthenMC(AuthenMCModel model)
        {
            if (model == null)
                return ToJsonResponse(false);
            var result = await _svMCredit.AuthenByUserId(model.UserId, model.IsUpdateToken, model.IsUpdateProduct, model.IsUpdateLoanPeriod, model.IsUpdateLocation, model.IsUpdateCity);
            return ToJsonResponse(true, result, result);
        }
        public async Task<JsonResult> CheckSaleApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckSale(GlobalData.User.IDUser, model.Value);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckCat()
        {
            return View();
        }
        public async Task<JsonResult> CheckCatApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckCat(GlobalData.User.IDUser, model.Value);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckCIC()
        {
            return View();
        }
        public async Task<JsonResult> CheckCICApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckCIC(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckDuplicate()
        {
            return View();
        }
        public async Task<JsonResult> CheckDupApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckDup(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckStatus()
        {
            return View();
        }
        public async Task<JsonResult> CheckStatusApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckStatus(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText, string status, string type, int page)
        {
            var result = await _svMCredit.SearchProfiles(freeText, status, type, page, GlobalData.User.IDUser);
            return ToJsonResponse(true, "", result);
        }
        public ActionResult AddNew()
        {
            ViewBag.User = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> SaveDraft(MCredit_TempProfileAddModel model)
        {
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.CreatedBy = GlobalData.User.IDUser;
            var id = await _bizMCredit.CreateProfile(profile);
            if(id>0)
            {
                var obj = _mapper.Map<MCProfilePostModel>(profile);
                var result = await _svMCredit.CreateProfile(obj, GlobalData.User.IDUser);
                return ToJsonResponse(true, result.message, result);
            }
            return ToJsonResponse(true, "", id);
        }
        public async Task<JsonResult> Create(MCredit_TempProfileAddModel model)
        {

            //var hoso = new HoSoBLL().LayChiTiet(Convert.ToInt32(model.Value));
            //var obj = _mapper.Map<ProfileAddObj>(hoso);
            //obj.cityId = "58";
            //obj.homeTown = "Bình Phước";
            //obj.isAddr = true;
            //obj.loanPeriodCode = "3";
            //obj.isInsurance = false;
            //obj.logSignCode = "28";
            //obj.productCode = "C0000011";
            //obj.saleID = "VBF0265";
            //var result = await _svMCredit.CreateProfile(model, 3514);
            return ToJsonResponse(true, "");
        }
        public async Task<JsonResult> GetMCSimpleList(int type)
        {
            var result = new List<OptionSimple>();
            if (type == (int)MCTableType.MCreditCity)
            {
                result = await _bizMCredit.GetMCCitiesSimpleList();
            }
            if (type == (int)MCTableType.MCreditLoanPeriod)
            {
                result = await _bizMCredit.GetMCLoanPerodSimpleList();
            }
            if (type == (int)MCTableType.MCreditlocations)
            {
                result = await _bizMCredit.GetMCLocationSimpleList();
            }
            if (type == (int)MCTableType.MCreditProduct)
            {
                result = await _bizMCredit.GetMCProductSimpleList();
            }
            return ToJsonResponse(true, "", result);
        }
    }
}