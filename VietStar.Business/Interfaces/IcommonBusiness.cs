using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;

namespace VietStar.Business.Interfaces
{
    public interface ICommonBusiness
    {
        Task<List<OptionSimple>> GetStatusList(string profileType);
        Task<List<FileProfileType>> GetProfileFileTypeByType(string profileType);
        Task<object> UploadFile(IFormFile file, int key, int fileId, int type, string rootPath);
    }
}
