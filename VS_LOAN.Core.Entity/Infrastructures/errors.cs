using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public static class errors
    {
        public const string an_error_has_occur = "Đã có lỗi xảy ra. Vui lòng thử lại";
        public const string model_null = "Dữ liệu không hợp lệ";
        public const string id_equal_0 = "Dữ liệu không hợp lệ";
        public const string customer_name_must_not_be_emty = "Tên khách hàng không được để trống";
        public const string phone_must_not_be_emty = "Sô điện thoại khách hàng không hợp lệ";
        public const string identity_number_must_not_be_emty = "Cmnd khách hàng không hợp lệ";
        public const string identity_date_invalid = "Ngày cấp chứng minh không hợp lệ";
        public const string tem_province_must_not_be_empty = "Địa chỉ tạm trú không hợp lệ";
        public const string customer_birthday_invalid = "Ngày sinh khách hàng không hợp lệ";
        public const string employee_type_must_not_be_emty = "Công việc không hợp lệ";
        public const string product_must_not_be_emty = "Sản phẩm vay không hợp lệ";
        public const string LoanAmountInvalid = "Số tiền vay không hợp lệ";
        public const string LoanTenorInvalid = "Thời hạn vay không hợp lệ";
        public const string image_selfie_missing = "Vui lòng tải lên hình ảnh cmnd/ảnh selfie";
    }
}
