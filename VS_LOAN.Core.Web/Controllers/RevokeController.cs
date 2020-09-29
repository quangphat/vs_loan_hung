using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.RevokeDebt;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Utility.OfficeOpenXML;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class RevokeController : BaseController
    {
        protected readonly IRevokeDebtBusiness _bizRevokeDebt;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly ITailieuRepository _rpTailieu;
        public RevokeController(IRevokeDebtBusiness revokeDebtBusiness, IMediaBusiness mediaBusiness, ITailieuRepository tailieuRepository) : base()
        {
            _bizRevokeDebt = revokeDebtBusiness;
            _bizMedia = mediaBusiness;
            _rpTailieu = tailieuRepository;
        }
        // GET: Revoke
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string freeText = null, string status = null, int groupId = 0, int assigneeId =0, int page = 1, int limit = 10, string fromDate = null, string toDate = null, int loaiNgay = 1, int ddlProcess =-1,int ddlProvince =-1)
        {

            DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.Now.AddDays(3);
            if (fromDate != "")
                dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
            if (toDate != "")
                dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

            var result = await _bizRevokeDebt.SearchAsync(GlobalData.User.IDUser, freeText, status, page, limit, groupId, assigneeId,dtFromDate,dtToDate,loaiNgay, ddlProcess, ddlProvince);
           
            return ToJsonResponse(true, null, result);
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
                var results = await _bizRevokeDebt.InsertFromFileAsync(fileStream, GlobalData.User.IDUser);
                return ToJsonResponse(results.IsSuccess, results.Message, results.Data);
            }

        }
        public async Task<ActionResult> Edit(int id)
        {
            var profile = await _bizRevokeDebt.GetByIdAsync(id, GlobalData.User.IDUser);
            ViewBag.model = profile;
            return View();
        }
        public async Task<JsonResult> Delete(int profileId)
        {
            if (GlobalData.User.RoleId != (int)UserTypeEnum.Admin)
                return ToJsonResponse(false, "Vui lòng liên hệ admin");
            await _bizRevokeDebt.DeleteByIdAsync(GlobalData.User.IDUser, profileId);
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> Update(int profileId, RevokeSimpleUpdate model)
        {
           
            await _bizRevokeDebt.UpdateSimpleAsync(model,GlobalData.User.IDUser, profileId);
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> Comments(int profileId)
        {
            var result = await _bizRevokeDebt.GetCommentsAsync( profileId);
            return ToJsonResponse(true,null, result);
        }
        public async Task<JsonResult> AddNote(int profileId, StringModel model)
        {
            if (model ==null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _bizRevokeDebt.AddNoteAsync(profileId, model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(result.IsSuccess,result.Message);
        }
        public async Task<JsonResult> GetFileUpload(int profileId, int profileType)
        {
            var result = await _bizMedia.GetFilesUploadByProfile(profileId, profileType);
            return ToJsonResponse(result.IsSuccess, result.Message, result.Data);
        }
        public async Task<JsonResult> UploadFile(int key,int fileId, int profileId)
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
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateProfileFolderForRevoke());
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
                                ProfileTypeId = (int)HosoType.RevokeDebt
                            }, fileId);
                        }
                        else
                        {
                            await _rpTailieu.Add(new TaiLieu
                            {
                                FileName = file.Name,
                                FileKey = key,
                                FilePath = file.FileUrl,
                                ProfileId = profileId,
                                ProfileTypeId = (int)HosoType.RevokeDebt,
                                Folder = file.Folder,
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


        public async Task<ActionResult> DownloadReportAsync(string freeText = null, string status = null, int groupId = 0, int assigneeId = 0, int page = 1, int limit = 10, string fromDate = null, string toDate = null, int loaiNgay = 1, int ddlProcess = -1, int ddlProvince = -1)
        {
         
            string newUrl = string.Empty;
            try
            {
                List<RevokeDebtSearch> rs = new List<RevokeDebtSearch>();
                DateTime dtFromDate = DateTime.Now.AddDays(-90), dtToDate = DateTime.Now;
                if (fromDate != "")
                    dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
                if (toDate != "")
                    dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

                var result = await _bizRevokeDebt.SearchAsync(GlobalData.User.IDUser, freeText, status, 1, 100000, groupId, assigneeId, dtFromDate, dtToDate, loaiNgay, ddlProcess, ddlProvince);

                rs = result.Datas;
                string destDirectory = VS_LOAN.Core.Utility.Path.DownloadBill + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/";
                bool exists = System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                if (!exists)
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + destDirectory);
                string fileName = "Report-revoke" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".xlsx";
                using (FileStream stream = new FileStream(Server.MapPath(destDirectory + fileName), FileMode.CreateNew))
                {
                    Byte[] info = System.IO.File.ReadAllBytes(Server.MapPath(VS_LOAN.Core.Utility.Path.ReportTemplate + "Report-revoke.xlsx"));
                    stream.Write(info, 0, info.Length);
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        string nameSheet = "revoke";
                        ExcelOOXML excelOOXML = new ExcelOOXML(archive);
                        int rowindex = 4;
                        if (rs != null)
                        {
                            excelOOXML.InsertRow(nameSheet, rowindex, rs.Count - 1, true);
                            for (int i = 0; i < rs.Count; i++)// dòng
                            {
                                var item = rs[i];
                                excelOOXML.SetCellData(nameSheet, "A" + rowindex, (i + 1).ToString());
                                excelOOXML.SetCellData(nameSheet, "B" + rowindex,item.Id.ToString());
                                excelOOXML.SetCellData(nameSheet, "C" + rowindex, item.AgreementNo);
                                excelOOXML.SetCellData(nameSheet, "D" + rowindex, item.IdCardNumber);
                                excelOOXML.SetCellData(nameSheet, "E" + rowindex, item.CustomerName);                              
                                excelOOXML.SetCellData(nameSheet, "F" + rowindex, item.StatusName);
                                excelOOXML.SetCellData(nameSheet, "G" + rowindex, item.TotalCurros);
                                excelOOXML.SetCellData(nameSheet, "H" + rowindex, item.PaymentAppointmentDate != null ? item.PaymentAppointmentDate.ToString() : "");
                                excelOOXML.SetCellData(nameSheet, "I" + rowindex, item.PaymentAppointmentAmount != null ? item.PaymentAppointmentAmount.ToString() : "");
                                excelOOXML.SetCellData(nameSheet, "J" + rowindex, item.AssigneeName);
                                excelOOXML.SetCellData(nameSheet, "K" + rowindex, item.UpdatedUser);
                                excelOOXML.SetCellData(nameSheet, "L" + rowindex, item.UpdatedTime.ToString());
                                rowindex++;
                            }
                        }
                        archive.Dispose();
                    }
                    stream.Dispose();
                }

                    newUrl = "/File/GetFile?path=" + destDirectory + fileName;
                    return ToResponse(true,"" ,newUrl);
                
            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message);
            }

        }
    }
}