using Microsoft.AspNetCore.Http;
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
        Task<List<FileProfileType>> GetProfileFileTypeByType(string profileType, int profileId = 0);
        Task<bool> DeleteByIdAsync(int fileId, string guidId);
        Task<object> UploadFileAsync(IFormFile file, int key, int fileId, string guildId, int profileId, int type, string rootPath);
        Task<FileModel> UploadAsync(Stream stream, string key, string name, string webRootPath);
    }

}
