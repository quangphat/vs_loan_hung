using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface ILogBusiness
    {
        Task<bool> LogInfo(string name, string content);
    }
}
