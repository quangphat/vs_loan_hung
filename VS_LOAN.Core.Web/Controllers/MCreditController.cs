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

namespace VS_LOAN.Core.Web.Controllers
{
    public class MCreditController : BaseController
    {
        protected readonly IMCeditRepository _rpMCredit;
        protected readonly IMCreditService _svMCredit;
        protected readonly INoteRepository _rpNote;
        protected readonly IMCreditRepositoryTest _rpMcTest;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly ITailieuRepository _rpTailieu;
        public MCreditController(IMCeditRepository rpMCredit,
            INoteRepository noteRepository,
            IMCreditRepositoryTest mCreditRepositoryTest,
            IMediaBusiness mediaBusiness,
            ITailieuRepository tailieuRepository,
            IMCreditService loanContractService) : base()
        {
            _rpMCredit = rpMCredit;
            _svMCredit = loanContractService;
            _rpNote = noteRepository;
            _bizMedia = mediaBusiness;
            _rpMcTest = mCreditRepositoryTest;
            _rpTailieu = tailieuRepository;
        }

        public async Task<JsonResult> AuthenMC(AuthenMCModel model)
        {
            if (model == null)
                return ToJsonResponse(false);
            var result = await _svMCredit.AuthenByUserId(model.UserId, model.TableToUpdateIds);
            return ToJsonResponse(true, result, result);
        }
        public async Task<JsonResult> CheckSaleApi(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _svMCredit.CheckSale(GlobalData.User.IDUser, model.Value);
            return ToJsonResponse(true, result.msg != null ? result.msg.ToString() : string.Empty, result);
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
                        TypeId = NoteType.MCreditTemp
                    };
                    _rpNote.AddNoteAsync(note);
                }
            }
            return ToJsonResponse(true, "", id);
        }
        public async Task<JsonResult> UpdateDraft(MCredit_TempProfileAddModel model)
        {
            if (model == null || model.Id <= 0)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.UpdatedBy = GlobalData.User.IDUser;
            var result = await _rpMCredit.UpdateDraftProfile(profile);
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
                    TypeId = NoteType.MCreditTemp
                };
                _rpNote.AddNoteAsync(note);
            }

            //var obj = _mapper.Map<MCProfilePostModel>(profile);
            //var result = await _svMCredit.CreateProfile(obj, GlobalData.User.IDUser);
            return ToJsonResponse(true, "");
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
            var profile = await _rpMCredit.GetTemProfileById(profileId);
            if (profile == null)
                return ToJsonResponse(false, "Hồ sơ không tồn tại", new List<LoaiTaiLieuModel>());

            var data = await _svMCredit.GetFileUpload(new GetFileUploadRequest
            {
                Code = profile.ProductCode.Trim(),
                Id = "0",
                Loccode = profile.LocSignCode.Trim(),
                Issl = profile.IsAddr ? "1" : "0",
                Money = profile.LoanMoney.ToString().Replace(",0000", "")
            }, GlobalData.User.IDUser);
            if (data == null || data.Groups == null)
                return ToJsonResponse(false, "Không thể lấy file", new List<LoaiTaiLieuModel>());
            var uploadedFiles = new List<FileUploadModel>();
            if (profileId > 0)
            {
                uploadedFiles = await _rpTailieu.GetTailieuByHosoId(profileId, (int)HosoType.MCredit);
                if (uploadedFiles == null)
                    uploadedFiles = new List<FileUploadModel>();
            }
            var result = new List<LoaiTaiLieuModel>();
            foreach (var group in data.Groups)
            {
                if (group.Documents == null)
                    continue;
                foreach (var doc in group.Documents)
                {
                    var files = uploadedFiles.Where(p => p.Key == doc.Id);
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
                        Tailieus = files.ToList()
                    });
                }

            }
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> UploadFile(int key, int fileId, int orderId, int profileId, string documentName, string documentCode, int documentId, int groupId)
        {
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
                            _rpTailieu.AddMCredit(new MCTailieuSqlModel
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
                                MC_MapBpmVar = string.Empty
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
            if (model == null || model.Id <= 0)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.UpdatedBy = GlobalData.User.IDUser;
            var createJsonFile = await _bizMedia.ProcessFilesToSendToMC(model.Id, Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}"));
            return ToJsonResponse(true);
        }
    }
}