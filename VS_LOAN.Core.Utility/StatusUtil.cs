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
            string result = "";
            switch (status)
            {
                case (int)HosoCourierStatus.New:
                    result = "Mới";
                    break;
                case (int)HosoCourierStatus.InProgress:
                    result = "Đang xử lý";
                    break;
                case (int)HosoCourierStatus.Deny:
                    result =  "Từ chối";
                    break;
                case (int)HosoCourierStatus.Accept:
                    result = "Chấp nhận";
                    break;
                case (int)HosoCourierStatus.Giaingan:
                    result = "Giải ngân";
                    break;
                case (int)HosoCourierStatus.Finish:
                    result = "Hoàn thành";
                    break;
                case (int)HosoCourierStatus.Cancel:
                    result = "Hủy";
                    break;
                case (int)HosoCourierStatus.Nhaplieu:
                    result = "Nhập liệu";
                    break;
                case (int)HosoCourierStatus.Thamdinh:
                    result = "Thẩm định";
                    break;
                case (int)HosoCourierStatus.BosungHoso:
                    result = "Bổ sung hồ sơ";
                    break;
                case (int)HosoCourierStatus.DaDoichieu:
                    result = "Đã đối chiếu";
                    break;
                case (int)HosoCourierStatus.PCB:
                    result = "PCB";
                    break;
                default: result= "";
                    break;
            }
            return result;
        }
    }
}
