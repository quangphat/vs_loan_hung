using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class LogBusiness : ILogBusiness
    {
        public Task<bool> LogInfo(string name, string content)
        {
            throw new NotImplementedException();
        }
    }
}
