using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IMediaBusiness
    {
        Task<bool> ProcessFilesToSendToMC(int profileId);
        Task<MediaUploadConfig> UploadSingle(Stream stream, string key, int fileId, string name, string webRootPath);
        Task<List<HosoCourier>> ReadXlsxFile(MemoryStream stream, int createBy);
        FileModel GetFileUploadUrl(string fileInputName, string webRootPath, string folder);
        //FileModel GetFileUploadUrl(string fileInputName, string webRootPath, string documentCode, );
    }
}
