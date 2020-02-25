using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public enum EcHosoResult
    {
        Waiting = 0,
        Rejected = 1,
        Accepted = 2
    }
    public enum EcHosoStatus
    {
        WatingOtp = 1,
        OTPReceived = 2,
        Full = 3,
        SelectOffer = 4,
        FilesUploaded = 5,
        Finish = 6
    }
    public enum F88Result
    {
        Approve = 1,
        Deny = 2
    }
    public enum ServiceType
    {
        F88 = 1,
        EasyCredit = 2
    }
    public enum HosoType
    {
        Hoso = 1,
        HosoCourrier = 2,
        ECCredit = 3
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
        PCB = 8
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
