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
    public class EmployeeController : VietStarBaseController
    {
        protected readonly IEmployeeBusiness _bizEmployee;
        public EmployeeController(IEmployeeBusiness employeeBusiness,CurrentProcess process) : base(process)
        {
            _bizEmployee = employeeBusiness;
        }
        [Authorize]
        [HttpGet("GetByGroupId/{groupId}")]
        public async Task<IActionResult> GetByGroupId(int groupId)
        {
            var result = await _bizEmployee.GetMemberByGroupId(groupId);
            return ToResponse(result);
        }
    }
}