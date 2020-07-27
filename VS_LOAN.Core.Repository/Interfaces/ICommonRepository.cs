using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<OptionSimple>> GetProfileStatusByCode(string profileType, int orgId,  int roleId = 0);
    }
}
