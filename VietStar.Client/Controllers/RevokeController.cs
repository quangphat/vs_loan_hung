using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Collection;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class RevokeController : VietStarBaseController
    {
        protected readonly IRevokeDebtBusiness _bizRevoke;
        public RevokeController(IRevokeDebtBusiness revokeBusiness,CurrentProcess process) : base(process)
        {
            _bizRevoke = revokeBusiness;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string freeText = null,
            string status = null, 
            int groupId = 0,
            int assigneeId = 0,
            int page = 1, 
            int limit = 10,
            DateTime? fromDate = null,
            DateTime? toDate = null, 
            int dateType = 1,
            int processStatus = -1)
        {

           
            var result = await _bizRevoke.SearchAsync(freeText, status, page, limit, groupId, assigneeId, fromDate, toDate, dateType, processStatus);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions ="import-revoke")]
        [HttpPost("revoke/import")]
        public async Task<IActionResult> Import()
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizRevoke.InsertFromFileAsync(file);
            return ToResponse(result);
        }

        [HttpGet("revoke/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _bizRevoke.GetByIdAsync(id);
            return View(profile);
        }

        [HttpPut("revoke/update/{profileId}")]
        public async Task<IActionResult> Update(int profileId,[FromBody] RevokeSimpleUpdate model)
        {

            var result = await _bizRevoke.UpdateSimpleAsync(model, profileId);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "revoke.delete")]
        [HttpDelete("revoke/{profileId}")]
        public async Task<IActionResult> Delete(int profileId)
        {
            
            var result = await _bizRevoke.DeleteByIdAsync(profileId);
            return ToResponse(result);
        }
    }
}