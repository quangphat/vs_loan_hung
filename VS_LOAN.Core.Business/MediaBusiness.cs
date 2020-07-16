using Ionic.Zip;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class MediaBusiness : IMediaBusiness
    {
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly IHosoCourrierRepository _rpCourierProfile;
        protected readonly IMCeditRepository _rpMCredit;
        public MediaBusiness(ITailieuRepository tailieuRepository, 
            IMCeditRepository mCeditRepository,
            IHosoCourrierRepository hosoCourrierRepository)
        {
            _rpTailieu = tailieuRepository;
            _rpCourierProfile = hosoCourrierRepository;
            _rpMCredit = mCeditRepository;
        }
        public async Task<MediaUploadConfig> UploadSingle(Stream stream, string key, int fileId, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = GetFileUploadUrl(name, webRootPath, Utility.FileUtils.GenerateProfileFolder());
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                fileUrl = file.FileUrl;
            }
            string deleteURL = fileId <= 0 ? $"/hoso/delete?key={key}" : $"/hoso/delete/0/{fileId}";
            if (fileId > 0)
            {

                await _rpTailieu.UpdateExistingFile(new TaiLieu
                {
                    FileName = file.Name,
                    Folder = file.Folder,
                    FilePath = file.FileUrl,
                    ProfileId = 0,
                    ProfileTypeId = 0
                }, fileId);
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
        public FileModel GetFileUploadUrl(string fileInputName, string webRootPath, string folder, bool isKeepFileName = false)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = Utility.FileUtils.GenerateProfileFolder();
            }
            string fileName = isKeepFileName ? fileInputName : DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileInputName.Trim().Replace(" ", "_");
            string fullFolder = $"{webRootPath}/{folder}";
            if (!Directory.Exists(fullFolder))
                Directory.CreateDirectory(fullFolder);
            string fullPath = System.IO.Path.Combine(webRootPath, $"{folder}/{fileName}");
            return new FileModel
            {
                FileUrl = $"{Utility.FileUtils._profile_parent_folder}{folder}/{fileName}",
                Name = fileName,
                FullPath = $"{webRootPath}/{folder}/{fileName}",
                Folder = fullFolder
            };

        }
        public async Task<List<HosoCourier>> ReadXlsxFile(MemoryStream stream, int createBy)
        {
            var result = new TupleModel();
            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();

            int count = 0;
            var hosos = new List<HosoCourier>();
            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.Cells.Count > 1)
                        {
                            bool isNullRow = row.Cells.Count < 3 ? true : false;
                        }
                        var hoso = new HosoCourier()
                        {
                            CustomerName = row.Cells[0] != null ? row.Cells[0].ToString() : "",
                            Phone = row.Cells[1] != null ? row.Cells[1].ToString() : "",
                            Cmnd = row.Cells[2] != null ? row.Cells[2].ToString() : "",
                            LastNote = row.Cells[4] != null ? row.Cells[4].ToString() : "",
                            ProvinceId = row.Cells[5] != null ? Convert.ToInt32(row.Cells[5].ToString()) : 0,
                            DistrictId = row.Cells[6] != null ? Convert.ToInt32(row.Cells[6].ToString()) : 0,
                            Status = (int)HosoCourierStatus.New,
                            CreatedBy = createBy
                        };
                        var strAssignee = row.Cells[3] != null ? row.Cells[3].ToString() : "";
                        var assigneeIdsStr = string.IsNullOrWhiteSpace(strAssignee) ? new List<string>() : strAssignee.Split(',').ToList();
                        var assigneeIds = (assigneeIdsStr != null && assigneeIdsStr.Any()) ? assigneeIdsStr.Select(s => Convert.ToInt32(s)).ToList() : new List<int>();
                        hoso.AssigneeIds = assigneeIds;
                        hoso.AssignId = assigneeIds.FirstOrDefault();

                        hoso.GroupId = await _rpCourierProfile.GetGroupIdByNguoiQuanLyId(hoso.AssignId);
                        hosos.Add(hoso);
                        count++;
                    }
                }
                catch
                {
                    return hosos;
                }

            }
            return hosos;
        }
        public async Task<string> ProcessFilesToSendToMC(int profileId, string rootPath)
        {
            string mcProfileId = string.Empty;
            var profile = await _rpMCredit.GetTemProfileById(profileId);
            mcProfileId = profile.MCId;
            if (string.IsNullOrWhiteSpace(mcProfileId))
                return string.Empty;
            if (profile == null || string.IsNullOrWhiteSpace(profile.MCId))
                return string.Empty;
            var files = await _rpTailieu.GetTailieuByHosoId(profileId, (int)HosoType.MCredit);
            if (files == null || !files.Any())
                return string.Empty;
            var jsonFile = new McJsonFile();
            var x = files.Select(p => p.MC_GroupId);
            //string values = "";
            var filePaths = new List<string>();
            foreach (var f in files)
            {
                var group = jsonFile.groups.FirstOrDefault(p => p.id == f.MC_GroupId);
                if (group == null)
                {
                    group = new McJsonFileGroup { id = f.MC_GroupId, docs = new List<MCJsonFileGroupDoc>() };
                    jsonFile.groups.Add(group);
                }
                var doc = group.docs.FirstOrDefault(p => p.code == f.DocumentCode);
                if (doc == null)
                {
                    doc = new MCJsonFileGroupDoc { code = f.DocumentCode, files = new List<MCJsonFileGroupDocFile>() };
                    group.docs.Add(doc);
                }
                //var newFile = RenameFile(f.Folder, f.FileName, $"{mcProfileId}-{f.DocumentCode}-{(doc.files.Count + 1)}.jpg");
                doc.files.Add(new MCJsonFileGroupDocFile { name = f.FileName });
                
                filePaths.Add(System.IO.Path.Combine(f.Folder, f.FileName));
            }
            var jsonFileInfo = CreateJsonFile(jsonFile, mcProfileId, rootPath);
            filePaths.Add(jsonFileInfo.FullPath);
            return await CreateZipFile(filePaths, jsonFileInfo.Folder, mcProfileId);
        }
        protected FileModel CreateJsonFile(McJsonFile model, string profileId, string rootPath)
        {
            if (model == null)
                return null;
            var fileInfo = GetFileUploadUrl("info", rootPath, Utility.FileUtils.GenerateProfileFolderForMc(), true);
            if (fileInfo == null)
                return null;
            var content = JsonConvert.SerializeObject(model, Formatting.None);
            fileInfo.FullPath = $"{fileInfo.FullPath}.txt";
            Utility.FileUtils.WriteToFile(fileInfo.FullPath, content);
            return fileInfo;
        }
        protected FileModel RenameFile(string folder, string oldFileName, string newFileName)
        {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(oldFileName) || string.IsNullOrWhiteSpace(newFileName))
                return null;
            string newfile = System.IO.Path.Combine(folder, newFileName);
            string oldFile = System.IO.Path.Combine(folder, oldFileName);
            if (File.Exists(newfile))
            {
                File.Delete(newfile);
            }
            File.Move(oldFile, newfile);
            return new FileModel {
                FullPath = newfile,
                Name = newFileName
            };
        }
        protected async Task<string> CreateZipFile(IEnumerable<string> filePaths, string folder, string fileName)
        {
            if (filePaths == null || !filePaths.Any())
                return string.Empty;
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filePaths, "");
                    zip.Save(System.IO.Path.Combine(folder, $"{fileName}.zip"));
                }
            }
            catch
            {
                return string.Empty;
            }

            return $"{folder}/{fileName}.zip";
        }
        
    }
}
