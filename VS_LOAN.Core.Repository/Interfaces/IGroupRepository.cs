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
        Task<List<NhomDropDownModel>> GetAll(int userId);
        string LayChuoiMaCha(int maNhom);
        Task<List<ThongTinToNhomModel>> LayDSNhomConAsync(int parentGroupId, int userId);
        Task<ThongTinToNhomSuaModel> LayTheoMaAsync(int maNhom);
        Task<List<OptionSimple>> LayDSThanhVienNhomAsync(int groupId, int userId);
        Task<List<OptionSimple>> LayDSKhongThanhVienNhomAsync(int groupId, int userId);
        Task<ThongTinChiTietToNhomModel> LayChiTietTheoMaAsync(int groupId);
        bool Sua(NhomModel nhom, List<int> lstThanhVien);
        List<ThongTinNhanVienModel> LayDSChiTietThanhVienNhom(int maNhom);
        List<NhomDropDownModel> LayDSCuaNhanVien(int userId);
    }
}
