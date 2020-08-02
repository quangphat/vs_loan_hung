using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
       
        public MediaBusiness(
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            
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
    }
}
