using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Courier;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class CourierController : VietStarBaseController
    {
        protected readonly ICourierBusiness _bizCourier;
        public CourierController(ICourierBusiness courierBusiness, CurrentProcess process) : base(process)
        {
            _bizCourier = courierBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string freeText = null
            , int provinceId = 0
            , int courierId = 0
            , string status = null
            , int groupId = 0
            , int page = 1
            , int limit = 10
            , string salecode = null)
        {
            var result = await _bizCourier.GetsAsync(freeText, courierId, status, page, limit, groupId, provinceId, salecode);
            return ToResponse(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateAsync([FromBody] CourierAddModel model)
        {
            var result = await _bizCourier.CreateAsync(model);
            return ToResponse(result);

        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _bizCourier.GetByIdAsync(id);
            ViewBag.isAdmin = _process.User.Rolecode == "admin" ? true : false;
            return View(model);
        }

        public async Task<IActionResult> UpdateAsync([FromBody] CourierUpdateModel model)
        {
            var result = await _bizCourier.UpdateAsync(model);
            return ToResponse(result);
        }
    }
}