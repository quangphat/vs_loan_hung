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

namespace VS_LOAN.Core.Web.Controllers
{
    public class CustomerController : BaseController
    {
        protected readonly ICustomerRepository _rpCustomer;
        protected readonly IPartnerRepository _rpPartner;
        public CustomerController(ICustomerRepository customerRepository, IPartnerRepository partnerRepository):base()
        {
            _rpCustomer = customerRepository;
            _rpPartner = partnerRepository;
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
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var totalRecord = await _rpCustomer.Count(freeText, GlobalData.User.IDUser);
            var datas = await _rpCustomer.Gets(freeText, page, limit, GlobalData.User.IDUser);
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = "";// LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public async Task<ActionResult> Create(CustomerModel model)
        {
            if (model.Partners == null || !model.Partners.Any())
                return ToResponse(false, "Vui lòng chọn đối tác", 0);
            var partner = model.Partners[0];
            bool isMatch = partner.IsSelect;
            var customer = new Customer
            {
                FullName = model.FullName,
                CheckDate = model.CheckDate,
                Cmnd = model.Cmnd,
                CICStatus = 0,
                LastNote = model.Note,
                CreatedBy = GlobalData.User.IDUser,
                Gender = model.Gender,
                PartnerId = model.Partners != null ? partner.Id : 0,
                IsMatch = model.Partners != null ? partner.IsSelect : false,
                MatchCondition = isMatch == true ? partner.Name : string.Empty,
                NotMatch = isMatch == false ? partner.Name : string.Empty,
                ProvinceId = model.ProvinceId,
                BirthDay = model.BirthDay,
                Address = model.Address,
                Phone = model.Phone,
                Salary = model.Salary
            };
            
            var id = await _rpCustomer.Create(customer);
            if (id > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    var note = new CustomerNote
                    {
                        Note = model.Note,
                        CustomerId = id,
                        CreatedBy = customer.CreatedBy
                    };
                    _rpCustomer.AddNote(note);
                }
                return ToResponse(true);
            }
            return ToResponse(false);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var customer = await  _rpCustomer.GetById(id);
            customer.BirthDay = BusinessExtension.FromUnixTime(customer.BirthDay.Value.ToUnixTime());
            ViewBag.customer = customer;

            return View();
        }
        public JsonResult GetCustomerById(int id)
        {
            var customer = new CustomerRepository().GetById(id);
            return ToJsonResponse(true, null, customer);
        }
        public async Task<ActionResult> Update(CustomerEditModel model)
        {

            if (model == null || model.Customer == null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (model.Partners == null || !model.Partners.Any())
                return ToResponse(false, "Vui lòng chọn đối tác", 0);
            var partner = model.Partners[0];
            bool isMatch = partner.IsSelect;
           
           
            var customer = new Customer
            {
                Id = model.Customer.Id,
                FullName = model.Customer.FullName,
                CheckDate = model.Customer.CheckDate,
                Cmnd = model.Customer.Cmnd,
                CICStatus = model.Customer.CICStatus,
                LastNote = model.Customer.Note,
                UpdatedBy = GlobalData.User.IDUser,
                Gender = model.Customer.Gender,
                PartnerId = model.Partners != null ? partner.Id : 0,
                IsMatch = model.Partners != null ? partner.IsSelect : false,
                MatchCondition = isMatch == true ? partner.Name : string.Empty,
                NotMatch = isMatch == false ? partner.Name : string.Empty,
                ProvinceId = model.Customer.ProvinceId,
                BirthDay = model.Customer.BirthDay,
                Address = model.Customer.Address,
                Phone = model.Customer.Phone,
                Salary = model.Customer.Salary
            };

            var result = await _rpCustomer.Update(customer);
            if (!result)
                return ToResponse(false);
            if (!string.IsNullOrWhiteSpace(model.Customer.Note))
            {
                var note = new CustomerNote
                {
                    Note = model.Customer.Note,
                    CustomerId = customer.Id,
                    CreatedBy = customer.UpdatedBy
                };
                _rpCustomer.AddNote(note);
            }
            
            return ToResponse(true);
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
            var datas = await _rpCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true, null, datas);
        }
    }
}
