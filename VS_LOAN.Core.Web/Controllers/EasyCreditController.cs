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
        protected readonly IEcLocationBusiness _bizEcLocation;
        public EasyCreditController(CurrentProcess currentProcess, 
            ITailieuBusniness tailieuBusniness, 
            IECLoanBusiness eCLoanBusiness,
            IEcLocationBusiness ecLocationBusiness):base(currentProcess)
        {
            _bizTailieu = tailieuBusniness;
            _bizEc = eCLoanBusiness;
            _bizEcLocation = ecLocationBusiness;
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

        [HttpGet]
        [Route("eclocation/{type}")]
        public async Task<JsonResult> GetEcLocation(int type,int id = 0)
        {
            var result = await _bizEcLocation.GetLocation(type, id);
            return ToJsonResponseV2(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetEcIssuePlace()
        {
            var result = await _bizEcLocation.GetIssuePlace();
            return ToJsonResponseV2(result);
        }
    }
}