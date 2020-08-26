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
    [Route("employees")]
    [Authorize]
    public class EmployeeController : VietStarBaseController
    {
        protected readonly IEmployeeBusiness _bizEmployee;
        public EmployeeController(IEmployeeBusiness employeeBusiness,CurrentProcess process) : base(process)
        {
            _bizEmployee = employeeBusiness;
        }
        
        [HttpGet("GetByGroupId/{groupId}")]
        public async Task<IActionResult> GetByGroupId(int groupId)
        {
            var result = await _bizEmployee.GetMemberByGroupIdAsync(groupId);
            return ToResponse(result);
        }

        [HttpGet("GetByProvinceId/{provinceId}")]
        public async Task<IActionResult> GetByProvinceId(int provinceId)
        {
            var result = await _bizEmployee.GetByProvinceIdAsync(provinceId);
            return ToResponse(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllEmployeePaging(string freeText, int page = 1)
        {
            var result = await _bizEmployee.GetAllEmployeePagingAsync( page, freeText);
            return ToResponse(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployeePaging()
        {
            var result = await _bizEmployee.GetAllEmployeeAsync();
            return ToResponse(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _bizEmployee.GetRoleList();
            return ToResponse(result);
        }

        [HttpGet("Index")]
        [MyAuthorize(Permissions ="employee")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("search")]
        [MyAuthorize(Permissions = "employee")]
        public async Task<IActionResult> Search(string freetext = "",
            int roleId = 0,
            int page = 1, int limit = 10)
        {
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            var datas = await _bizEmployee.SearchsAsync( roleId, freetext, page, limit);
            return ToResponse(datas);
        }

    }
}