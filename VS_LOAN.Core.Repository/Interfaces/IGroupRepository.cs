using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IGroupRepository
    {
        List<NhomDropDownModel> LayDSDuyetCuaNhanVien(int userId);
        int Them(NhomModel nhom, List<int> lstThanhVien, int createdBy);
        Task<List<NhomDropDownModel>> GetAll();
        string LayChuoiMaCha(int maNhom);
        List<ThongTinToNhomModel> LayDSNhomCon(int maNhomCha);
        ThongTinToNhomSuaModel LayTheoMa(int maNhom);
        List<NhanVienNhomDropDownModel> LayDSThanhVienNhom(int maNhom);
        List<NhanVienNhomDropDownModel> LayDSKhongThanhVienNhom(int maNhom);
        ThongTinChiTietToNhomModel LayChiTietTheoMa(int maNhom);
        bool Sua(NhomModel nhom, List<int> lstThanhVien);
        List<ThongTinNhanVienModel> LayDSChiTietThanhVienNhom(int maNhom);
        List<NhomDropDownModel> LayDSCuaNhanVien(int userId);
    }
}
