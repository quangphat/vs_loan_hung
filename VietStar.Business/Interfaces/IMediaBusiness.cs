using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.FileProfile;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IMediaBusiness
    {
        Task<FileModel> UploadAsync(Stream stream, string key, string name, string webRootPath);
    }

}
