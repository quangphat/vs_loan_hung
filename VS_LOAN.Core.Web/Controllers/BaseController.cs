using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
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
                x.CreateMap<HoSoInfoModel, ProfileAddObj>()
                .ForMember(a => a.name, b => b.MapFrom(c => c.TenKhachHang))
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
                x.CreateMap<MCredit_TempProfileAddModel, MCredit_TempProfile>();
                x.CreateMap<MCredit_TempProfile, MCProfilePostModel>()
                .ForMember(a=>a.Name,b=>b.MapFrom(c=>c.CustomerName))
                .ForMember(a => a.HomeTown, b => b.MapFrom(c => c.Hometown))
                .ForMember(a => a.Bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
                .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
                .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
                .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CCCDNumber))
                .ForMember(a => a.IdNumberDate, b => b.MapFrom(c => c.IssueDate.ToShortDateString()))
                .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddr))
                .ForMember(a => a.CityId, b => b.MapFrom(c => c.ProvinceId))
                .ForMember(a => a.ProductCode, b => b.MapFrom(c => c.ProductCode))
                .ForMember(a => a.LoanPeriodCode, b => b.MapFrom(c => c.LoanPeriodCode))
                .ForMember(a => a.LoanMoney, b => b.MapFrom(c => c.LoanMoney.ToString()))
                .ForMember(a => a.LocSignCode, b => b.MapFrom(c => c.LocSignCode))
                .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.IsInsurrance))
                .ForMember(a => a.SaleId, b => b.MapFrom(c => c.SaleId))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
                ;
                x.CreateMap<MCredit_TempProfileAddModel, MCProfilePostModel>()
               .ForMember(a => a.Name, b => b.MapFrom(c => c.CustomerName))
               .ForMember(a => a.HomeTown, b => b.MapFrom(c => c.Hometown))
               .ForMember(a => a.Bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
               .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
               .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
               .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CCCDNumber))
               .ForMember(a => a.IdNumberDate, b => b.MapFrom(c => c.IssueDate.ToShortDateString()))
               .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddr))
               .ForMember(a => a.CityId, b => b.MapFrom(c => c.ProvinceId))
               .ForMember(a => a.ProductCode, b => b.MapFrom(c => c.ProductCode))
               .ForMember(a => a.LoanPeriodCode, b => b.MapFrom(c => c.LoanPeriodCode))
               .ForMember(a => a.LoanMoney, b => b.MapFrom(c => c.LoanMoney.ToString()))
               .ForMember(a => a.LocSignCode, b => b.MapFrom(c => c.LocSignCode))
               .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.IsInsurrance))
               .ForMember(a => a.SaleId, b => b.MapFrom(c => c.SaleId))
               .ForMember(a => a.Status, b => b.MapFrom(c => c.Status));
                x.CreateMap<CheckSaleObj, UpdateSaleModel>()
               .ForMember(a => a.SaleName, b => b.MapFrom(c => c.name))
               .ForMember(a => a.SaleNumber, b => b.MapFrom(c => c.idNumber))
               .ForMember(a => a.SaleId, b => b.MapFrom(c => c.id));

                x.CreateMap<ProfileGetByIdResponseObj, MCredit_TempProfile>()
              .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Name))
              .ForMember(a => a.MCId, b => b.MapFrom(c => c.Id))
              .ForMember(a => a.Hometown, b => b.MapFrom(c => c.HomeTown))
              .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.Bod))
              .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
              .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
              .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CccdNumber))
              .ForMember(a => a.IssueDate, b => b.MapFrom(c => c.IdDate))
              .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddrSame))
              .ForMember(a => a.ProvinceId ,b => b.MapFrom(c => c.CityId))
              .ForMember(a => a.Address, b => b.MapFrom(c => c.LocSignAddr))
              .ForMember(a => a.Hometown, b => b.MapFrom(c => c.HomeTown))
              ;
            });

            _mapper = config.CreateMapper();
        }
        public ActionResult ToResponse(bool success = true, string message = null, object data = null)
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