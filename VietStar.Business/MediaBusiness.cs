using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using VietStar.Business.Infrastructures;
using VietStar.Business.Interfaces;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Infrastructures;
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
        public MediaBusiness(IFileProfileRepository fileProfileRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpFile = fileProfileRepository;
        }
        public async Task<List<FileProfile>> GetProfileFileTypeByTypeAsync(string profileType, int profileId = 0, string webRootPath = null)
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
        public async Task<bool> DeleteByIdAsync(int fileId, string guidId)
        {
            return await _rpFile.DeleteByIdAsync(fileId, guidId);
        }

        public async Task<FileModel> UploadAsync(Stream stream, string key, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtensions.GetFileUploadUrl(name, webRootPath, Utility.FileUtils.GenerateProfileFolder());
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
            string fileUrl = "";
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
                        result = await UploadAsync(stream, key.ToString(), file.FileName, rootPath);
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

                if (_type.IndexOf("pdf") > 0)
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = result.Name,
                                                url = deleteURL,
                                                key =key,
                                                type="pdf",
                                                width ="120px"
                                                }
                                        },
                        append = false
                    };
                    return config;
                }
                else
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = result.Name,
                                                url = deleteURL,
                                                key =key,
                                                width ="120px"
                                            }
                                        },
                        append = false
                    };
                    return config;
                }

            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
