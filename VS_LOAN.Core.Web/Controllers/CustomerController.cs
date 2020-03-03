﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CustomerController : BaseController
    {
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
        public JsonResult Search(string freeText = null, int page = 1, int limit = 10)
        {
            var bzCustomer = new CustomerBLL();
            var totalRecord = bzCustomer.Count(freeText);
            var datas = bzCustomer.Gets(freeText, page, limit);
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public ActionResult Create(CustomerModel model)
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
                NotMatch = isMatch == false ? partner.Name : string.Empty
            };
            var _bizCustomer = new CustomerBLL();
            var id = _bizCustomer.Create(customer);
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
                    _bizCustomer.AddNote(note);
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

            if (model == null || model.Customer == null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (model.Partners == null || !model.Partners.Any())
                return ToResponse(false, "Vui lòng chọn đối tác", 0);
            var partner = model.Partners[0];
            bool isMatch = partner.IsSelect;
            var bizCustomer = new CustomerBLL();
           
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
                NotMatch = isMatch == false ? partner.Name : string.Empty
            };

            var result = bizCustomer.Update(customer);
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
                bizCustomer.AddNote(note);
            }
            
            return ToResponse(true);
        }
        public JsonResult GetPartner(int customerId = 0)
        {
            var bizPartner = new PartnerBLL();
            var partners = bizPartner.GetListForCheckCustomerDuplicate();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            return ToJsonResponse(true, null, partners);
        }
        public JsonResult GetAllPartner()
        {
            var bizPartner = new PartnerBLL();
            var partners = bizPartner.GetListForCheckCustomerDuplicate();

            return ToJsonResponse(true, null, partners);
        }
        public JsonResult GetNotes(int customerId)
        {
            var bizCustomer = new CustomerBLL();
            var datas = bizCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true, null, datas);
        }
    }
}
