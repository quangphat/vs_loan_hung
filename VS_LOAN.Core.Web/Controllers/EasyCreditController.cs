using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    [Route("EasyCredit")]
    public class EasyCreditController : BaseController
    {
        protected readonly ITailieuBusniness _bizTailieu;
        protected readonly IECLoanBusiness _bizEc;
        public EasyCreditController(CurrentProcess currentProcess, ITailieuBusniness tailieuBusniness, IECLoanBusiness eCLoanBusiness):base(currentProcess)
        {
            _bizTailieu = tailieuBusniness;
            _bizEc = eCLoanBusiness;
        }
        // GET: EasyCredit
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Init()
        {
            return View();
        }
        [HttpPost]
        [Route("SaveInit")]
        public async Task<JsonResult> SaveInit(EcHoso model)
        {
            var result = await _bizEc.SaveEcHoso(null);
            var x = _process;
            return ToJsonResponseV2(result);
        }
        public async Task<JsonResult> GetLoaiTailieu()
        {
            var result = await _bizTailieu.GetLoaiTailieuList((int)HosoType.ECCredit);
            return ToJsonResponse(true, null, result);
        }
    }
}