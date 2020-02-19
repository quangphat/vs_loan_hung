using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VietBankApi.Business.Interfaces
{
    public interface ILogBusiness
    {
        Task<bool> InfoLog(string name, string content = null);
    }
}
