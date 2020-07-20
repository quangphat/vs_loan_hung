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
    public class ProfileController : VietStarBaseController
    {
        protected readonly IProfileBusiness _bizProfile;
        public ProfileController(IProfileBusiness profileBusiness,CurrentProcess process) : base(process)
        {
            _bizProfile = profileBusiness;
        }
        [MyAuthorize(Permissions ="profile,profile.view")]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(DateTime? fromDate, DateTime? toDate, int dateType = 1, int groupId = 0, int memberId = 0, string status = null, string freeText = null, int page = 1, int limit = 20)
        {
            var result = await _bizProfile.Gets(fromDate, toDate, dateType, groupId, memberId, status, freeText, page, limit);
            return ToResponse(result);
        }
    }
}