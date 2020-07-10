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
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CompanyController : BaseController
    {
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_8_1, _href = "Company/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_8_2, _href = "Company/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var bizCompany = new CompanyRepository();
            var totalRecord = await bizCompany.CountAsync(freeText);
            var datas = await bizCompany.GetsAsync(freeText, page, limit);
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public async Task<ActionResult> Create(Company model)
        {
            var _bizCompany = new CompanyRepository();
            var id = await _bizCompany.CreateAsync(model);
            if (id > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = id,
                        TypeId = NoteType.Company,
                        UserId = GlobalData.User.IDUser
                    };
                    var bizNote = new NoteRepository();
                    await bizNote.AddNoteAsync(note);
                }
                return ToResponse(true);
            }
            return ToResponse(false);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var bizCompany = new CompanyRepository();
            var company = await bizCompany.GetByIdAsync(id);
            ViewBag.company = company;
            return View();
        }
        public async Task<ActionResult> Update(Company model)
        {

            if (model == null )
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            var bizCompany = new CompanyRepository();

            var result = await bizCompany.UpdateAsync(model);
            if (!result)
                return ToResponse(false);
            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {
                var note = new GhichuModel
                {
                    Noidung = model.LastNote,
                    HosoId = model.Id,
                    TypeId = NoteType.Company,
                    UserId = GlobalData.User.IDUser
                };
                var bizNote = new NoteRepository();
                await bizNote.AddNoteAsync(note);
            }
            
            return ToResponse(true);
        }
        public async Task<JsonResult> GetPartner()
        {
            var bizPartner = new PartnerBLL();
            var partners = await bizPartner.GetListForCheckCustomerDuplicateAsync();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            return ToJsonResponse(true, null, partners);
        }
        public async Task<JsonResult> GetAllPartner()
        {
            var bizPartner = new PartnerBLL();
            var partners = await bizPartner.GetListForCheckCustomerDuplicateAsync();

            return ToJsonResponse(true, null, partners);
        }
        public async Task<JsonResult> GetNotes(int companyId)
        {
            var bizNote = new NoteRepository();
            var datas = await bizNote.GetNoteByTypeAsync(companyId, typeId: (int)NoteType.Company);
            return ToJsonResponse(true, null, datas);
        }
    }
}
