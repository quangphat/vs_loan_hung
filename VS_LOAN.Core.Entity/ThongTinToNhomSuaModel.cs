using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class ThongTinToNhomSuaModel
    {
        public int ID { get; set; }
        public string TenNgan { get; set; }
        public string Ten { get; set; }
        public int MaNguoiQuanLy { get; set; }
        public int MaNhomCha { get; set; }
    }
}
