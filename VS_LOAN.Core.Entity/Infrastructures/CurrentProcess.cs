using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public class CurrentProcess
    {
        public List<ErrorMessage> Errors { get; set; }
        public int UserId { get; set; }
        public bool HasError { get { return Errors.Count > 0; } }
        public CurrentProcess()
        {
            Errors = new List<ErrorMessage>();
        }
        public void AddError(string message)
        {
            Errors.Add(new ErrorMessage { Message = message });
        }
    }
}
