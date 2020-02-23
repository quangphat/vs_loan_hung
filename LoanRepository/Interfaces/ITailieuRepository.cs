using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace LoanRepository.Interfaces
{
    public interface ITailieuRepository
    {
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int type = 0);
        Task<bool> RemoveAllTailieu(int hosoId, int typeId);
        Task<bool> Add(TaiLieu model);
    }
}
