using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IEcEmploymentBusiness
    {
        Task<List<StringOptionSimple>> GetEmployment(string type);
    }
}
