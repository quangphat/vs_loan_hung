using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Employee
{
    public class ResetPasswordModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }
    }
}
