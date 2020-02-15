using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public interface ICurrentProcess
    {
        List<ErrorMessage> Errors { get; set; }
        int UserId { get; set; }
        bool HasError { get; }
        void AddError(string message);
    }
    public class CurrentProcess : ICurrentProcess
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
