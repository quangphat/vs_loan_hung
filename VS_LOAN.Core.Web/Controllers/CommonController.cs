using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CommonController : BaseController
    {
        protected readonly INoteRepository _rpNote;
        protected readonly ICommonRepository _rpCommon;
        public CommonController(INoteRepository noteRepository, ICommonRepository commonRepository):base()
        {
            _rpNote = noteRepository;
            _rpCommon = commonRepository;
        }
        public async Task<JsonResult> Comments(int profileId, int type)
        {
            var result = await _rpNote.GetNoteByTypeAsync(profileId, type);
            return ToJsonResponse(true, "", data: result);
        }
        public async Task<JsonResult> ProfileStatus()
        {
            var result = await _rpCommon.GetProfileStatusByCode(Constanst.revoke_debt_max_row_import, GlobalData.User.OrgId, roleId: GlobalData.User.RoleId);
            return ToJsonResponse(true, "", data: result);
        }
        public FileResult DownloadTemplateFile(string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\TemplateReport\\" + fileName);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = fileName;
            return response;
        }
        
    }
}