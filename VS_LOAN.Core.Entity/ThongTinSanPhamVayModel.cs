using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class ThongTinSanPhamVayModel
    {
        public int ID { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
    }
}
