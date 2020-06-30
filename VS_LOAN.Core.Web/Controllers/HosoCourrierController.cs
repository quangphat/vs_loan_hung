using System;
using System.Collections.Generic;
using System.IO;
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
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CourrierController : BaseController
    {
        private bool result;

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
        public async Task<JsonResult> Search(string freeText = null, int provinceId = 0, int courierId = 0, string status = null, int groupId = 0, int page = 1, int limit = 10)
        {
            var bzCourier = new HosoCourrierBusiness();
            var totalRecord = await bzCourier.CountHosoCourrier(freeText, courierId, status, groupId);
            var datas = await bzCourier.GetHosoCourrier(freeText, GlobalData.User.IDUser, status, page, limit, groupId);
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = GlobalData.User.IDUser;
            return View();
        }
        public async Task<ActionResult> Create(HosoCorrierRequestModel model)
        {
            if (model == null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ", 0);
            }
            if (model.AssignId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn Courier", 0);
            }
            var hoso = new HosoCourier
            {
                CustomerName = model.CustomerName,
                ReceiveDate = DateTime.Now,
                Cmnd = model.Cmnd,
                Status = (int)HosoCourierStatus.New,
                LastNote = model.LastNote,
                CreatedBy = GlobalData.User.IDUser,
                ProductId = model.ProductId,
                PartnerId = model.PartnerId,
                Phone = model.Phone,
                AssignId = model.AssignId,
                GroupId = model.GroupId,
                ProvinceId = model.ProvinceId
            };
            var _bizCourrier = new HosoCourrierBusiness();
            var id = await _bizCourrier.Create(hoso);
            if (id > 0)
            {
                var tasks = new List<Task>();
                var ids = new List<int>() { model.AssignId, GlobalData.User.IDUser };
                foreach(var assigneeId in ids)
                {
                    tasks.Add(_bizCourrier.InsertCourierAssignee(id, assigneeId));
                }
                await Task.WhenAll(tasks);
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var bizNote = new NoteBusiness();
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = id,
                        UserId = hoso.CreatedBy,
                        TypeId = NoteType.HosoCourrier
                    };
                    bizNote.AddNoteAsync(note);

                }

                return ToResponse(true, "", id);
            }
            return ToResponse(false);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var hoso = await new HosoCourrierBusiness().GetById(id);
            ViewBag.hoso = hoso;
            return View();
        }
        public JsonResult LayDSGhichu(int hosoId)
        {
            List<GhichuViewModel> rs = new HoSoBLL().LayDanhsachGhichu(hosoId, (int)HosoType.HosoCourrier);
            if (rs == null)
                rs = new List<GhichuViewModel>();
            return ToJsonResponse(true, null, rs);
        }
        public async Task<ActionResult> Update(HosoCorrierRequestModel model)
        {

            if (model == null || model.Id <= 0)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                return ToResponse(false, "Tên khách hàng không được để trống");
            }
            if (model.AssignId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn courier");
            }
            var hoso = new HosoCourier
            {
                CustomerName = model.CustomerName,
                Cmnd = model.Cmnd,
                Status = model.Status,
                LastNote = model.LastNote,
                UpdatedBy = GlobalData.User.IDUser,
                ProductId = model.ProductId,
                PartnerId = model.PartnerId,
                Phone = model.Phone,
                AssignId = model.AssignId,
                Id = model.Id,
                GroupId = model.GroupId,
                ProvinceId = model.ProvinceId
            };
            var _bizCourrier = new HosoCourrierBusiness();
            var result = await _bizCourrier.Update(model.Id, hoso);
            if (result)
            {
                _bizCourrier.InsertCourierAssignee(model.Id, model.AssignId);
                if(!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var bizNote = new NoteBusiness();
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = model.Id,
                        UserId = hoso.UpdatedBy,
                        TypeId = NoteType.HosoCourrier
                    };
                    bizNote.AddNoteAsync(note);
                }
                
            }
            return ToResponse(true);
        }
        public async Task<JsonResult> GetPartner(int customerId)
        {
            var bizCustomer = new CustomerBusiness();
            var bizPartner = new PartnerBLL();
            var customerCheck = bizCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = await bizPartner.GetListForCheckCustomerDuplicateAsync();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            foreach (var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }

            return ToJsonResponse(true, null, partners);
        }
        public JsonResult GetNotes(int customerId)
        {
            var bizCustomer = new CustomerBusiness();
            var datas = bizCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true, null, datas);
        }
        public async Task<JsonResult> GetStatusList()
        {
            var bizHoso = new HosoBusiness();
            var result = await bizHoso.GetStatusListByType((int)HosoType.HosoCourrier);
            return ToJsonResponse(true, string.Empty, result);
        }
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
                return ToJsonResponse(false);
            var bizTailieu = new TailieuBusiness();
            if (isReset)
            {
                var deleteAll = await bizTailieu.RemoveAllTailieu(hosoId, (int)HosoType.HosoCourrier);
                if (!deleteAll)
                    return ToJsonResponse(false);
            }
            foreach (var item in filesGroup)
            {
                if (item.files.Any())
                {
                    foreach (var file in item.files)
                    {
                        var tailieu = new TaiLieu
                        {
                            FileName = file.FileName,
                            FilePath = file.FileUrl,
                            HosoId = hosoId,
                            TypeId = Convert.ToInt32(file.Key),
                            LoaiHoso = (int)HosoType.HosoCourrier
                        };
                        await bizTailieu.Add(tailieu);
                    }
                }
            }
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> Import()
        {
            var file = Request.Files[0];
            if (file == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            Stream stream = file.InputStream;
            stream.Position = 0;
            var bizMedia = new MediaBusiness();
            using (var fileStream = new MemoryStream())
            {
                await stream.CopyToAsync(fileStream);
                var results = await bizMedia.ReadXlsxFile(fileStream, GlobalData.User.IDUser);
                if (results == null || !results.Any())
                    return ToJsonResponse(false, "Không thành công", null);
                var _bizCourrier = new HosoCourrierBusiness();
                var tasks = new List<Task>();
                foreach (var item in results)
                {
                    tasks.Add(InsertHosoFromFile(item, _bizCourrier));
                }

                await Task.WhenAll(tasks);
                return ToJsonResponse(true,"Thành công");
            }

        }
        private async Task<bool> InsertHosoFromFile(HosoCourier hoso, HosoCourrierBusiness _bizCourrier)
        {
            var id = await _bizCourrier.Create(hoso);
            if(hoso.AssigneeIds==null || hoso.AssigneeIds.Any())
            {
            }
            var tasks = new List<Task>();
            foreach (var assingeeId in hoso.AssigneeIds)
            {
                tasks.Add(_bizCourrier.InsertCourierAssignee(id, assingeeId));
            }
            await Task.WhenAll(tasks);
            if (!string.IsNullOrWhiteSpace(hoso.LastNote))
            {
                var bizNote = new NoteBusiness();
                var note = new GhichuModel
                {
                    Noidung = hoso.LastNote,
                    HosoId = id,
                    UserId = GlobalData.User.IDUser,
                    TypeId = NoteType.HosoCourrier
                };
                bizNote.AddNoteAsync(note);

            }
            return true;
        }
        public FileResult DownloadTemplateFile(string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\ImportSanPham\\" + fileName);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = fileName;
            return response;
        }
    }
}
