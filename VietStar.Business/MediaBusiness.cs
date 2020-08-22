using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using McreditServiceCore.Interfaces;
using Microsoft.AspNetCore.Http;
using VietStar.Business.Infrastructures;
using VietStar.Business.Interfaces;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class MediaBusiness : BaseBusiness, IMediaBusiness
    {
        protected readonly IFileProfileRepository _rpFile;
        protected readonly IMCreditRepository _rpMCredit;
        protected readonly IMCreditService _svMcredit;
        protected readonly ILogRepository _rpLog;
        public MediaBusiness(IFileProfileRepository fileProfileRepository,
            IMCreditRepository mCreditRepository,
            IMCreditService mCreditService,
            ILogRepository logRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpFile = fileProfileRepository;
            _rpMCredit = mCreditRepository;
            _rpLog = logRepository;
            _svMcredit = mCreditService;
        }
        public async Task<List<FileProfile>> GetProfileFileTypeByTypeAsync(string profileType, int profileId = 0, string webRootPath = null, string mcId = null)
        {
            if (string.IsNullOrWhiteSpace(profileType))
            {
                AddError("Dữ liệu không hợp lệ");
                return null;
            }
            profileType = profileType.ToLower();
            int type = 0;
            switch (profileType)
            {
                case "common":
                    type = (int)ProfileType.Common;
                    break;
                case "courier":
                    type = (int)ProfileType.Courier;
                    break;
                case "mcredit":
                    type = (int)ProfileType.MCredit;
                    break;
                case "company":
                    type = (int)ProfileType.Company;
                    break;
                case "revoke":
                    type = (int)ProfileType.RevokeDebt;
                    break;
            }
            if (profileType == "mcredit")
                return await GetFileUploadMcreditAsync(profileId, mcId, webRootPath);

            var fileTypes = await _rpFile.GetByType(type);
            if (fileTypes == null || !fileTypes.Any())
                return null;
            var files = await _rpFile.GetFilesByProfileIdAsync(type, profileId);
            var result = new List<FileProfile>();

            foreach (var kind in fileTypes)
            {
                var filesByType = files.Where(p => p.Key == kind.FileKey);
                //foreach (var f in filesByType)
                //{
                //    f.FileUrl = Path.Combine(webRootPath, f.FileUrl);
                //}
                var item = new FileProfile
                {
                    Id = kind.Id,
                    RootPath = webRootPath,
                    FileKey = kind.FileKey,
                    IsRequire = kind.IsRequire,
                    Name = kind.Name,
                    ProfileTypeId = kind.ProfileTypeId,
                    ProfileFiles = filesByType != null ? filesByType.ToList() : new List<FileUploadModel>()
                };
                result.Add(item);
            }
            return result;
        }

        protected async Task<List<FileProfile>> GetFileUploadMcreditAsync(int profileId, string mcId = null, string webRootPath = null)
        {
            var profile = await _rpMCredit.GetTemProfileById(profileId);
            if (!profile.success)
                return ToResponse<List<FileProfile>>(null, "Hồ sơ không tồn tại");

            var data = await _svMcredit.GetFileUpload(new GetFileUploadRequest
            {
                Code = profile.data.ProductCode.Trim(),
                Id = "0",
                Loccode = profile.data.LocSignCode.Trim(),
                Issl = profile.data.IsAddr ? "1" : "0",
                Money = profile.data.LoanMoney.ToString().Replace(",0000", "")
            });
            await _rpLog.InsertLog($"mcredit-GetFileUpload-{mcId}", data.Dump());
            if (!data.success)
                return ToResponse<List<FileProfile>>(null, data.error);
            var uploadedFiles = new List<FileUploadModel>();
            if (profileId > 0)
            {
                uploadedFiles = await _rpFile.GetFilesByProfileIdAsync((int)ProfileType.MCredit, profileId);
            }
            if (uploadedFiles == null)
                uploadedFiles = new List<FileUploadModel>();
            var result = new List<FileProfile>();
            var groups = data.data.Groups;
            foreach (var group in groups)
            {
                if (group.Documents == null)
                    continue;

                foreach (var doc in group.Documents)
                {
                    if (group.GroupId == 24 && doc.DocumentCode == "ElectricBill")
                    {
                        continue;
                    }
                    var files = uploadedFiles.Where(p => p.Key == doc.Id && p.MC_GroupId == group.GroupId);
                    result.Add(new FileProfile
                    {
                        Id = doc.Id,
                        RootPath = webRootPath,
                        IsRequire = group.Mandatory ? 1 : 0,
                        Name = doc.DocumentName,
                        DocumentId = doc.Id,
                        GroupId = group.GroupId,
                        DocumentCode = doc.DocumentCode,
                        MapBpmVar = doc.MapBpmVar,
                        ProfileId = profileId,
                        ProfileTypeId = (int)ProfileType.MCredit,
                        DocumentName = doc.DocumentName,
                        ProfileFiles = files.ToList(),
                        AllowUpload = string.IsNullOrWhiteSpace(profile.data.MCId)
                    });


                }

            }
            result = result.OrderByDescending(p => p.IsRequire == 1).ToList();
            return result;
        }

        public async Task<bool> DeleteByIdAsync(int fileId, string guidId)
        {
            return await _rpFile.DeleteByIdAsync(fileId, guidId);
        }

        public async Task<FileModel> UploadAsync(Stream stream, string key, string name, string webRootPath,string folder)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtensions.GetFileUploadUrl(name, webRootPath, folder,_process.User.Id);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                return file;
            }
        }
        public async Task<object> UploadFileAsync(IFormFile file, int key, int fileId, string guildId, int profileId, int type, string rootPath)
        {
            if (file == null || string.IsNullOrWhiteSpace(guildId))
                return null;
            var _type = string.Empty;
            string deleteURL = string.Empty;
            //string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");

            var result = null as FileModel;
            try
            {

                if (!BusinessExtensions.IsNotValidFileSize(file.Length))
                {
                    using (Stream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        result = await UploadAsync(stream, key.ToString(), file.FileName, rootPath, Utility.FileUtils.GenerateProfileFolder());
                    }

                }
                if (result != null)
                {
                    var id = await _rpFile.Add(new ProfileFileAddSql
                    {
                        FileKey = key,
                        FileName = result.Name,
                        FilePath = result.FileUrl,
                        Folder = result.Folder,
                        ProfileId = profileId,
                        ProfileTypeId = type,
                        GuildId = guildId,
                        FileId = fileId
                    });
                    deleteURL = $"/media/delete/{id.data}/{guildId}";
                }

                return CreateConfig(result, key, deleteURL);

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<object> UploadFileMcreditAsync(IFormFile file,
            string rootPath, 
            int key, 
            int fileId, 
            string guildId, 
            int profileId, 
            string documentName, 
            string documentCode, 
            int documentId, 
            int groupId, 
            string mcId = null)
        {
            if (file == null || string.IsNullOrWhiteSpace(guildId))
                return null;
            var _type = string.Empty;
            string deleteURL = string.Empty;
            var result = null as FileModel;
            try
            {

                if (!BusinessExtensions.IsNotValidFileSize(file.Length))
                {
                    using (Stream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        result = await UploadAsync(stream, key.ToString(), file.FileName, rootPath, Utility.FileUtils.GenerateProfileFolderForMc());
                    }

                }
                if (result != null)
                {
                    var id = await _rpFile.Add(new MCProfileFileSqlModel
                    {
                        FileKey = key,
                        FileName = result.Name,
                        FilePath = result.FileUrl,
                        Folder = result.Folder,
                        ProfileId = profileId,
                        ProfileTypeId = (int)ProfileType.MCredit,
                        GuildId = guildId,
                        FileId = fileId,
                        DocumentCode = documentCode,
                        DocumentName = documentName,
                        MC_DocumentId = documentId,
                        MC_GroupId = groupId,
                        OrderId = 0,
                        MC_MapBpmVar = string.Empty,
                        McId = mcId
                    });
                    deleteURL = $"/media/delete/{id.data}/{guildId}";
                }

                return CreateConfig(result, key, deleteURL);

            }
            catch (Exception e)
            {
                return null;
            }

        }


        protected object CreateConfig(FileModel model, int key, string deleteUrl)
        {
            if (model == null)
                return null;
            if (string.IsNullOrWhiteSpace(model.Extension) || model.Extension.IndexOf("pdf") <= 0)
            {
                var config = new
                {
                    initialPreview = model.FileUrl,
                    initialPreviewConfig = new[] {
                                            new {
                                                caption = model.Name,
                                                url = deleteUrl,
                                                key = key,
                                                width ="120px"
                                            }
                                        },
                    append = false
                };
                return config;
            }
            return new
            {
                initialPreview = model.FileUrl,
                initialPreviewConfig = new[] {
                                            new {
                                                caption = model.Name,
                                                url = deleteUrl,
                                                key =key,
                                                type="pdf",
                                                width ="120px"
                                                }
                                        },
                append = false
            };
        }
    }
}
