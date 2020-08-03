using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Route("Common")]
    public class CommonController : VietStarBaseController
    {
        protected readonly ICommonBusiness _bizCommon;
        protected readonly IHostingEnvironment _hosting;
        public CommonController(CurrentProcess process, ICommonBusiness commonBusiness,IHostingEnvironment hosting) : base(process)
        {
            _bizCommon = commonBusiness;
            _hosting = hosting;
        }
        [HttpGet("GetStatusList/{profileType}")]
        public async Task<IActionResult> GetStatusList(string profileType)
        {
            var result = await _bizCommon.GetStatusList(profileType);
            return ToResponse(result);
        }
        [HttpGet("GetFileType/{profileType}")]
        public async Task<IActionResult> GetFileType(string profileType)
        {
            var result = await _bizCommon.GetProfileFileTypeByType(profileType);
            return ToResponse(result);
        }
        [HttpPost("UploadFile/{key}/{fileType}/{fileId}")]
        public async Task<IActionResult> Upload(int key, int fileType,int fileId = 0)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizCommon.UploadFile(file,key,fileId,fileType,_hosting.ContentRootPath);
            return Json(result);
        }
        [HttpGet("partners")]
        public async Task<IActionResult> GetPartners()
        {
            var result = await _bizCommon.GetPartnersAsync();
            return ToResponse(result);
        }
        [HttpGet("products/{partnerId}")]
        public async Task<IActionResult> GetProducts(int partnerId)
        {
            var result = await _bizCommon.GetProductsAsync(partnerId);
            return ToResponse(result);
        }
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var result = await _bizCommon.GetProvincesAsync();
            return ToResponse(result);
        }
        [HttpGet("districts/{provinceId}")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var result = await _bizCommon.GetDistrictsAsync(provinceId);
            return ToResponse(result);
        }
        [HttpGet("sales")]
        public async Task<IActionResult> GetSales()
        {
            var result = await _bizCommon.GetSalesAsync();
            return ToResponse(result);
        }
        [HttpGet("couriers")]
        public async Task<IActionResult> GetCouriers()
        {
            var result = await _bizCommon.GetCouriersAsync();
            return ToResponse(result);
        }
    }
}