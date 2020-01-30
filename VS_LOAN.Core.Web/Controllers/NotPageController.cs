
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class NotPageController : LoanController
    {
        public NotPageController(CurrentProcess currentProcess) : base(currentProcess)
        {

        }
        //
        // GET: /NotPage/
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View("Index");
        }
    }
}
