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
    [Route("Groups")]
    [Authorize]
    public class GroupController : VietStarBaseController
    {
        protected readonly IGroupBusiness _bizGroup;
        public GroupController(IGroupBusiness groupBusiness,CurrentProcess process) : base(process)
        {
            _bizGroup = groupBusiness;
        }
        
        [HttpGet("GroupsByUserId")]
        public async Task<IActionResult> GetGroupsByUserId()
        {
            var result = await _bizGroup.GetGroupByUserId();
            return ToResponse(result);
        }
        
        [HttpGet("ApproveGroupsByUserId")]
        public async Task<IActionResult> GetApproveGroupsByUserId()
        {
            var result = await _bizGroup.GetApproveGroupByUserId();
            return ToResponse(result);
        }

        [HttpGet("ParentGroups")]
        public async Task<IActionResult> GetParentGroups()
        {
            var result = await _bizGroup.GetParentGroups();
            return ToResponse(result);
        }
    }
}