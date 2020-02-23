using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class EasyCreditController : BaseController
    {
        protected readonly ITailieuBusniness _bizTailieu;
        public EasyCreditController(CurrentProcess currentProcess, ITailieuBusniness tailieuBusniness):base(currentProcess)
        {
            _bizTailieu = tailieuBusniness;
        }
        // GET: EasyCredit
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Init()
        {
            var tailieus = await _bizTailieu.GetLoaiTailieuList((int)HosoType.ECCredit);
            return View();
        }
    }
}