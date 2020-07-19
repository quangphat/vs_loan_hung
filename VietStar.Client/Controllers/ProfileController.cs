using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    public class ProfileController : VietStarBaseController
    {
        public ProfileController(CurrentProcess process) : base(process)
        {
        }
        [MyAuthorize(Permissions ="profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}