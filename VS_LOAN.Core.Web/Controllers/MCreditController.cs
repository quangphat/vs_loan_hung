using MCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MCreditController : BaseController
    {
        protected readonly IMCeditBusiness _bizMCredit;
        protected readonly IMCreditService _svMCredit;
        public MCreditController(IMCeditBusiness mCeditBusiness, IMCreditService loanContractService)
        {
            _bizMCredit = mCeditBusiness;
            _svMCredit = loanContractService;
        }
        public ActionResult CheckCat()
        {
            return View();
        }
        public async Task<JsonResult> CheckCatApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
           var result = await _svMCredit.CheckCat(GlobalData.User.IDUser,model.Value);
            return ToJsonResponse(true, result.msg, result);
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
            return ToJsonResponse(true, result.msg, result);
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
            return ToJsonResponse(true, result.msg, result);
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
            return ToJsonResponse(true, result.msg, result);
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
    }
}