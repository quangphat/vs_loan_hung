using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class HoSoDuyetModel
    {
        public int ID { get; set; }
        public string MaHoSo { get; set; }
        public DateTime NgayTao { get; set; }
        public string DoiTac { get; set; }
        public string CMND { get; set; }
        public string TenKH { get; set; }
        public int MaTrangThai { get; set; }
        public string TrangThaiHS { get; set; }
        public string KetQuaHS { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string NhanVienBanHang { get; set; }
        public string MaNVLayHS { get; set; }
        public string DoiNguBanHang { get; set; }
        public string MaNV { get; set; }
        public string MaNVSua { get; set; }
        public bool CoBaoHiem { get; set; }
        public string DiaChiKH { get; set; }
        public string GhiChu { get; set; }
        public string TenSanPham { get; set; }
        //public int F88Result { get; set; }
    }
}
