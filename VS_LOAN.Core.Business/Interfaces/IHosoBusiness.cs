using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IHosoBusiness
    {
        Task<HoSoInfoModel> GetDetail(int id);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
    }
}
