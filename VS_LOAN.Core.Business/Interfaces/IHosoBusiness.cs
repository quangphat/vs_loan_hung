using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IHosoBusiness
    {
        
        void TestLog(string log);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
    }
}
