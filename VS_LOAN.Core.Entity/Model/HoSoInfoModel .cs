using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class HoSoInfoModel
    {
        private int _id;
        private string _maHoSo;
        private string _maKhachHang;
        private string _tenKhachHang;
        private string _CMND;
        private string _diaChi;
        private int _courierCode;
        private int _maKhuVuc;
        private string _sdt;
        private string _sdt2;
        private int _gioiTinh;
        private DateTime _ngayTao;
        private int _maNguoiTao;
        private int _hoSoCuaAi;
        private DateTime _ngayCapNhat;
        private int _maNguoiCapNhat;
        private DateTime _ngayNhanDon;
        private int _maTrangThai;
        private string _tenTrangThai;
        private int _maKetQua;
        private string _ketQuaText;
        private int _coBaoHiem;
        private decimal _soTienVay;
        private int _hanVay;
        private string _tenCuaHang;
        private string _ghiChu;
        private int _sanPhamVay;
        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string MaHoSo
        {
            get
            {
                return _maHoSo;
            }

            set
            {
                _maHoSo = value;
            }
        }

        public string MaKhachHang
        {
            get
            {
                return _maKhachHang;
            }

            set
            {
                _maKhachHang = value;
            }
        }

        public string TenKhachHang
        {
            get
            {
                return _tenKhachHang;
            }

            set
            {
                _tenKhachHang = value;
            }
        }

        public string CMND
        {
            get
            {
                return _CMND;
            }

            set
            {
                _CMND = value;
            }
        }

        public string DiaChi
        {
            get
            {
                return _diaChi;
            }

            set
            {
                _diaChi = value;
            }
        }

        public int MaKhuVuc
        {
            get
            {
                return _maKhuVuc;
            }

            set
            {
                _maKhuVuc = value;
            }
        }

        public string SDT
        {
            get
            {
                return _sdt;
            }

            set
            {
                _sdt = value;
            }
        }

        public int GioiTinh
        {
            get
            {
                return _gioiTinh;
            }

            set
            {
                _gioiTinh = value;
            }
        }

        public DateTime NgayTao
        {
            get
            {
                return _ngayTao;
            }

            set
            {
                _ngayTao = value;
            }
        }

        public int MaNguoiTao
        {
            get
            {
                return _maNguoiTao;
            }

            set
            {
                _maNguoiTao = value;
            }
        }

        public DateTime NgayCapNhat
        {
            get
            {
                return _ngayCapNhat;
            }

            set
            {
                _ngayCapNhat = value;
            }
        }

        public int MaNguoiCapNhat
        {
            get
            {
                return _maNguoiCapNhat;
            }

            set
            {
                _maNguoiCapNhat = value;
            }
        }

        public DateTime NgayNhanDon
        {
            get
            {
                return _ngayNhanDon;
            }

            set
            {
                _ngayNhanDon = value;
            }
        }

        public int MaTrangThai
        {
            get
            {
                return _maTrangThai;
            }

            set
            {
                _maTrangThai = value;
            }
        }

        public int MaKetQua
        {
            get
            {
                return _maKetQua;
            }

            set
            {
                _maKetQua = value;
            }
        }

        public int CoBaoHiem
        {
            get
            {
                return _coBaoHiem;
            }

            set
            {
                _coBaoHiem = value;
            }
        }

        public int HanVay
        {
            get
            {
                return _hanVay;
            }

            set
            {
                _hanVay = value;
            }
        }

        public string TenCuaHang
        {
            get
            {
                return _tenCuaHang;
            }

            set
            {
                _tenCuaHang = value;
            }
        }

        public string GhiChu
        {
            get
            {
                return _ghiChu;
            }

            set
            {
                _ghiChu = value;
            }
        }

        public decimal SoTienVay
        {
            get
            {
                return _soTienVay;
            }

            set
            {
                _soTienVay = value;
            }
        }

        public string SDT2
        {
            get
            {
                return _sdt2;
            }

            set
            {
                _sdt2 = value;
            }
        }

        public int HoSoCuaAi
        {
            get
            {
                return _hoSoCuaAi;
            }

            set
            {
                _hoSoCuaAi = value;
            }
        }

        public int SanPhamVay
        {
            get
            {
                return _sanPhamVay;
            }

            set
            {
                _sanPhamVay = value;
            }
        }

        private List<TaiLieuModel> _lstTaiLieu = new List<TaiLieuModel>();
        public List<TaiLieuModel> LstTaiLieu
        {
            get
            {
                return _lstTaiLieu;
            }

            set
            {
                _lstTaiLieu = value;
            }
        }

        public int CourierCode
        {
            get
            {
                return _courierCode;
            }

            set
            {
                _courierCode = value;
            }
        }

        public string TenTrangThai
        {
            get
            {
                return _tenTrangThai;
            }

            set
            {
                _tenTrangThai = value;
            }
        }

        public string KetQuaText
        {
            get
            {
                return _ketQuaText;
            }

            set
            {
                _ketQuaText = value;
            }
        }
        public DateTime BirthDay { get; set; }
        public DateTime CmndDay { get; set; }
    }
}
