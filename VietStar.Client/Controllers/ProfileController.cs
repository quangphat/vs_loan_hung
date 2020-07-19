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
        [MyAuthorize(Permissions ="profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}