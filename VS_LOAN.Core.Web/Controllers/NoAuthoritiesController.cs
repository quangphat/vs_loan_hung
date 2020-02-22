using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Web.Controllers
{
    public class NoAuthoritiesController : BaseController
    {
        //
        // GET: /NoAuthorities/

        public ActionResult Index()
        {
            return View();
        }

    }
}
