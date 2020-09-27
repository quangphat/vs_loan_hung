using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.CheckDup;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CustomerController : BaseController
    {
        protected readonly ICustomerRepository _rpCustomer;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly ICheckDupBusiness _bizCheckDup;
        public CustomerController(ICustomerRepository customerRepository,
            IPartnerRepository partnerRepository,
            ICheckDupBusiness checkDupBusiness):base()
        {
            _rpCustomer = customerRepository;
            _rpPartner = partnerRepository;
            _bizCheckDup = checkDupBusiness;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_5_1, _href = "Customer/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_5_2, _href = "Customer/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }

        public JsonResult GetListStatus()
        {
            var result = new List<OptionSimple>();

            var item = new OptionSimple()
            {
                Code = "0",
                IsSelect = true,
                Name = "Bổ Sung hồ sơ"
            };

            result.Add(item);


            item = new OptionSimple()
            {
                Code = "2",
                IsSelect = false,
                Name = "Đã check"
            };

            result.Add(item);

            item = new OptionSimple()
            {
                Code = "3",
                IsSelect = false,
                Name = "Chưa check"
            };
            result.Add(item);
            item = new OptionSimple()
            {
                Code = "4",
                IsSelect = false,
                Name = "Cancel"
            };
            result.Add(item);
            return ToJsonResponse(true, "", result);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var result = await _bizCheckDup.GetsAsync(freeText, page, limit, GlobalData.User.IDUser);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = "";// LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public async Task<JsonResult> CreateAsync(CheckDupAddModel model)
        {
            var result = await _bizCheckDup.CreateAsync(model, GlobalData.User.IDUser);
            return ToJsonResponse(result.IsSuccess, result.Message, result.Data);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var customer = await  _bizCheckDup.GetByIdAsync(id);
            return View(customer);
        }
        public async Task<ActionResult> UpdateAsync(CheckDupEditModel model)
        {
            var result = await _bizCheckDup.UpdateAsync(model, GlobalData.User.IDUser);
            return ToResponse(result.IsSuccess, result.Message, result.Data);
        }
        public async Task<JsonResult> GetPartner(int customerId = 0)
        {
            var partners = await _rpPartner.GetListForCheckCustomerDuplicateAsync();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            return ToJsonResponse(true, null, partners);
        }
        public async Task<JsonResult> GetAllPartner()
        {
            var partners = await _rpPartner.GetListForCheckCustomerDuplicateAsync();

            return ToJsonResponse(true, null, partners);
        }

        public async Task<JsonResult> GetNotes(int customerId)
        {
            var result = await _bizCheckDup.GetNotesAsync(customerId);
            return ToJsonResponse(true, null,result);
        }
        //public async Task<JsonResult> GetNotes(int customerId)
        //{
        //    var datas = await _rpCustomer.GetNoteByCustomerId(customerId);
        //    return ToJsonResponse(true, null, datas);
        //}
    }
}
