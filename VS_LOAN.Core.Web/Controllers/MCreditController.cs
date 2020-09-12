using MCreditService.Interfaces;
using MCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Utility;
using Newtonsoft.Json;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MCreditController : BaseController
    {
        private static IDictionary<int, string> _ignore_duplicate_documents = new Dictionary<int, string> {
            { 24, "ElectricBill" },
            { 29, "SalaryStatement" }
        };
        protected readonly IMCeditRepository _rpMCredit;
        protected readonly IMCreditService _svMCredit;
        protected readonly INoteRepository _rpNote;
        protected readonly IMCreditRepositoryTest _rpMcTest;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly ILogRepository _rpLog;
        public MCreditController(IMCeditRepository rpMCredit,
            INoteRepository noteRepository,
            IMCreditRepositoryTest mCreditRepositoryTest,
            IMediaBusiness mediaBusiness,
            ITailieuRepository tailieuRepository,
            ILogRepository logRepository,
            IEmployeeRepository employeeRepository,
            IMCreditService loanContractService) : base()
        {
            _rpMCredit = rpMCredit;
            _svMCredit = loanContractService;
            _rpNote = noteRepository;
            _bizMedia = mediaBusiness;
            _rpMcTest = mCreditRepositoryTest;
            _rpTailieu = tailieuRepository;
            _rpEmployee = employeeRepository;
            _rpLog = logRepository;
        }

        public async Task<JsonResult> AuthenMC(AuthenMCModel model)
        {
            if (model == null)
                return ToJsonResponse(false);
            var result = await _svMCredit.AuthenByUserId(model.UserId, model.TableToUpdateIds);
            return ToJsonResponse(true, result, result);
        }
        public async Task<JsonResult> CheckSaleApi(StringModel2 model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckSale(GlobalData.User.IDUser, model.Value);
            if (!string.IsNullOrWhiteSpace(model.Value2))
            {
                int profileId = Convert.ToInt32(model.Value2);
                if (profileId > 0)
                {
                    if (result.status == "success" && result.obj != null)
                    {
                        var sale = _mapper.Map<UpdateSaleModel>(result.obj);
                        _rpMCredit.UpdateSale(sale, Convert.ToInt32(model.Value2));
                    }
                }

            }

            return ToJsonResponse(result.status == "success" ? true : false, result.msg != null ? result.msg.ToString() : string.Empty, result);
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
            return ToJsonResponse(result.status == "error" ? false : true, result.msg?.ToString(), result);
        }
        public async Task<JsonResult> IsCheckCat(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _rpMCredit.IsCheckCat(model.Value);
            return ToJsonResponse(true, null, result);
        }
        public ActionResult CheckCIC()
        {
            return View();
        }
        public async Task<JsonResult> CheckCICApi(StringModel2 model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckCIC(model.Value, model.Value2, GlobalData.User.IDUser);
            return ToJsonResponse(result.status == "error" ? false : true, result.msg?.ToString(), result);
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
            return ToJsonResponse(true, result.msg?.ToString(), result);
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
            return ToJsonResponse(true, result.msg?.ToString(), result);
        }
        public ActionResult Temp()
        {
            return View();
        }

        public async Task<JsonResult> SearchTemps(string freeText, string status, int page = 1, int limit = 20, string fromDate = null, string toDate = null, int loaiNgay = 0, int manhom = 0,

          int mathanhvien = 0)
        {
            page = page <= 0 ? 1 : page;



            DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.Now.AddDays(3);
            if (fromDate != "")
                dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
            if (toDate != "")
                dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

            var profiles = await _rpMCredit.GetTempProfiles(page, limit, freeText, GlobalData.User.IDUser, status, dtFromDate, dtToDate, loaiNgay, manhom, mathanhvien = 0);
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<ProfileSearchSql>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }
        public async Task<ActionResult> Index()
        {
            //if (GlobalData.User.IDUser != (int)UserTypeEnum.Admin)
            //    return RedirectToAction("Index", "NoAuthorities");
            var isAdmin = await _rpEmployee.CheckIsAdmin(GlobalData.User.IDUser);
            if (isAdmin == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "NoAuthorities");
            }

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
        public async Task<JsonResult> CreateDraft(MCredit_TempProfileAddModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.CreatedBy = GlobalData.User.IDUser;
            var id = await _rpMCredit.CreateDraftProfile(profile);
            if (id > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    GhichuModel note = new GhichuModel
                    {
                        UserId = GlobalData.User.IDUser,
                        HosoId = id,
                        Noidung = model.LastNote,
                        CommentTime = DateTime.Now,
                        TypeId = (int)NoteType.MCreditTemp
                    };
                    await _rpNote.AddNoteAsync(note);
                }
                var peopleCanView = await _rpEmployee.GetPeopleIdCanViewMyProfile(GlobalData.User.IDUser);
                if (peopleCanView != null && peopleCanView.Any())
                {
                    peopleCanView.Add(GlobalData.User.IDUser);
                    peopleCanView.Add(1); //admin
                    var ids = string.Join(".", peopleCanView);
                    await _rpMCredit.InsertPeopleWhoCanViewProfile(id, ids);
                }
            }
            return ToJsonResponse(true, "", id);
        }
        public async Task<JsonResult> AddRefuseReasonToNote(int profileId, string type)
        {
            if (profileId <= 0 || string.IsNullOrWhiteSpace(type))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = await _svMCredit.GetProfileById(profileId.ToString(), GlobalData.User.IDUser);

            if (profile.status == "error")
            {
                return ToJsonResponse(false, profile.message);
            }
            if (profile.obj == null)
            {
                return ToJsonResponse(false, "Không tìm thấy hồ sơ");
            }
            var profileInPortal = await _rpMCredit.GetTemProfileByMcId(profile.obj.Id);
            if (profileInPortal == null)
            {
                return ToJsonResponse(false, "Không tìm thấy hồ sơ trong portal");
            }
            if (type == "refuse")
            {
                return await AddRefuseToNote(profileInPortal.Id, profile.obj);
            }
            if (type == "reason")
            {
                return await AddReasonToNote(profileInPortal.Id, profile.obj);
            }
            return ToJsonResponse(true);

        }
        protected async Task<JsonResult> AddRefuseToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && !string.IsNullOrWhiteSpace(profile.Refuse))
            {
                await _rpNote.AddNoteAsync(new GhichuModel
                {
                    HosoId = profileId,
                    CommentTime = DateTime.Now,
                    Noidung = profile.Refuse,
                    TypeId = (int)NoteType.MCreditTemp,
                    UserId = GlobalData.User.IDUser
                });
                await _rpLog.InsertLog($"AddRefuseToNote-{profileId}", profile.Refuse);
            }
            return ToJsonResponse(true);

        }
        protected async Task<JsonResult> AddReasonToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && profile.Reason != null)
            {
                var reasonName = JsonConvert.SerializeObject(profile.Reason);
                if (!string.IsNullOrWhiteSpace(reasonName))
                {
                    await _rpNote.AddNoteAsync(new GhichuModel
                    {
                        HosoId = profileId,
                        CommentTime = DateTime.Now,
                        Noidung = reasonName,
                        TypeId = (int)NoteType.MCreditTemp,
                        UserId = GlobalData.User.IDUser
                    });
                }
                await _rpLog.InsertLog($"AddReasonToNote-{profileId}", reasonName);
            }


            return ToJsonResponse(true);

        }
        public async Task<JsonResult> UpdateTempProfileStatus(int profileId)
        {
            var temp = await _rpMCredit.GetTemProfileById(profileId);
            if (temp == null)
            {
                return ToJsonResponse(false, "Không tìm thây hồ sơ trong portal");
            }
            if (string.IsNullOrWhiteSpace(temp.MCId))
            {
                return ToJsonResponse(false, "Hồ sơ chưa được đẩy qua mc");
            }
            var mcProfile = await _svMCredit.GetProfileById(temp.MCId, GlobalData.User.IDUser);
            if (mcProfile == null)
            {
                return ToJsonResponse(false, "Không tìm thấy hồ sơ phía MC");
            }
            if (mcProfile.obj == null || mcProfile.status == "error")
            {
                return ToJsonResponse(false, mcProfile.message);
            }
            int status = 0;
            try
            {
                status = Convert.ToInt32(mcProfile.obj.Status);
            }
            catch
            {
                status = temp.Status;
            }



            if (temp.Status != status)
            {
                var result = await _rpMCredit.UpdateTempProfileStatusAsync(temp.Id, status);
                if (mcProfile.obj.Reason != null)
                {
                    var reasonName = JsonConvert.SerializeObject(mcProfile.obj.Reason);
                    await _rpNote.AddNoteAsync(new GhichuModel
                    {
                        HosoId = profileId,
                        CommentTime = DateTime.Now,
                        Noidung = reasonName,
                        TypeId = (int)NoteType.MCreditTemp,
                        UserId = GlobalData.User.IDUser
                    });
                    await _rpLog.InsertLog($"AddReasonToNote-{profileId}", reasonName);
                }
                await _rpNote.AddNoteAsync(new GhichuModel
                {
                    HosoId = profileId,
                    CommentTime = DateTime.Now,
                    Noidung = mcProfile.obj.Refuse,
                    TypeId = (int)NoteType.MCreditMCECheck,
                    UserId = GlobalData.User.IDUser
                });
                await _rpLog.InsertLog($"AddRefuseToNote-{profileId}", mcProfile.obj.Refuse);
                return ToJsonResponse(result);
            }
            return ToJsonResponse(false);
        }
        public async Task<JsonResult> UpdateDraft(MCredit_TempProfileAddModel model)
        {
            if (model == null || model.Id <= 0)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.UpdatedBy = GlobalData.User.IDUser;
            var isAdmin = await _rpEmployee.CheckIsAdmin(GlobalData.User.IDUser);
            profile.Status = isAdmin ? model.Status : (int)MCreditProfileStatus.Submit;
            await _rpLog.InsertLog("mcredit-UpdateDraft", model.Dump());
            var result = await _rpMCredit.UpdateDraftProfile(profile,false);
            if (!result)
            {
                return ToJsonResponse(result, "Lỗi cập nhật");
            }

            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {
                GhichuModel note = new GhichuModel
                {
                    UserId = GlobalData.User.IDUser,
                    HosoId = model.Id,
                    Noidung = model.LastNote,
                    CommentTime = DateTime.Now,
                    TypeId = (int)NoteType.MCreditTemp
                };
                await _rpNote.AddNoteAsync(note);
            }
            var peopleCanView = await _rpEmployee.GetPeopleIdCanViewMyProfile(GlobalData.User.IDUser);
            if (peopleCanView != null && peopleCanView.Any())
            {
                peopleCanView.Add(GlobalData.User.IDUser);
                peopleCanView.Add(1); //admin
                var ids = string.Join(".", peopleCanView);
                await _rpMCredit.InsertPeopleWhoCanViewProfile(model.Id, ids);
            }
            //var obj = _mapper.Map<MCProfilePostModel>(profile);
            //var result = await _svMCredit.CreateProfile(obj, GlobalData.User.IDUser);
            return ToJsonResponse(true, "");
        }
        public async Task<ActionResult> TempProfile(int id)
        {
            var result = await _rpMCredit.GetTemProfileById(id);
            var peopleCanView = await _rpMCredit.GetPeopleCanViewMyProfile(id);
            if (peopleCanView != null && peopleCanView.Any())
            {
                if (!peopleCanView.Contains(GlobalData.User.IDUser))
                    return RedirectToAction("Temp");
            }
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;
            return View();
        }
        public async Task<ActionResult> MCreditProfile(int id)
        {
            var result = await _svMCredit.GetProfileById(id.ToString(), GlobalData.User.IDUser);
            if (result.status == "error")
            {
                return RedirectToAction("Index");
            }
            result.obj.IsAddrSame = "1";
            result.obj.IsInsurrance = "1";
            var profile = await _rpMCredit.GetTemProfileByMcId(result.obj.Id);
            if (profile == null)
            {
                return ToResponse(false, "Không tìm thấy hồ sơ tương ứng trong portal");
            }
            result.obj.LocalProfileId = profile != null ? profile.Id : 0;
            var peopleCanView = await _rpMCredit.GetPeopleCanViewMyProfile(profile.Id);
            if (peopleCanView != null && peopleCanView.Any())
            {
                if (!peopleCanView.Contains(GlobalData.User.IDUser))
                    return RedirectToAction("Index");
            }
            if (result.obj != null && result.obj.Reason != null)
            {
                result.obj.ReasonName = JsonConvert.SerializeObject(result.obj.Reason);
            }
            ViewBag.model = result.status == "success" ? result.obj : new ProfileGetByIdResponseObj();
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
            if (type == (int)MCTableType.MCreditProfileStatus)
            {
                result = await _rpMCredit.GetMCProfileStatusList();
            }
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> GetFileUpload(int profileId, int profileType = 3, string mcId = null)
        {
            var profile = null as MCredit_TempProfile;
            profile = await _rpMCredit.GetTemProfileById(profileId);
            if (profile == null)
                return ToJsonResponse(false, "Hồ sơ không tồn tại", new List<LoaiTaiLieuModel>());
            var code = profile.ProductCode != null ? profile.ProductCode.Trim() : null;
            var Loccode = profile.LocSignCode != null ? profile.LocSignCode.Trim() : null;

            var ignoreDocuments = await _rpMCredit.GetIgnoreMcDocumentAsync();
            if (ignoreDocuments == null)
                ignoreDocuments = new List<IgnoreDocumentUpload>();

            var data = await _svMCredit.GetFileUpload(new GetFileUploadRequest
            {
                Code = code,
                Id = "0",
                Loccode = Loccode,
                Issl = profile.IsAddr ? "1" : "0",
                Money = profile.LoanMoney.ToString().Replace(",0000", "")
            }, GlobalData.User.IDUser);
            await _rpLog.InsertLog($"mcredit-GetFileUpload-{mcId}", data.Dump());
            if (data == null || data.Groups == null)
                return ToJsonResponse(false, "Không thể lấy file", new List<LoaiTaiLieuModel>());
            var uploadedFiles = new List<FileUploadModel>();
            if (profileId > 0)
            {
                uploadedFiles = await _rpTailieu.GetTailieuByHosoId(profile.Id, (int)HosoType.MCredit);
            }
            if (uploadedFiles == null)
                uploadedFiles = new List<FileUploadModel>();
            var result = new List<LoaiTaiLieuModel>();
            foreach (var group in data.Groups)
            {
                if (group.Documents == null)
                    continue;

                foreach (var doc in group.Documents)
                {
                    var isIgnore = ignoreDocuments.Any(p => p.GroupId == group.GroupId && doc.DocumentCode == p.DocumentCode);
                    if(isIgnore)
                    {
                        continue;
                    }
                    var files = uploadedFiles.Where(p => p.Key == doc.Id && p.MC_GroupId == group.GroupId);
                        result.Add(new MCFileUpload
                        {
                            ID = doc.Id,
                            BatBuoc = group.Mandatory ? 1 : 0,
                            Ten = doc.DocumentName,
                            DocumentId = doc.Id,
                            GroupId = group.GroupId,
                            DocumentCode = doc.DocumentCode,
                            MapBpmVar = doc.MapBpmVar,
                            ProfileId = profileId,
                            ProfileTypeId = profileType,
                            DocumentName = doc.DocumentName,
                            Tailieus = files.ToList(),
                            AllowUpload = string.IsNullOrWhiteSpace(profile.MCId)
                       });  
                }

            }
            result = result.OrderByDescending(p => p.BatBuoc == 1).ToList();
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> UploadFile(int key, int fileId, int orderId, int profileId, string documentName, string documentCode, int documentId, int groupId, string mcId = null)
        {
            //var profile = await _rpMCredit.GetTemProfileById(profileId);
            //if(!string.IsNullOrWhiteSpace(profile.MCId) || profile == null)
            //    return Json(new { Result = string.Empty });
            string fileUrl = "";
            var _type = string.Empty;
            string deleteURL = string.Empty;
            var file = new FileModel();
            try
            {
                foreach (string f in Request.Files)
                {
                    var fileContent = Request.Files[f];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");

                        stream.Position = 0;
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateProfileFolderForMc());
                        using (var fileStream = System.IO.File.Create(file.FullPath))
                        {
                            await stream.CopyToAsync(fileStream);
                            fileStream.Close();
                            fileUrl = file.FileUrl;
                        }
                        deleteURL = fileId <= 0 ? $"/hoso/delete?key={key}" : $"/hoso/delete/0/{fileId}";
                        if (fileId > 0)
                        {
                            await _rpTailieu.UpdateExistingFile(new TaiLieu
                            {
                                FileKey = key,
                                FileName = file.Name,
                                Folder = file.Folder,
                                FilePath = file.FileUrl,
                                ProfileId = profileId,
                                ProfileTypeId = (int)HosoType.MCredit
                            }, fileId);
                        }
                        else
                        {
                            await _rpTailieu.AddMCredit(new MCTailieuSqlModel
                            {
                                FileName = file.Name,
                                FileKey = key,
                                FilePath = file.FileUrl,
                                ProfileId = profileId,
                                ProfileTypeId = (int)HosoType.MCredit,
                                DocumentCode = documentCode,
                                DocumentName = documentName,
                                MC_DocumentId = documentId,
                                MC_GroupId = groupId,
                                OrderId = orderId,
                                Folder = file.Folder,
                                MC_MapBpmVar = string.Empty,
                                McId = mcId
                            });
                        }
                        _type = System.IO.Path.GetExtension(fileContent.FileName);
                    }

                }
                if (_type.IndexOf("pdf") > 0)
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = file.Name,
                                                url = deleteURL,
                                                key =key,
                                                type="pdf",
                                                width ="120px"
                                                }
                                        },
                        append = false
                    };
                    return Json(config);
                }
                else
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = file.Name,
                                                url = deleteURL,
                                                key =key,
                                                width ="120px"
                                            }
                                        },
                        append = false
                    };
                    return Json(config);
                }
                //return Json(result);
            }
            catch (Exception)
            {
                Session["LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        public async Task<JsonResult> ProcessFile(StringModel model)
        {
            string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");
            await _bizMedia.ProcessFilesToSendToMC(Convert.ToInt32(model.Value), root);
            //var result = await _svMCredit.SendFiles(Convert.ToInt32(model.Value), System.IO.Path.Combine("D:\\Dev\\my8", "99999999.zip"), "99999999");
            return ToJsonResponse(true, "");
        }
        public async Task<JsonResult> SubmitToMCredit(MCredit_TempProfileAddModel model)
        {
            try
            {
                var isAdmin = await _rpEmployee.CheckIsAdmin(GlobalData.User.IDUser);
                if (!isAdmin)
                {
                    return ToJsonResponse(false, "Vui lòng liên hệ Admin");
                }
                if (model == null || model.Id <= 0)
                    return ToJsonResponse(false, "Dữ liệu không hợp lệ");
                if (model.SaleId <= 0)
                    return ToJsonResponse(false, "Vui lòng chọn Sale");
                var profile = await _rpMCredit.GetTemProfileById(model.Id);
                if (profile == null)
                    return ToJsonResponse(false, "Hồ sơ không tồn tại");
                var profileSql = _mapper.Map<MCredit_TempProfile>(model);
                profileSql.UpdatedBy = GlobalData.User.IDUser;
                var profileMC = _mapper.Map<MCProfilePostModel>(model);
                var files = await _rpTailieu.GetTailieuByHosoId(model.Id, (int)HosoType.MCredit);
                if (files == null || !files.Any())
                    return ToJsonResponse(false, "Vui lòng upload hồ sơ");
                var result = await _svMCredit.CreateProfile(profileMC, GlobalData.User.IDUser);
                if (result == null || result.status == "error")
                    return ToJsonResponse(false, "", result);
                profileSql.MCId = result.id;
                profile.Status = (int)MCreditProfileStatus.SentToMc;
                await _rpMCredit.UpdateDraftProfile(profileSql);
                await _rpTailieu.UpdateTailieuHosoMCId(model.Id, result.id);
                var zipFile = await _bizMedia.ProcessFilesToSendToMC(model.Id, Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}"));
                if (zipFile == "files_is_empty")
                {
                    return ToJsonResponse(false, "Vui lòng upload hồ sơ");
                }
                var sendFileResult = await _svMCredit.SendFiles(GlobalData.User.IDUser, zipFile, result.id);
                return ToJsonResponse(sendFileResult.status == "success" ? true : false, "", sendFileResult);
                //return ToJsonResponse(true, "", new { file = zipFile, id = result.id });
            }
            catch (Exception e)
            {
                await _rpLog.InsertLog("SubmitToMCredit", e.InnerException != null ? e.InnerException.Dump() : e.Dump());
                return ToJsonResponse(false);
            }

        }
        public async Task<JsonResult> Copy(int profileId)
        {
            if (profileId <= 0)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            var profile = await _rpMCredit.GetTemProfileById(profileId);
            if (profile == null)
                return ToJsonResponse(false, "Hồ sơ không tồn tại");
            profile.MCId = string.Empty;
            profile.UpdatedBy = 0;
            profile.Status = 0;
            var id = await _rpMCredit.CreateDraftProfile(profile);
            if (id > 0)
            {
                await _rpTailieu.CopyFileFromProfile(profileId, (int)HosoType.MCredit, id);
                await _rpMCredit.DeleteById(profileId);
            }
            return ToJsonResponse(id > 0 ? true : false, "", id);
        }
        public async Task<JsonResult> ReSendFileToEC(int profileId)
        {
            var profile = await _rpMCredit.GetTemProfileByMcId(profileId.ToString());
            if (profile == null || string.IsNullOrWhiteSpace(profile.MCId))
                return ToJsonResponse(false, "Hồ sơ không tồn tại hoặc chưa được gửi qua MCredit");
            var zipFile = await _bizMedia.ProcessFilesToSendToMC(profile.Id, Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}"));
            var sendFileResult = await _svMCredit.SendFiles(GlobalData.User.IDUser, zipFile, profile.MCId);

            var deleteResulft = await _bizMedia.DeleteFileZip(zipFile);
            await _rpLog.InsertLog("ReSendFileToEC", sendFileResult != null ? sendFileResult.Dump() : "ReSendFileToEC = null");
            return ToJsonResponse(sendFileResult.status == "success" ? true : false, "", sendFileResult);
        }
        public async Task<JsonResult> GetNotes(int profileId)
        {
            //var profile = await _rpMCredit.GetTemProfileById(profileId);
            //if(profile==null)
            //    return ToJsonResponse(true, null);
            var note = await _svMCredit.GetNotes(profileId.ToString(), GlobalData.User.IDUser);
            if (note == null || note.objs == null)
                return ToJsonResponse(true, "Không có dữ liệu");
            return ToJsonResponse(true, null, note.objs);
        }
        public async Task<JsonResult> AddNoteNotes(StringModel2 model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value) || string.IsNullOrWhiteSpace(model.Value2))
                return ToJsonResponse(false);
            var profile = await _rpMCredit.GetTemProfileById(Convert.ToInt32(model.Value));

            var x = await _svMCredit.AddNote(new NoteAddRequestModel
            {
                Content = model.Value2,
                Id = model.Value
            }, GlobalData.User.IDUser);

            return ToJsonResponse(true);
        }

        public async Task<JsonResult> AddNoteTempMC (StringModel3 model3)
        {
                GhichuModel note = new GhichuModel
                {
                UserId = GlobalData.User.IDUser,
                HosoId = model3.HosoId,
                Noidung = model3.Content,
                CommentTime = DateTime.Now,
                TypeId = (int)NoteType.MCreditTemp
                };
                await _rpNote.AddNoteAsync(note);
                return ToJsonResponse(true);
        }
    }
}