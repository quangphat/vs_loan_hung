using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCreditService.Interfaces
{
    public interface IApiService
    {
        Task<string> GetECToken();
    }
}
