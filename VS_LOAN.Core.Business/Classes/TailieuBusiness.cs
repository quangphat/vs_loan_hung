using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Business.Classes
{
    public class TailieuBusiness : BaseBusiness, ITailieuBusniness
    {
        protected readonly ITailieuRepository _rpTailieu;
        public TailieuBusiness(CurrentProcess currentProcess, ITailieuRepository tailieuRepository) : base(typeof(TailieuBusiness), currentProcess)
        {
            _rpTailieu = tailieuRepository;
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
