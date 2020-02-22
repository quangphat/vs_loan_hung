﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.F88Model;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    [RoutePrefix("api/f88")]
    public class F88Controller : BaseApiController
    {
        protected readonly IHosoBusiness _bizHoso;
        protected readonly HttpClient _httpClient;
        public F88Controller(HttpClient httpClient ,IHosoBusiness hosoBusiness, CurrentProcess currentProcess):base(currentProcess)
        {
            _bizHoso = hosoBusiness;
            _httpClient = httpClient;
        }
        [HttpPost]
        [Route("receiveResult")]
        public async Task<IHttpActionResult> F88Result([FromBody] F88ResultModel model)
        {
            if (model == null)
                return Ok(ToResponse(false, "Dữ liệu không hợp lệ"));
            if (model.HosoId <= 0)
            {
                return Ok(ToResponse(false, "Id hồ sơ không hợp lệ"));
            }
            if (!Enum.IsDefined(typeof(VS_LOAN.Core.Entity.F88Result), model.ResultId))
            {
                return Ok(ToResponse(false, "Kết quả không hợp lệ"));
            }
            var bizHoso = new HosoBusiness();
            await _bizHoso.UpdateF88Result(model.HosoId, model.ResultId, model.Reason);
            return Ok(ToResponse(true, "Thành công"));
        }
        [HttpPost]
        [Route("test")]
        public async Task<IHttpActionResult> Test([FromBody] StringModel model)
        {
            return Ok();
        }

    }
}
