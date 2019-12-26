using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business
{
    public class MediaBusiness : BaseBusiness
    {

        
        public async Task<bool> UpdateExistingFile(int fileId, string name, string url, int typeId = 1)
        {
            var p = new DynamicParameters();
            p.Add("fileId", fileId);
            p.Add("name", name);
            p.Add("url", url);
            p.Add("typeId", typeId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("updateExistingFile", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<MediaUploadConfig> UploadSingle(Stream stream, string key, int fileId, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                fileUrl = file.FileUrl;
            }
            string deleteURL = fileId <= 0 ? $"/hoso/delete?key={key}" : $"/hoso/delete/0/{fileId}";
            if (fileId > 0)
            {
                await UpdateExistingFile(fileId, file.Name, file.FileUrl);
            }

            var _type = System.IO.Path.GetExtension(name);
            var config = new MediaUploadConfig
            {
                initialPreview = fileUrl,
                initialPreviewConfig = new PreviewConfig[] {
                                    new  PreviewConfig{
                                        caption = file.Name,
                                        url = deleteURL,
                                        key =key,
                                        width ="120px"
                                        }
                                },
                append = false,
                Id = Guid.NewGuid()
            };
            return config;

            //return new MediaUploadConfig();
        }
        public  FileModel GetFileUploadUrl(string fileInputName, string webRootPath)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileInputName.Trim().Replace(" ", "_");
            string root = System.IO.Path.Combine(webRootPath, "HoSo");
            try
            {
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);
            }
            catch (Exception e)
            {
                return new FileModel
                {
                    FileUrl = "error",
                    Name = e.Message,
                    FullPath = ""
                };
            }
            string pathTemp = "";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            pathTemp = DateTime.Now.Year.ToString();
            string pathYear = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathYear))
                Directory.CreateDirectory(pathYear);
            pathTemp += "/" + DateTime.Now.Month.ToString();
            string pathMonth = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathMonth))
                Directory.CreateDirectory(pathMonth);
            pathTemp += "/" + DateTime.Now.Day.ToString();
            string pathDay = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathDay))
                Directory.CreateDirectory(pathDay);
            string path = System.IO.Path.Combine(pathDay, fileName);
            string url = "/Upload/HoSo/" + pathTemp + "/" + fileName;
            return new FileModel
            {
                FileUrl = url,
                Name = fileName,
                FullPath = path
            };

        }
    }
}
