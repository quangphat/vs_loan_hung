using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> InsertLog(string name, string content);
    }
}
