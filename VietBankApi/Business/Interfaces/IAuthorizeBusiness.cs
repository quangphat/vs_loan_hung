using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VietBankApi.Business.Interfaces
{
    public interface IAuthorizeBusiness
    {
        Task<string> GetToken();
    }
}
