using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Web.Controllers
{
    public class RevokeController : BaseController
    {

        public RevokeController():base()
        {

        }
        // GET: Revoke
        public ActionResult Index()
        {
            return View();
        }
    }
}