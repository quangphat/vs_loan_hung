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
    public class HosoTailieu : LoaiTaiLieuModel
    {
        public List<FileUploadModel> Tailieus { get; set; }
    }
}
