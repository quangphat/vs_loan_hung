using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Business.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CourrierController : BaseController
    {
        protected readonly IHosoCourrierRepository _rpCourierProfile;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly ICustomerRepository _rpCustomer;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly IHosoRepository _rpHoso;
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly INoteRepository _rpNote;
        protected readonly IEmployeeRepository _rpEmployee;
        //protected readonly ILogRepository _rpLog;
        public CourrierController(IHosoCourrierRepository hosoCourrierRepository,
            IPartnerRepository partnerRepository,
            IHosoRepository hosoRepository,
            ITailieuRepository tailieuRepository,
            INoteRepository noteRepository,
            IEmployeeRepository employeeRepository,
            
            ICustomerRepository customerRepository, IMediaBusiness mediaBusiness) : base()
        {
            _rpNote = noteRepository;
            _rpCourierProfile = hosoCourrierRepository;
            _rpCustomer = customerRepository;
            _rpTailieu = tailieuRepository;
            _rpHoso = hosoRepository;
            _rpPartner = partnerRepository;
            _bizMedia = mediaBusiness;
            _rpEmployee = employeeRepository;
           // _rpLog = logRepository;
        }
        private bool result;


        public ActionResult Index()
        {
            var isAdmin = GlobalData.User.UserType == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.isAdmin = isAdmin ? 1 : 0;
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, int provinceId = 0, int courierId = 0, string status = null, int groupId = 0, int page = 1, int limit = 10, string salecode = null)
        {
            //var totalRecord = await _rpCourierProfile.CountHosoCourrier(freeText, courierId, GlobalData.User.IDUser, status, groupId, provinceId, salecode);
            var datas = await _rpCourierProfile.GetHosoCourrier(freeText, courierId, GlobalData.User.IDUser, status, page, limit, groupId, provinceId, salecode);
            var totalRecord = (datas != null && datas.Any()) ? datas[0].TotalRecord : 0;
            //await _rpLog.InsertLog("courier-search", $"freetext:{freeText}, userid:{ GlobalData.User.IDUser}, courierId:{courierId}, status:{status}, salecode: {salecode}");
            var result = DataPaging.Create(datas, totalRecord);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult AddNew()
        {
            ViewBag.formindex = "";//LstRole[RouteData.Values["action"].ToString()]._formindex;
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
            var sale = await _rpEmployee.GetEmployeeByCode(model.SaleCode.ToString().Trim());
            if (sale == null)
            {
                return ToResponse(false, "Sale không tồn tại, vui lòng kiểm tra lại");
            }
            var hoso = new HosoCourier
            {
                CustomerName = model.CustomerName,
                Cmnd = model.Cmnd,
                Status = (int)HosoCourierStatus.New,
                LastNote = model.LastNote,
                CreatedBy = GlobalData.User.IDUser,
                SaleCode = model.SaleCode,
                Phone = model.Phone,
                AssignId = model.AssignId,
                GroupId = model.GroupId,
                DistrictId = model.DistrictId,
                ProvinceId = model.ProvinceId
            };

            var id = await _rpCourierProfile.Create(hoso);
            if (id > 0)
            {
                var tasks = new List<Task>();
                var ids = new List<int>() { model.AssignId, GlobalData.User.IDUser, 1 };//1 is Thainm
                if (!string.IsNullOrWhiteSpace(model.SaleCode))
                {

                    if (sale != null)
                    {
                        ids.Add(sale.Id);
                    }
                }
                foreach (var assigneeId in ids)
                {
                    tasks.Add(_rpCourierProfile.InsertCourierAssignee(id, assigneeId));
                }
                await Task.WhenAll(tasks);
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = id,
                        UserId = hoso.CreatedBy,
                        TypeId = (int)NoteType.HosoCourrier
                    };
                    await _rpNote.AddNoteAsync(note);

                }

                return ToResponse(true, "", id);
            }
            return ToResponse(false);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var hoso = await new HosoCourrierRepository().GetById(id);
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
            var profile = await _rpCourierProfile.GetById(model.Id);
            if (profile == null)
            {
                return ToJsonResponse(false, "Hồ sơ không tồn tại");
            }
            bool isAdmin = await _rpEmployee.CheckIsAdmin(GlobalData.User.IDUser);
            if (profile.Status == (int)TrangThaiHoSo.Cancel)
            {
                
                if (!isAdmin)
                {
                    return ToJsonResponse(false, "Bạn không có quyền, vui lòng liên hệ Admin");
                }
            }
            var sale = null as OptionSimple;
            if(!isAdmin )
            {
                model.SaleCode = "";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.SaleCode))
                    return ToResponse(false, "Mã teleSale ko hợp lệ");
                sale = await _rpEmployee.GetEmployeeByCode(model.SaleCode.Trim());
                if (sale == null)
                {
                    return ToResponse(false, "Sale không tồn tại, vui lòng kiểm tra lại");
                }
            }
            
            var hoso = new HosoCourier
            {
                CustomerName = model.CustomerName,
                Cmnd = model.Cmnd,
                Status = model.Status,
                LastNote = model.LastNote,
                UpdatedBy = GlobalData.User.IDUser,
                Phone = model.Phone,
                SaleCode = model.SaleCode,
                AssignId = model.AssignId,
                Id = model.Id,
                GroupId = model.GroupId,
                DistrictId = model.DistrictId,
                ProvinceId = model.ProvinceId
            };

            var result = await _rpCourierProfile.Update(model.Id, hoso);
            if (result)
            {
                if (!string.IsNullOrWhiteSpace(model.SaleCode))
                {

                    if (sale != null)
                    {

                        await _rpCourierProfile.InsertCourierAssignee(model.Id, sale.Id);
                    }
                }
                _rpCourierProfile.InsertCourierAssignee(model.Id, model.AssignId);
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    
                    var note = new GhichuModel
                    {
                        Noidung = model.LastNote,
                        HosoId = model.Id,
                        UserId = hoso.UpdatedBy,
                        TypeId = (int)NoteType.HosoCourrier
                    };
                    await _rpNote.AddNoteAsync(note);
                }

            }
            return ToResponse(true);
        }
        public async Task<JsonResult> GetPartner(int customerId)
        {
            var customerCheck = await _rpCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = await _rpPartner.GetListForCheckCustomerDuplicateAsync();
            if (partners == null)
                return ToJsonResponse(true, null, new List<OptionSimple>());
            foreach (var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }

            return ToJsonResponse(true, null, partners);
        }
        public async Task<JsonResult> GetNotes(int customerId)
        {

            var datas = await _rpCustomer.GetNoteByCustomerId(customerId);
            return ToJsonResponse(true, null, datas);
        }
        public async Task<JsonResult> GetStatusList()
        {
            var result = await _rpHoso.GetStatusListByType((int)HosoType.HosoCourrier);
            return ToJsonResponse(true, string.Empty, result);
        }
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
                return ToJsonResponse(false);

            if (isReset)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieu(hosoId, (int)HosoType.HosoCourrier);
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
                            ProfileId = hosoId,
                            FileKey = Convert.ToInt32(file.Key),
                            ProfileTypeId = (int)HosoType.HosoCourrier,
                            Folder = file.FileUrl
                        };
                        await _rpTailieu.Add(tailieu);
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
            using (var fileStream = new MemoryStream())
            {
                await stream.CopyToAsync(fileStream);
                var results = await _bizMedia.ReadXlsxFile(fileStream, GlobalData.User.IDUser);
                if (results == null || !results.Any())
                    return ToJsonResponse(false, "Không thành công", null);

                var tasks = new List<Task>();
                foreach (var item in results)
                {
                    tasks.Add(InsertHosoFromFile(item, _rpCourierProfile));
                }

                await Task.WhenAll(tasks);
                return ToJsonResponse(true, "Thành công");
            }

        }
        private async Task<bool> InsertHosoFromFile(HosoCourier hoso, IHosoCourrierRepository _rpCourrier)
        {
            var id = await _rpCourrier.Create(hoso);
            if (!string.IsNullOrWhiteSpace(hoso.LastNote))
            {

                var note = new GhichuModel
                {
                    Noidung = hoso.LastNote,
                    HosoId = id,
                    UserId = GlobalData.User.IDUser,
                    TypeId = (int)NoteType.HosoCourrier
                };
                await _rpNote.AddNoteAsync(note);

            }
            if (hoso.AssigneeIds == null || !hoso.AssigneeIds.Any())
            {
                hoso.AssigneeIds = new List<int>();
            }
            hoso.AssigneeIds.Add(GlobalData.User.IDUser);
            hoso.AssigneeIds.Add(1);//Thainm
            var tasks = new List<Task>();
            foreach (var assingeeId in hoso.AssigneeIds)
            {
                tasks.Add(_rpCourrier.InsertCourierAssignee(id, assingeeId));
            }
            await Task.WhenAll(tasks);

            return true;
        }
       
        public async Task<JsonResult> GetEmployeesFromOne(int id)
        {

            var employee = await _rpCourierProfile.GetEmployeeById(id);
            if (employee == null)
                return ToJsonResponse(false, null, null);
            return ToJsonResponse(true, "", new List<OptionSimple> { new OptionSimple {
                Id = employee.IDUser,
                Name = employee.FullName
            } });
        }
    }
}
