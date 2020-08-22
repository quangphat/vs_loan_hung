﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;

namespace VietStar.Business.Infrastructures
{
    public static class BusinessExtensions
    {
        public static FileModel GetFileUploadUrl(string fileInputName, string webRootPath, string folder, int userId, bool isKeepFileName = false)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = Utility.FileUtils.GenerateProfileFolder();
            }
            string fileName = isKeepFileName ? fileInputName : $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_{Guid.NewGuid().ToString()}_{userId}";
            string fullFolder = $"{webRootPath}/{Utility.FileUtils._profile_parent_folder}{folder}";
            if (!Directory.Exists(fullFolder))
                Directory.CreateDirectory(fullFolder);
            string fullPath = System.IO.Path.Combine(webRootPath, $"{Utility.FileUtils._profile_parent_folder}{folder}/{fileName}");
            return new FileModel
            {
                FileUrl = $"{Utility.FileUtils._profile_parent_folder}{folder}/{fileName}",
                Name = fileName,
                FullPath = $"{webRootPath}/{Utility.FileUtils._profile_parent_folder}{folder}/{fileName}",
                Folder = fullFolder,
                Extension = System.IO.Path.GetExtension(fileInputName)
            };

        }
        public static bool IsNotValidFileSize(long fileLength)
        {
            if (fileLength > Constants.MaxFileSize * 1024 * 1024)
                return true;
            return false;
        }
    }

}
