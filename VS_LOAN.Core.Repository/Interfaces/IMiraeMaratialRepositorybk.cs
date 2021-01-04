using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IMiraeMaratialRepository
    {
        Task<List<FileUploadModel>> GetTailieuMiraeHosoId(int hosoId, int type);
        Task<bool> RemoveTailieuMirae(int hosoId, int tailieuId);
        Task<bool> RemoveAllTailieuMirae(int hosoId, int typeId);
        Task<bool> AddMirae(TaiLieu model);
        Task<bool> UpdateExistingFileMirae(TaiLieu taiLieu, int fileId);
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int profileType = 0);
    }
}
