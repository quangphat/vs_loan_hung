using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class TaiLieuModel
    {
        private int _maLoai;
        private List<FileInfo> _lstFile = new List<FileInfo>();

        public int MaLoai
        {
            get
            {
                return _maLoai;
            }

            set
            {
                _maLoai = value;
            }
        }

        public List<FileInfo> LstFile
        {
            get
            {
                return _lstFile;
            }

            set
            {
                _lstFile = value;
            }
        }
    }
    public class FileInfo
    {
        private string _ten = string.Empty;
        private string _duongDan = string.Empty;

        public string Ten
        {
            get
            {
                return _ten;
            }

            set
            {
                _ten = value;
            }
        }

        public string DuongDan
        {
            get
            {
                return _duongDan;
            }

            set
            {
                _duongDan = value;
            }
        }
    }
        
}
