using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IHosoBusiness
    {
        Task<List<OptionSimple>> GetStatusListByType(int typeId);
        Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId, int type);
        Task<bool> RemoveTailieu(int hosoId, int tailieuId);
        Task<HoSoInfoModel> GetDetail(int id);
        Task<bool> UpdateF88Result(int hosoId, int f88Result, string reason);
    }
}
