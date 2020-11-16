using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ITailieuRepository
    {
        Task<List<ImportExcelFrameWorkModel>> GetImportTypes(int type);
        Task<List<LoaiTaiLieuModel>> LayDS();
        Task<bool> CopyFileFromProfile(int copyProfileId, int profileTypeId, int newProfileId);
        Task<List<FileUploadModel>> GetTailieuByMCId(string mcId);
        Task<bool> UpdateTailieuHosoMCId(int profileId, string mcId);
        Task<bool> UpdateExistingFile(TaiLieu taiLieu, int fileId);
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int profileType = 0);
        Task<bool> RemoveAllTailieu(int hosoId, int typeId);
        Task<bool> RemoveTailieu(int hosoId, int tailieuId);
        Task<bool> Add(TaiLieu model);
        Task<bool> AddMCredit(MCTailieuSqlModel model);
        Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId, int type);
        Task<List<FileUploadModel>> GetTailieuOCByHosoId(int hosoId, int type);
        Task<bool> RemoveTailieuOcb(int hosoId, int tailieuId);
        Task<bool> RemoveAllTailieuOcb(int hosoId, int typeId);
        Task<bool> AddOCB(TaiLieu model);
        Task<bool> UpdateExistingFileOCB(TaiLieu taiLieu, int fileId);


    }
}
