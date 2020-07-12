using MCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MCreditController : BaseController
    {
        protected readonly IMCeditRepository _rpMCredit;
        protected readonly IMCreditService _svMCredit;
        protected readonly INoteRepository _rpNote;
        protected readonly IMCreditRepositoryTest _rpMcTest;
        public MCreditController(IMCeditRepository rpMCredit,
            INoteRepository noteRepository,
            IMCreditRepositoryTest mCreditRepositoryTest,
            IMCreditService loanContractService) : base()
        {
            _rpMCredit = rpMCredit;
            _svMCredit = loanContractService;
            _rpNote = noteRepository;
            _rpMcTest = mCreditRepositoryTest;
        }
        public async Task<JsonResult> AuthenMC(AuthenMCModel model)
        {
            if (model == null)
                return ToJsonResponse(false);
            var result = await _svMCredit.AuthenByUserId(model.UserId, model.IsUpdateToken, model.IsUpdateProduct, model.IsUpdateLoanPeriod, model.IsUpdateLocation, model.IsUpdateCity);
            return ToJsonResponse(true, result, result);
        }
        public async Task<JsonResult> CheckSaleApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckSale(GlobalData.User.IDUser, model.Value);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckCat()
        {
            return View();
        }
        public async Task<JsonResult> CheckCatApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckCat(GlobalData.User.IDUser, model.Value);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckCIC()
        {
            return View();
        }
        public async Task<JsonResult> CheckCICApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckCIC(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckDuplicate()
        {
            return View();
        }
        public async Task<JsonResult> CheckDupApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckDup(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult CheckStatus()
        {
            return View();
        }
        public async Task<JsonResult> CheckStatusApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckStatus(model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(true, result.msg.ToString(), result);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText, string status, string type, int page)
        {
            var result = await _svMCredit.SearchProfiles(freeText, status, type, page, GlobalData.User.IDUser);
            return ToJsonResponse(true, "", result);
        }
        public ActionResult AddNew()
        {
            ViewBag.User = GlobalData.User;
            return View();
        }
        public async Task<JsonResult> SaveDraft(MCredit_TempProfileAddModel model)
        {
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.CreatedBy = GlobalData.User.IDUser;
            var id = await _rpMCredit.CreateProfile(profile);
            if (id > 0)
            {
                if(!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    GhichuModel note = new GhichuModel
                    {
                        UserId = GlobalData.User.IDUser,
                        HosoId = id,
                        Noidung = model.LastNote,
                        CommentTime = DateTime.Now,
                        TypeId = NoteType.MCreditTemp
                    };
                    _rpNote.AddNoteAsync(note);
                }
               
                var obj = _mapper.Map<MCProfilePostModel>(profile);
                var result = await _svMCredit.CreateProfile(obj, GlobalData.User.IDUser);
                return ToJsonResponse(true, result.message, result);
            }
            return ToJsonResponse(true, "", id);
        }
        public async Task<ActionResult> TempProfile(int id)
        {
            var result = await _rpMCredit.GetTemProfileById(id);
            ViewBag.model = result;
            return View();
        }
        public async Task<JsonResult> Create(MCredit_TempProfileAddModel model)
        {

            //var hoso = new HoSoBLL().LayChiTiet(Convert.ToInt32(model.Value));
            //var obj = _mapper.Map<ProfileAddObj>(hoso);
            //obj.cityId = "58";
            //obj.homeTown = "Bình Phước";
            //obj.isAddr = true;
            //obj.loanPeriodCode = "3";
            //obj.isInsurance = false;
            //obj.logSignCode = "28";
            //obj.productCode = "C0000011";
            //obj.saleID = "VBF0265";
            //var result = await _svMCredit.CreateProfile(model, 3514);
            return ToJsonResponse(true, "");
        }
        public async Task<JsonResult> GetMCSimpleList(int type)
        {
            var result = new List<OptionSimple>();
            if (type == (int)MCTableType.MCreditCity)
            {
                result = await _rpMCredit.GetMCCitiesSimpleList();
            }
            if (type == (int)MCTableType.MCreditLoanPeriod)
            {
                result = await _rpMCredit.GetMCLoanPerodSimpleList();
            }
            if (type == (int)MCTableType.MCreditlocations)
            {
                result = await _rpMCredit.GetMCLocationSimpleList();
            }
            if (type == (int)MCTableType.MCreditProduct)
            {
                result = await _rpMCredit.GetMCProductSimpleList();
            }
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> GetFileUpload(int profileId, int profileType = 3)
        {
            var data = await _rpMcTest.GetFilesNeedToUpload(new GetFileUploadRequest {
                Code= "C0000027",
                Id = "99999999",
                Loccode = "KIK220001",
                Issl ="0",
                Money="20000000",
                token =""
            });
            if (data == null || data.Groups==null)
                return ToJsonResponse(false, "Không thể lấy file", new List<LoaiTaiLieuModel>());
            var result = new List<LoaiTaiLieuModel>();
            foreach(var group in data.Groups)
            {
                if (group.Documents == null)
                    continue;
                foreach(var doc in group.Documents)
                {
                    result.Add(new MCFileUpload {
                    ID = doc.Id,
                    BatBuoc = group.Mandatory ? 1 :0,
                    Ten = doc.DocumentName,
                    DocumentId = doc.Id,
                    GroupId = group.GroupId,
                    DocumentCode = doc.DocumentCode,
                    MapBpmVar = doc.MapBpmVar,
                    ProfileId = profileId,
                    ProfileTypeId = profileType,
                    DocumentName = doc.DocumentName
                });
                }
                
            }
            return ToJsonResponse(true, "", result);
        }
    }
}