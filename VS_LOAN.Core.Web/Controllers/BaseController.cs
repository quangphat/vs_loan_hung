using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.Model;
namespace VS_LOAN.Core.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMapper _mapper;
        protected BaseController()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<HoSoInfoModel,ProfileAddObj>()
                .ForMember(a => a.name, b => b.MapFrom(c=>c.TenKhachHang))
                .ForMember(a => a.cityId, b => b.MapFrom(c => c.MaKhuVuc))
                .ForMember(a => a.bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
                .ForMember(a => a.phone, b => b.MapFrom(c => c.SDT))
                .ForMember(a => a.productCode, b => b.MapFrom(c => c.ProductCode))
                .ForMember(a => a.idNumber, b => b.MapFrom(c => c.CMND))
                .ForMember(a => a.idNumberDate, b => b.MapFrom(c => c.CmndDay.ToShortDateString()))
                //.ForMember(a => a.loanPeriodCode, b => b.MapFrom(c => c.CMND))
                .ForMember(a => a.loanMoney, b => b.MapFrom(c => c.SoTienVay.ToString()))
                //.ForMember(a => a.saleID, b => b.MapFrom(c => c.CMND))
                ;
                
            });

            _mapper = config.CreateMapper();
        }
        public ActionResult ToResponse(bool success = true, string message = null, object data = null)
        {
            string code = string.Empty;
            if(success)
            {

                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            return Json(new { data, success, code }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult ToJsonResponse(bool success = true, string message = null, object data = null)
        {
            string code = string.Empty;
            if (success)
            {

                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
            }
            else
            {
                code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
            }
            return Json(new { data, success, code }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ToJsonResponse(bool success, string message = null, object data = null)
        //{
        //    var result = new RMessage();
        //    if (success)
        //    {

        //        result.code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Succ : message;
        //    }
        //    else
        //    {
        //        result.code = string.IsNullOrWhiteSpace(message) ? Resources.Global.Message_Error : message;
        //    }
        //    result.success = success;
        //    result.data = data;
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}
    }
    public class BaseApiController : ApiController
    {
        protected JsonnResponseModel ToResponse(bool success = true, string message = null)
        {
            return new JsonnResponseModel { Success = success, Message = message };
        }
    }
    public class JsonnResponseModel

    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}