using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface ITailieuBusiness
    {
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList();
        Task<bool> RemoveAllTailieu(int hosoId, int typeId);
        Task<bool> Add(TaiLieu model);
    }
}
