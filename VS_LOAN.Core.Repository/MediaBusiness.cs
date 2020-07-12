﻿using Dapper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Repository
{
    public class MediaBusiness : BaseRepository
    {
        public MediaBusiness() : base(typeof(MediaBusiness)) { }

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
        public FileModel GetFileUploadUrl(string fileInputName, string webRootPath)
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
        public async Task<List<HosoCourier>> ReadXlsxFile(MemoryStream stream, int createBy)
        {
            var result = new TupleModel();
            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            //if (((HSSFRow)rows.Current).Cells.Count < 3)
            //{
            //    return new TupleModel { success = false, message = "Dữ liệu không hợp lệ" };
            //}
            var bizCourier = new HosoCourrierRepository();
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
                        
                        hoso.GroupId = await bizCourier.GetGroupIdByNguoiQuanLyId(hoso.AssignId);
                        hosos.Add(hoso);
                        //if (!string.IsNullOrWhiteSpace(hoso.CustomerName) && !string.IsNullOrWhiteSpace(hoso.Phone))
                        //{
                        //    await bizCourier.Create(hoso, groupId);

                        //}
                        count++;
                    }
                }
                catch
                {
                    return hosos;
                }

            }
            return hosos;
            //result = new TupleModel
            //{
            //    success = true,
            //    message = $"Import thành công {count} dòng."
            //};
            //return result;
        }
    }
}