using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;

namespace VietStar.Business.Interfaces
{
    public interface ICommonBusiness
    {
        Task<List<OptionSimple>> GetStatusList();
    }
}
