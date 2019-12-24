using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CourrierController : BaseController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_7_1, _href = "Courrier/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_7_2, _href = "Courrier/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string freeText = null, int page = 1,int  limit = 10)
        {
            var bzCustomer = new CustomerBLL();
            var totalRecord = bzCustomer.Count(freeText);
            var datas = bzCustomer.Gets(freeText, page, limit);
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true,null,result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public async Task<ActionResult> Create(HosoCorrierRequestModel model)
        {
            if(model==null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ", 0);
            }
            var hoso = new HosoCourrier
            {
                CustomerName = model.CustomerName,
                ReceiveDate = DateTime.Now,
                Cmnd = model.Cmnd,
                Status = (int)TrangThaiHoSo.Nhap,
                LastNote = model.LastNote,
                CreatedBy = GlobalData.User.IDUser,
                ProductId = model.ProductId,
                PartnerId = model.PartnerId,
                ProvinceId = model.ProvinceId,
                Phone = model.Phone,
                DistrictId = model.DistrictId,
                Address = model.Address,
                
            };
            var _bizCourrier = new HosoCourrierBusiness();
            var id = await _bizCourrier.Create(hoso);
            if (id > 0)
            {
                if(!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var bizNote = new NoteBusiness();
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = id,
                        UserId = hoso.CreatedBy,
                        TypeId = (int)HosoType.HosoCourrier
                    };
                    await bizNote.AddNote(note);
                }
                
                return ToResponse(true);
            }        
            return ToResponse(false);
            
        }
        public ActionResult Edit(int id)
        {
            var customer = new CustomerBLL().GetById(id);
            ViewBag.customer = customer;
            return View();
        }
        public ActionResult Update(CustomerEditModel model)
        {
            
            if(model == null || model.Customer == null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            var bizCustomer = new CustomerBLL();
            var match = string.Empty;
            var notmatch = string.Empty;
            if(model.Partners !=null)
            {
                match = string.Join(",", model.Partners.Where(p => p.IsSelect == true).Select(p => p.Name).ToArray());
                notmatch = string.Join(",", model.Partners.Where(p => !p.IsSelect ).Select(p => p.Name).ToArray());
            }
            
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
                MatchCondition = match,
                NotMatch = notmatch
            };

            var result = bizCustomer.Update(customer);
            if (!result)
                return ToResponse(false);
            if(!string.IsNullOrWhiteSpace(model.Customer.Note))
            {
                var note = new CustomerNote
                {
                    Note = model.Customer.Note,
                    CustomerId = customer.Id,
                    CreatedBy = customer.UpdatedBy
                };
                bizCustomer.AddNote(note);
            }
            if(model.Partners ==null || !model.Partners.Any())
            {
                return ToResponse(true);
            }
            var bizPartner = new PartnerBLL();
            var deleteCustomerCheck = bizCustomer.DeleteCustomerCheck(customer.Id);
            if(deleteCustomerCheck)
            {
                var selectedPartner = model.Partners.Where(p => p.IsSelect == true).ToList();
                if(selectedPartner.Any())
                {
                    foreach(var item in selectedPartner)
                    {
                        var partner = new CustomerCheck
                        {
                            CustomerId = customer.Id,
                            PartnerId = item.Id,
                            Status = 1,
                            CreatedBy = customer.UpdatedBy
                        };
                        bizCustomer.InserCustomerCheck(partner);
                    }
                }
            }
            return ToResponse(true);
        }
        public JsonResult GetPartner(int customerId)
        {
            var bizCustomer = new CustomerBLL();
            var bizPartner = new PartnerBLL();
            var customerCheck = bizCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = bizPartner.GetListForCheckCustomerDuplicate();
            if (partners == null)
                return ToJsonResponse(true,null,new List<OptionSimple>());
            foreach(var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }

            return ToJsonResponse(true,null,partners);
        }
        public  JsonResult GetNotes(int customerId)
        {
            var bizCustomer = new CustomerBLL();
            var datas = bizCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true,null,datas);
        }
    }
}
