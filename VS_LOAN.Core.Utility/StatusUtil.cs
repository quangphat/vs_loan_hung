using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Utility
{
    public class StatusUtil
    {
        public static string ReturnStatusString(int status)
        {
            switch (status)
            {
                case (int)HosoCourierStatus.New:
                    return "Mới";
                case (int)HosoCourierStatus.InProgress:
                    return "Đang xử lý";
                case (int)HosoCourierStatus.Deny:
                    return "Từ chối";
                case (int)HosoCourierStatus.Accept:
                    return "Chấp nhận";
                case (int)HosoCourierStatus.Giaingan:
                    return "Giải ngân";
                case (int)HosoCourierStatus.Finish:
                    return "Hoàn thành";
                case (int)HosoCourierStatus.Cancel:
                    return "Hủy";
                case (int)HosoCourierStatus.Nhaplieu:
                    return "Nhập liệu";
                case (int)HosoCourierStatus.Thamdinh:
                    return "Thẩm định";
                case (int)HosoCourierStatus.BosungHoso:
                    return "Bổ sung hồ sơ";
                case (int)HosoCourierStatus.DaDoichieu:
                    return "Đã đối chiếu";
                case (int)HosoCourierStatus.PCB:
                    return "PCB";
                default: return "";
            }
            
        }
    }
}
