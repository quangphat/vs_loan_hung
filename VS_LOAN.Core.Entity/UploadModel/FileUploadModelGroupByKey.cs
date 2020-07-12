using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.UploadModel
{
    public class FileUploadModel
    {
        public int Key { get; set; }
        public string KeyName { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int FileId { get; set; }
        public string Id { get; set; }
        public bool? IsRequire { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public int MC_DocumentId { get; set; }
        public string MC_MapBpmVar { get; set; }
        public int MC_GroupId { get; set; }
    }
    public class FileUploadModelGroupByKey
    {
        public int key { get; set; }
        public List<FileUploadModel> files { get; set; }
    }
    public class MediaUploadConfig
    {
        public string initialPreview { get; set; }
        public PreviewConfig[] initialPreviewConfig { get; set; }
        public bool append { get; set; }
        public Guid Id { get; set; }
    }
    public class PreviewConfig
    {
        public string caption { get; set; }
        public string url { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public string width { get; set; }
    }
    public class FileModel
    {
        public string FileUrl { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
    }
}
