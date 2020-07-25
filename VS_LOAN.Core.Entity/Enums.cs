using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public enum Organization
    {
        VietBank = 1,
        RevokeDebt = 2
    }
    public enum MCreditTempProfileStatus
    {
        Draft = 0,
        SubmitedTOMCredit = 2
    }
    public enum MCTableType
    {
        MCreditCity = 1,
        MCreditLoanPeriod = 2,
        MCreditlocations = 3,
        MCreditProduct = 4,
        MCreditProfileStatus = 5
    }
    public enum PartnerType
    {
        MCredit = 7
    }
    public enum NoteType
    {
        Hoso = 1,
        HosoCourrier = 2,
        Company = 3,
        MCreditTemp = 4

    }
    public enum CatType
    {
        CatA = 1,
        CatB =2,
        CatC = 3
    }
    public enum F88Result
    {
        Approve = 1,
        Deny = 2
    }
    public enum HosoType
    {
        Hoso = 1,
        HosoCourrier = 2,
        MCredit = 3,
        RevokeDebt = 5
    }
    public enum AutoID
    {
        HoSo = 1
    }
    public enum CICStatus
    {
        NotDebt = 0,// không nợ xấu
        Warning = 1, //nợ chú ý
        Debt = 2 //nợ xấu
    }
    public enum HosoCourierStatus
    {
        New = 1,
        InProgress = 2,
        Deny = 3,
        Accept = 4,
        Giaingan = 5,
        Finish = 6,
        Cancel = 7,
        Nhaplieu = 8,
        Thamdinh = 9,
        BosungHoso = 10,
        DaDoichieu = 11,
        PCB = 12
    }
    public enum TrangThaiHoSo
    {
        Nhap = 0,
        NhapLieu = 1,// Nhập liệu
        ThamDinh = 2,// Thẩm định
        TuChoi = 3,// Từ chối
        BoSungHoSo = 4,//
        GiaiNgan = 5,
        DaDoiChieu = 6,
        Cancel = 7,
        PCB = 8,
        Finish = 14
    }
    public enum KetQuaHoSo
    {
        Trong = 1,
        GoiKhachHang = 2,
        ThamDinhDiaBan = 3,
        ChoKhoanVay = 4

    }
    public enum IDDefaultDoiTac
    {
        VietCreadit = 3
    }
    public enum UserTypeEnum
    {
        Admin = 1,
        Teamlead = 2,
        Sale = 3
    }
}
