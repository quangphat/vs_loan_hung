using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IHosoRepository
    {
        void TestLog(string log);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
    }
}
