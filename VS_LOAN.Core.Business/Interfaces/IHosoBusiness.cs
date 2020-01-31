using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IHosoBusiness
    {
        void TestLog(string log);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
        Task<bool> RemoveTailieu(int hosoId, int tailieuId);
        Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId, int type);
        Task<List<OptionSimple>> GetStatusListByType(int typeId);
    }
}
