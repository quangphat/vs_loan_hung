using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Entity
{
    public class LoaiTaiLieuModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public int BatBuoc { get; set; }
    }
    public class TaiLieu
    {
        public int FileKey { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
    }

    public class MCTailieuSqlModel : TaiLieu
    {
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public int MC_DocumentId { get; set; }
        public string MC_MapBpmVar { get; set; }
        public int MC_GroupId { get; set; }
        public int OrderId { get; set; }
    }
    public class HosoTailieu : LoaiTaiLieuModel
    {
        public List<FileUploadModel> Tailieus { get; set; }
    }
    public class MCFileUpload : LoaiTaiLieuModel
    {
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public int DocumentId { get; set; }
        public string MapBpmVar { get; set; }
        public int GroupId { get; set; }
    }
}
