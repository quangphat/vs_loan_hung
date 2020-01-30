using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Employee
{
    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int RoleId { get; set; }
        public DateTime? WorkDate { get; set; }
        public string WorkDateStr { get; set; }
        public int? CreatedBy { get; set; }
        public string Code { get; set; }
    }
}
