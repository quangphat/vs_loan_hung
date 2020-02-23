using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public class CurrentProcess
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Error { get; set; }
        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrWhiteSpace(Error);
            }
        }
    }
}
