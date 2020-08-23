using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;
using McreditServiceCore;
using McreditServiceCore.Interfaces;
using VietStar.Entities.Commons;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class MCreditController : VietStarBaseController
    {
        protected readonly IMCreditBusiness _bizMCredit;
        protected readonly IMCreditService _svMcredit;
        public MCreditController(IMCreditBusiness mcreditBusiness,
            IMCreditService mCreditService,
            CurrentProcess process) : base(process)
        {
            _bizMCredit = mcreditBusiness;
            _svMcredit = mCreditService;
        }

        public async Task<IActionResult> AuthenMC(AuthenMcRequestModel model)
        {
            if (model == null)
                return ToResponse(false);
            var result = await _svMcredit.AuthenByUserId(model.UserId, model.TableToUpdateIds);
            return ToResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSaleApi([FromBody]CheckSaleModel model)
        {
            var result = await _bizMCredit.CheckSaleAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckCat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckCatApi([FromBody]StringModel model)
        {
            var result = await _bizMCredit.CheckCatAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckCIC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckCICApi([FromBody]CheckCICModel model)
        {

            var result = await _bizMCredit.CheckCICAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckDuplicate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckDupApi([FromBody]StringModel model)
        {

            var result = await _bizMCredit.CheckDupAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckStatus()
        {
            return View();
        }

        public async Task<IActionResult> CheckStatusApi(StringModel model)
        {
            var result = await _bizMCredit.CheckStatusAsync(model);
            return ToResponse(result);
        }

        public IActionResult Temp()
        {
            return View();
        }

        public async Task<IActionResult> SearchTemps(string freeText, string status, int page = 1, int limit = 20)
        {
            var result = await _bizMCredit.SearchsTemsAsync(freeText, status, page, limit);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcredit-profile")]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcredit-profile")]
        public async Task<IActionResult> Search(string freeText, string status, string type, int page = 1)
        {
            var result = await _bizMCredit.SearchsAsync(freeText, status, type, page);
            return ToResponse(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateDraft([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.CreateDraftAsync(model);
            return ToResponse(result);
        }

        public async Task<IActionResult> AddRefuseReasonToNote(int profileId, string type)
        {
            var result = await _bizMCredit.AddRefuseReasonToNoteAsync(profileId, type);
            return ToResponse(result);

        }

        public async Task<IActionResult> UpdateTempProfileStatus(int profileId)
        {
            var result = await _bizMCredit.UpdateTempProfileStatusAsync(profileId);
            return ToResponse(result);
        }

        public async Task<IActionResult> UpdateDraft([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.UpdateDraftAsync(model);
            return ToResponse(result);
        }

        public async Task<IActionResult> TempProfile(int id)
        {
            var result = await _bizMCredit.GetTempProfileByIdAsync(id);
            return View(result);
        }
        public async Task<IActionResult> GetMCSimpleList(string type)
        {
            var result = await _bizMCredit.GetSimpleListByTypeAsync(type);
            return ToResponse(result);
        }

        public async Task<IActionResult> ReSendFileToEC(int mcProfileId)
        {
            var result = await _bizMCredit.ReSendFileToECAsync(mcProfileId);
            return ToResponse(result);
        }

        public async Task<IActionResult> GetNotes(int mcProfileId)
        {
            var result = await _bizMCredit.GetNotesAsync(mcProfileId);
            return ToResponse(result);
        }
        public async Task<IActionResult> AddNoteNotes(string mcId, [FromBody]StringModel model)
        {
            var result = await _bizMCredit.AddNoteToMcAsync(mcId, model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcredit-submit")]
        public async Task<IActionResult> SubmitToMCredit([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.SubmitToMCreditAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcredit-profile")]
        public async Task<ActionResult> MCreditProfile(int id)
        {
            var result = await _bizMCredit.GetMCreditProfileByIdAsync(id);
            return View(result);
        }
    }
}