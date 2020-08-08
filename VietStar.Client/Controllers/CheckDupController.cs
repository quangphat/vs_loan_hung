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
    public class CheckDupController : VietStarBaseController
    {
        protected readonly ICheckDupBusiness _bizCheckDup;
        public CheckDupController(ICheckDupBusiness checkdupBusiness,CurrentProcess process) : base(process)
        {
            _bizCheckDup = checkdupBusiness;
        }
        [MyAuthorize(Permissions ="profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}