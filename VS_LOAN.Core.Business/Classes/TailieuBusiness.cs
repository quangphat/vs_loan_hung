using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business.Classes
{
    public class TailieuBusiness : BaseBusiness, ITailieuBusniness
    {
        protected readonly ITailieuRepository _rpTailieu;
        public TailieuBusiness(CurrentProcess currentProcess, ITailieuRepository tailieuRepository) : base(typeof(TailieuBusiness), currentProcess)
        {
            _rpTailieu = tailieuRepository;
        }
        public async Task<bool> UploadFile(int hosoId, int hosoType, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
            {
                _process.Error = errors.model_null;
                return false;
            }
            if (isReset)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieu(hosoId, hosoType);
                if (!deleteAll)
                {
                    _process.Error = errors.an_error_has_occur;
                    return false;
                }  
            }
            foreach (var item in filesGroup)
            {
                if (item.files.Any())
                {
                    foreach (var file in item.files)
                    {
                        var tailieu = new TaiLieu
                        {
                            FileName = file.FileName,
                            FilePath = file.FileUrl,
                            HosoId = hosoId,
                            TypeId = Convert.ToInt32(file.Key),
                            LoaiHoso = hosoType
                        };
                        await _rpTailieu.Add(tailieu);
                    }
                }
            }
            return true;
        }
        public async Task<bool> Add(TaiLieu model)
        {
            return await _rpTailieu.Add(model);
        }

        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int type = 0)
        {
            return await _rpTailieu.GetLoaiTailieuList(type);
        }

        public async Task<bool> RemoveAllTailieu(int hosoId, int typeId)
        {
            return await _rpTailieu.RemoveAllTailieu(hosoId, typeId);
        }
    }
}
