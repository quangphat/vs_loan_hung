using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class RevokeController : BaseController
    {
        protected readonly IRevokeDebtBusiness _bizRevokeDebt;
        public RevokeController(IRevokeDebtBusiness revokeDebtBusiness) : base()
        {
            _bizRevokeDebt = revokeDebtBusiness;
        }
        // GET: Revoke
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, string status = null, int groupId = 0, int page = 1, int limit = 10)
        {
            var result = await _bizRevokeDebt.SearchAsync(GlobalData.User.IDUser, freeText, status, page, limit, groupId);
            return ToJsonResponse(true, null, result);
        }
        public async Task<JsonResult> Import()
        {
            var file = Request.Files[0];
            if (file == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            Stream stream = file.InputStream;
            stream.Position = 0;
            using (var fileStream = new MemoryStream())
            {
                await stream.CopyToAsync(fileStream);
                var results = await _bizRevokeDebt.InsertFromFileAsync(fileStream, GlobalData.User.IDUser);
                return ToJsonResponse(results.IsSuccess, results.Message, results.Data);
            }

        }
        public async Task<ActionResult> Edit(int id)
        {
            var profile = await _bizRevokeDebt.GetByIdAsync(id, GlobalData.User.IDUser);
            ViewBag.model = profile;
            return View();
        }
        public async Task<JsonResult> Delete(int profileId)
        {
            if (GlobalData.User.RoleId == (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Vui lòng liên hệ admin");
            await _bizRevokeDebt.DeleteByIdAsync(GlobalData.User.IDUser, profileId);
            return ToJsonResponse(true);
        }
    }
}