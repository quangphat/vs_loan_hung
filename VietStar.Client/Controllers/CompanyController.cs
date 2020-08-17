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
    public class CompanyController : VietStarBaseController
    {
        protected readonly ICompanyBusiness _bizCompany;
        public CompanyController(ICompanyBusiness companyBusiness, CurrentProcess process) : base(process)
        {
            _bizCompany = companyBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var result = await _bizCompany.SearchsAsync(freeText, page, limit);
            return ToResponse(result);
        }
    }
}