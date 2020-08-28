using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
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
    }
}