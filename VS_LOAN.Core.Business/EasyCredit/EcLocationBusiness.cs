using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class EcLocationBusiness : BaseBusiness, IEcLocationBusiness
    {
        protected readonly IEcLocationRepository _rpEcLocation;
        public EcLocationBusiness(CurrentProcess currentProcess, IEcLocationRepository ecLocationRepository) : base(typeof(EcLocationBusiness), currentProcess)
        {
            _rpEcLocation = ecLocationRepository;
        }
        public async Task<List<StringOptionSimple>> GetIssuePlace()
        {
            return await _rpEcLocation.GetIssuePlace();
        }
        public async Task<List<OptionSimple>> GetLocation(int type ,int parentId = 0)
        {
            
            if(type == 1)
            {
                return await _rpEcLocation.GetProvinces();
            }
            if (parentId <= 0)
            {
                _process.Error = errors.id_equal_0;
                return null;
            }
            if (type==2)
            {
                return await _rpEcLocation.GetDistricts(parentId);
            }

            return await _rpEcLocation.GetWards(parentId);
        }

        
    }
}
