using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;

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
        public async Task<IActionResult> Search(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , int page = 1
            , int limit = 20
            , string sort ="desc"
            , string sortField = "updatedTime")
        {
            //return ToResponse(new List<ProfileIndexModel> { new ProfileIndexModel { TotalRecord = 100 } });
            var result = await _bizProfile.GetsAsync(fromDate, toDate, dateType, groupId, memberId, status, freeText,sort, sortField, page, limit);

            return ToResponse(result);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost("profile/CreateAsync")]
        public async Task<IActionResult> CreateAsync(ProfileAdd model)
        {
            var result = await _bizProfile.CreateAsync(model);
            return ToResponse(result);
        }
    }
}