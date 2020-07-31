using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.HosoCourrier
{
    public class CourierExportModel : Pagination
    {
        [Description("Họ tên")]
        public string CustomerName { get; set; }
        [Description("Số điện thoại")]
        public string Phone { get; set; }
        [Description("Cmnd")]
        public string Cmnd { get; set; }
        [Description("Trạng thái")]
        public int Status { get; set; }
        [Description("Ghi chú")]
        public string LastNote { get; set; }
        [Description("SaleCode")]
        public string SaleCode { get; set; }
        [Description("Nhân viên")]
        public string AssignUser { get; set; }
        [Description("Sản phẩm")]
        public string ProductName { get; set; }
        [Description("Người tạo")]
        public string CreatedUser { get; set; }
        [Description("Tỉnh")]
        public string ProvinceName { get; set; }
        [Description("Huyện")]
        public string DistrictName { get; set; }
        [Description("Ngày tạo")]
        public string CreatedTime { get; set; }
       
    }
}
