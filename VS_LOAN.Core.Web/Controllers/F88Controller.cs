using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity.F88Model;

namespace VS_LOAN.Core.Web.Controllers
{
    //[Route("api/f88")]
    public class F88Controller : BaseApiController
    {
        [HttpPost]
        [Route("api/F88Result")]
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
            await bizHoso.UpdateF88Result(model.HosoId, model.ResultId, model.Reason);
            return Ok(ToResponse(true, "Thành công"));
        }
        [HttpPost]
        [Route("result2")]
        public async Task<IHttpActionResult> F88Result2([FromBody] F88ResultModel model)
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
            await bizHoso.UpdateF88Result(model.HosoId, model.ResultId, model.Reason);
            return Ok(ToResponse(true, "Thành công"));
        }
    }
}
