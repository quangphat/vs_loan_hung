using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace VS_LOAN.Core.Web.Controllers
{
    [Route("api/f88/")]
    public class F88Controller : ApiController
    {
        [HttpPost]
        [Route("result")]
        public async Task<IHttpActionResult> F88Result()
        {
            return Ok();
        }
    }
}
