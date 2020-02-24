using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface ITailieuBusniness
    {
        Task<bool> UploadFile(int hosoId, int hosoType, bool isReset, List<FileUploadModelGroupByKey> filesGroup);
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int type = 0);
        Task<bool> RemoveAllTailieu(int hosoId, int typeId);
        Task<bool> Add(TaiLieu model);
    }
}
