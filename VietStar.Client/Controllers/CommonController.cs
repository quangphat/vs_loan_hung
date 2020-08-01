using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Route("Common")]
    public class CommonController : VietStarBaseController
    {
        protected readonly ICommonBusiness _bizCommon;
        public CommonController(CurrentProcess process, ICommonBusiness commonBusiness) : base(process)
        {
            _bizCommon = commonBusiness;
        }
        [HttpGet("GetStatusList/{profileType}")]
        public async Task<IActionResult> GetStatusList(string profileType)
        {
            var result = await _bizCommon.GetStatusList(profileType);
            return ToResponse(result);
        }
    }
}