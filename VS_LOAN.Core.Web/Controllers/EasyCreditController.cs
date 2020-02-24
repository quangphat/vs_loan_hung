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
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Web.Controllers
{
    [Route("EasyCredit")]
    public class EasyCreditController : BaseController
    {
        protected readonly ITailieuBusniness _bizTailieu;
        protected readonly IECLoanBusiness _bizEc;
        protected readonly IEcLocationBusiness _bizEcLocation;
        protected readonly IEcEmploymentBusiness _bizEmployment;
        protected readonly IEcProductBusiness _bizEcProduct;
        public EasyCreditController(CurrentProcess currentProcess, 
            ITailieuBusniness tailieuBusniness, 
            IECLoanBusiness eCLoanBusiness,
            IEcLocationBusiness ecLocationBusiness,
            IEcEmploymentBusiness ecEmploymentBusiness,
            IEcProductBusiness ecProductBusiness):base(currentProcess)
        {
            _bizTailieu = tailieuBusniness;
            _bizEc = eCLoanBusiness;
            _bizEcLocation = ecLocationBusiness;
            _bizEmployment = ecEmploymentBusiness;
            _bizEcProduct = ecProductBusiness;
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
            var result = await _bizEc.SaveEcHosoStep1(model);
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
        [HttpGet]
        public async Task<JsonResult> GetEcEmploymentType(string type)
        {
            var result = await _bizEmployment.GetEmployment(type);
            return ToJsonResponseV2(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetEcProduct(string code)
        {
            var result = await _bizEcProduct.GetSimples(code);
            return ToJsonResponseV2(result);
        }
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            var result = await _bizTailieu.UploadFile(hosoId, (int)HosoType.ECCredit, isReset, filesGroup);
            return ToJsonResponseV2(result);
        }
    }
}