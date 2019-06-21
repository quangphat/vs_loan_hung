using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class SanPhamVayModel
    {
        public string ID { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int MaDoiTac { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaNguoiTao { get; set; }
        public int Loai { get; set; }
    }
}
