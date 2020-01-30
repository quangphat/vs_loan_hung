using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class NoAuthoritiesController : LoanController
    {
        //
        // GET: /NoAuthorities/
        public NoAuthoritiesController(CurrentProcess currentProcess) : base(currentProcess)
        {

        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
