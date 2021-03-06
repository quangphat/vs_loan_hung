﻿using System;
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
using System.IO.Compression;
using VS_LOAN.Core.Utility.OfficeOpenXML;

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
        public async Task<ActionResult> ExportFile(string freeText = null, int provinceId = 0, int courierId = 0, string status = null, int groupId = 0, int page = 1, int limit = 10, string salecode = null)
        {
            var request = new CourierSearchRequestModel
            {
                freeText = freeText,
                provinceId = provinceId,
                courierId = courierId,
                status = status,
                groupId = groupId,
                page = page,
                limit = limit,
                salecode = salecode
            };
            try
            {


                string destDirectory = VS_LOAN.Core.Utility.Path.DownloadBill + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/";
                bool exists = System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                if (!exists)
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                string fileName = "Report-DSHS" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".xlsx";
                using (FileStream stream = new FileStream(Server.MapPath(destDirectory + fileName), FileMode.CreateNew))
                {
                    Byte[] info = System.IO.File.ReadAllBytes(Server.MapPath(VS_LOAN.Core.Utility.Path.ReportTemplate + "CourierExportTemplate.xlsx"));
                    stream.Write(info, 0, info.Length);
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        string nameSheet = "DSHS";
                        ExcelOOXML excelOOXML = new ExcelOOXML(archive);
                        int rowindex = 2;
                        long totalRecord = 100;
                        decimal totalPage = 10;
                       
                        for (int p = 1; p <= totalPage; p++)
                        {
                         var result = await SearchByModel(request);
                            if (p == 1)
                                excelOOXML.InsertRow(nameSheet, rowindex, result.Datas.Count - 1, true);
                            totalPage = Math.Ceiling((decimal)result.TotalRecord / request.limit);
                            if (result != null)
                            {
                                totalRecord = result.TotalRecord;

                                
                                for (int i = 0; i < result.Datas.Count; i++)// dòng
                                {
                                    excelOOXML.SetCellData(nameSheet, "A" + rowindex, (i + 1).ToString());
                                    excelOOXML.SetCellData(nameSheet, "B" + rowindex, result.Datas[i].CustomerName.ToString());
                                    excelOOXML.SetCellData(nameSheet, "C" + rowindex, result.Datas[i].Phone);
                                    excelOOXML.SetCellData(nameSheet, "D" + rowindex, result.Datas[i].Cmnd);
                                    excelOOXML.SetCellData(nameSheet, "E" + rowindex, result.Datas[i].Status);
                                    excelOOXML.SetCellData(nameSheet, "F" + rowindex, result.Datas[i].DistrictName);
                                    excelOOXML.SetCellData(nameSheet, "G" + rowindex, result.Datas[i].ProvinceName);
                                    excelOOXML.SetCellData(nameSheet, "H" + rowindex, result.Datas[i].AssignUser);
                                    excelOOXML.SetCellData(nameSheet, "I" + rowindex, result.Datas[i].LastNote);
                                    excelOOXML.SetCellData(nameSheet, "J" + rowindex, result.Datas[i].CreatedTime);
                                    excelOOXML.SetCellData(nameSheet, "K" + rowindex, result.Datas[i].CreatedUser);
                                    excelOOXML.SetCellData(nameSheet, "L" + rowindex, result.Datas[i].UpdatedTime);
                                    excelOOXML.SetCellData(nameSheet, "M" + rowindex, result.Datas[i].UpdatedBy);

                                    rowindex++;
                                }

                               //rowindex++;
                            }
                        }

                        archive.Dispose();
                    }
                    stream.Dispose();
                }
                var file = "/File/GetFile?path=" + destDirectory + fileName;
                return ToResponse(true, null, file);
            }
            catch(Exception e)
            {
                return ToResponse(false);
            }
            //var result = await ExportUtil.Export<CourierSearchRequestModel, CourierExportModel>(Response, SearchByModel, request, "CourierProfiles.csv", columns, filePath);
            
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
            ViewBag.isAdmin = await _rpEmployee.CheckIsAdmin(GlobalData.User.IDUser);
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
        private async Task<DataPaging<List<CourierExportModel>>> SearchByModel(CourierSearchRequestModel request)
        {

            var datas = await _rpCourierProfile.GetHosoCourrier(request.freeText, request.courierId, GlobalData.User.IDUser, request.status, request.PageNumber, request.limit, request.groupId, request.provinceId, request.salecode);
            var totalRecord = (datas != null && datas.Any()) ? datas[0].TotalRecord : 0;
            try
            {
                var profiles = _mapper.Map<List<CourierExportModel>>(datas);
                var result = DataPaging.Create(profiles, totalRecord);
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }
    }
}
