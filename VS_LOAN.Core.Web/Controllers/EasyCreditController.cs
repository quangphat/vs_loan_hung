using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class EasyCreditController : BaseController
    {
        public EasyCreditController(CurrentProcess currentProcess):base(currentProcess)
        {

        }
        // GET: EasyCredit
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Init()
        {
            return View();
        }
    }
}