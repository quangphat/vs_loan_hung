using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IHosoRepository
    {
        void TestLog(string log);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
        Task<List<OptionSimple>> GetStatusListByType(int typeId);
        Task<HoSoInfoModel> GetDetail(int id);
    }
}
