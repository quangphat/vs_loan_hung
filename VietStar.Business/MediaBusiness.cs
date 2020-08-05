using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<object> UploadFileAsync(IFormFile file, int key, int fileId, string guildId, int type, int profileId, string rootPath)
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
